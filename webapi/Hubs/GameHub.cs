using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Lib.Dtos;
using Lib.Enums;
using Lib.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using webapi.Hubs.Static;

namespace webapi.Hubs;

public class GameHub : Hub
{
    private static readonly ICollection<string> WaitingPlayers = new HashSet<string>();
    private static readonly IDictionary<string, InitialPlayerInfo> ReadyPlayers = new Dictionary<string, InitialPlayerInfo>();

    public async Task UpdateBoardAfterMove(string updatedBoard, string whoMadeMove, string groupName, string opponentConnectionId,
        ILogger<GameHub> logger, IHttpClientFactory httpClientFactory)
    {
        var deserializedBoard = GameLogic.DeserializeBoard(updatedBoard) ?? throw new JsonException("Deserialization for BOARD can't be performed!");
        var whoHasWon = GameLogic.SearchForTheWInner(deserializedBoard);
        
        if (whoHasWon is not null)
        {
            /*var httpClient = httpClientFactory.CreateClient();
            logger.LogCritical("Are we there yet: ");
            var content = new StringContent(
                JsonConvert.SerializeObject(new {GameId = 18, WinnerId = 1}),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            var responseMessage = await httpClient.PatchAsync("http://localhost:5285/api/Game/UpdateGameForWinner?gameId=18&winnerId=1", content);
            logger.LogCritical("And what we got here: " + responseMessage);*/

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
        lock (ReadyPlayers)
        {
            ReadyPlayers[Context.ConnectionId].PlayerId = playerId;
        }

        await Clients.Client(opponentConnectionId).SendAsync("ReceiveOpponentDetails", playerId, playerUsername);

        logger.LogInformation("Method 'SendDataToOpponent' has been invoked.");
    }

    //GDZIE TU JEST PERFIDNY BAG BO CZASAMI POKAZUJE NA FRONCIE ZE COS ZLE CZEKA NA PLAYERA...Xd
    public async Task SendPlayerReadiness(string opponentConnectionId , string groupName, 
        ILogger<GameHub> logger, IHttpClientFactory httpClientFactory)
    {
        InitialPlayerInfo readiness;
        var playerConnectionId = Context.ConnectionId;

        lock (ReadyPlayers)
        {
            var success = ReadyPlayers.TryGetValue(playerConnectionId, out _);

            if (!success)
            {
                logger.LogError("Error: Invalid group name or player does not exist!");
                return;
            } 
            
            ReadyPlayers[playerConnectionId].IsReady = true;
            var isOpponentFound = ReadyPlayers.ContainsKey(opponentConnectionId);

            if (!isOpponentFound)
            {
                logger.LogError("Error: Invalid group name or player does not exist!");
                return;
            }

            ReadyPlayers.TryGetValue(opponentConnectionId, out readiness);
            logger.LogInformation("No to mamy użytkownika: {playerConnectionId} a readiness to: {readiness}", playerConnectionId, readiness);
        }

        if (readiness is not null && readiness.IsReady)
        {
            await Clients.Group(groupName).SendAsync("StartGame", 
                $"In game with identifier: {groupName} we have: {playerConnectionId} and {opponentConnectionId}");

            GameToBeSavedDto gameToBeSaved;
            lock (ReadyPlayers)
            {
                gameToBeSaved = new GameToBeSavedDto()
                {
                    Player1Id = ReadyPlayers[opponentConnectionId].PlayerId,
                    Player2Id = ReadyPlayers[playerConnectionId].PlayerId
                };
                
                ReadyPlayers.Remove(playerConnectionId);
                ReadyPlayers.Remove(opponentConnectionId);
            }

            /*var content = new StringContent(
                JsonConvert.SerializeObject(gameToBeSaved),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            var httpClient = httpClientFactory.CreateClient();
            logger.LogCritical("Are we there yet: ");
            var responseMessage = await httpClient.PostAsync("http://localhost:5285/api/Game/SaveGame", content);
            logger.LogCritical("And what we got here: " + responseMessage);*/

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
                ReadyPlayers.Add(connectionId, new InitialPlayerInfo());
                ReadyPlayers.Add(randomPlayerConnectionId, new InitialPlayerInfo());
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