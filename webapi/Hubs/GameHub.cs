using System.Net.Mime;
using System.Text;
using Lib.Dtos;
using Lib.Enums;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using webapi.Hubs.Static;

namespace webapi.Hubs;

public class GameHub : Hub
{
    private static readonly ICollection<string> WaitingPlayers = new HashSet<string>();
    private static readonly IDictionary<string, bool> ReadyPlayers = new Dictionary<string, bool>();

    public async Task UpdateBoardAfterMove(string updatedBoard, string whoMadeMove, string groupName, string opponentConnectionId,
        ILogger<GameHub> logger, IHttpClientFactory httpClientFactory)
    {
        var deserializedBoard = GameLogic.DeserializeBoard(updatedBoard) ?? throw new JsonException("Deserialization for BOARD can't be performed!");
        var whoHasWon = GameLogic.SearchForTheWInner(deserializedBoard);
        
        if (whoHasWon is not null)
        {
            var httpClient = httpClientFactory.CreateClient();
            //await Clients.Caller.InvokeAsync<(DateTime, DateTime)>("MeasureTime", default);
            var gameToBeSaved = new GameToBeSavedDto()
            {

            };
            var content = new StringContent(
                JsonConvert.SerializeObject(gameToBeSaved),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            logger.LogCritical("Are we there yet: ");
            var responseMessage = await httpClient.PostAsync("/api/Game/SaveGame", content); 
            logger.LogCritical("And what we got here: " + responseMessage);
            //Jeszcze trzeba sprawdzic moze czy sie udalo

            var whoHasWonStr = JsonConvert.SerializeObject(whoHasWon);
            await Clients.Group(groupName).SendAsync("MadeMove", updatedBoard, null, whoHasWonStr);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Groups.RemoveFromGroupAsync(opponentConnectionId, groupName);

            logger.LogInformation("We have a winner.");
            return;
        }

        var isBoardFull = GameLogic.CheckIfBoardIsFull(deserializedBoard);

        if (isBoardFull)
        {
            var whoHasWonStr = JsonConvert.SerializeObject(GameResult.Draw);
            await Clients.Group(groupName).SendAsync("MadeMove", updatedBoard, null, whoHasWonStr);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Groups.RemoveFromGroupAsync(opponentConnectionId, groupName);

            logger.LogInformation("We have a draw.");
            return;
        }

        //Niepoprawny group z klienta dlatego nie bylo wiadomosci
        //Trzeba sprawdzac czy sa w grze napewno obydwoje...
        var deserializedNextMove = GameLogic.DeserializeWhoMadeMove(whoMadeMove) ?? throw new JsonException("Deserialization for NEXT_MOVE can't be performed!");
        var whoIsNext = GameLogic.WhoWillBeNext(deserializedNextMove);
        var whoIsNextStr = JsonConvert.SerializeObject(whoIsNext);
        var gameStatusStr = JsonConvert.SerializeObject(GameResult.StillInGame);

        await Clients.Group(groupName).SendAsync("MadeMove", updatedBoard, whoIsNextStr, gameStatusStr);

        logger.LogInformation("Method 'UpdateBoardAfterMove' has been invoked.");
    }

    public async Task SendDataToOpponent(int playerId, string playerUsername, string opponentConnectionId, ILogger<GameHub> logger)
    {
        await Clients.Client(opponentConnectionId).SendAsync("ReceiveOpponentDetails", playerId, playerUsername);

        logger.LogInformation("Method 'SendDataToOpponent' has been invoked.");
    }

    //GDZIE TU JEST PERFIDNY BAG BO CZASAMI POKAZUJE NA FRONCIE ZE COS ZLE CZEKA NA PLAYERA...Xd
    public async Task SendPlayerReadiness(string opponentConnectionId , string groupName, ILogger<GameHub> logger)
    {
        bool readiness;
        var playerConnectionId = Context.ConnectionId;

        lock (ReadyPlayers)
        {
            var success = ReadyPlayers.TryGetValue(playerConnectionId, out _);

            if (!success)
            {
                logger.LogError("Error: Invalid group name or player does not exist!");
                return;
            } 
            
            ReadyPlayers[playerConnectionId] = true;
            var isOpponentFound = ReadyPlayers.ContainsKey(opponentConnectionId);

            if (!isOpponentFound)
            {
                logger.LogError("Error: Invalid group name or player does not exist!");
                return;
            }

            ReadyPlayers.TryGetValue(opponentConnectionId, out readiness);
            logger.LogInformation("No to mamy użytkownika: {playerConnectionId} a readiness to: {readiness}", playerConnectionId, readiness);
        }

        if (readiness)
        {
            await Clients.Group(groupName).SendAsync("StartGame", 
                $"In game with identifier: {groupName} we have: {playerConnectionId} and {opponentConnectionId}");
            lock (ReadyPlayers)
            {
                ReadyPlayers.Remove(playerConnectionId);
                ReadyPlayers.Remove(opponentConnectionId);
            }

            logger.LogInformation("Game for group: {groupName} is starting now.", groupName);
        }
        else
        {
            logger.LogInformation("Opponent for {playerConnectionId} is still not ready.", playerConnectionId);
        }
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;

        string? randomPlayerConnectionId = null;
        var shouldGameStart = false;

        lock (WaitingPlayers)
        {
            if (WaitingPlayers.Count is not 0)
            {
                var randomPlayer = RandomGenerator.Draw(0, WaitingPlayers.Count - 1);
                randomPlayerConnectionId = WaitingPlayers.ToList()[randomPlayer];
                shouldGameStart = true;

                WaitingPlayers.Remove(randomPlayerConnectionId);
            }
            else
            {
                WaitingPlayers.Add(connectionId);
            }
        }

        if (shouldGameStart && randomPlayerConnectionId is not null)
        {
            var groupName = GameLogic.CreateUniqueGroupName();

            await Groups.AddToGroupAsync(connectionId, groupName);
            await Groups.AddToGroupAsync(randomPlayerConnectionId, groupName);

            lock (ReadyPlayers)
            {
                ReadyPlayers.Add(connectionId,false);
                ReadyPlayers.Add(randomPlayerConnectionId, false);
            }

            var (piece1, piece2) = GameLogic.DrawRandomPiece();

            await Clients.Client(connectionId).SendAsync("AssignPiece", JsonConvert.SerializeObject(piece1));
            await Clients.Client(randomPlayerConnectionId).SendAsync("AssignPiece", JsonConvert.SerializeObject(piece2));

            await Clients.Client(connectionId).SendAsync("WhoIsMyOpponent", randomPlayerConnectionId, groupName);
            await Clients.Client(randomPlayerConnectionId).SendAsync("WhoIsMyOpponent", connectionId, groupName);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        lock (WaitingPlayers)
        {
            var connectionToDelete = WaitingPlayers.FirstOrDefault(connectionId => connectionId == Context.ConnectionId);
            if (connectionToDelete is not null)
            {
                WaitingPlayers.Remove(connectionToDelete);
            }
        }

        lock (ReadyPlayers)
        {
            ReadyPlayers.Remove(Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}