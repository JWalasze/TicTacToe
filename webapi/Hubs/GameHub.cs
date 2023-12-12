using System.Net.Mime;
using System.Text;
using Lib.Dtos;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using webapi.Hubs.Static;

namespace webapi.Hubs;

public class GameHub : Hub
{
    private static readonly ICollection<string> WaitingPlayers = new HashSet<string>();
    private static readonly IDictionary<string, bool> ReadyPlayers = new Dictionary<string, bool>();

    public async Task UpdateBoardAfterMove(string updatedBoard, string whoMadeMove, string groupName,
        ILogger<GameHub> logger, IHttpClientFactory httpClientFactory)
    {
        var deserializedBoard = GameLogic.DeserializeBoard(updatedBoard) ?? throw new JsonException("Deserialization for BOARD can't be performed!");
        var whoHasWon = GameLogic.SearchForTheWInner(deserializedBoard);
        if (whoHasWon is not null)
        {
            var httpClient = httpClientFactory.CreateClient();
            await Clients.Caller.InvokeAsync<(DateTime, DateTime)>("MeasureTime", default);
            var gameToBeSaved = new GameToBeSavedDto()
            {

            };
            var content = new StringContent(
                JsonConvert.SerializeObject(gameToBeSaved),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var responseMessage = await httpClient.PostAsync("/api/Game/SaveGame", content); 
            //Jeszcze trzeba sprawdzic moze czy sie udalo

            await Clients.Group(groupName).SendAsync("GameOver", whoHasWon);
            return;
        }
        //Niepoprawny group z klienta dlatego nie bylo wiadomosci
        //Trzeba sprawdzac czy sa w grze napewno obydwoje...

        var deserializedNextMove = GameLogic.DeserializeWhoMadeMove(whoMadeMove) ?? throw new JsonException("Deserialization for NEXT_MOVE can't be performed!");
        var whoIsNext = GameLogic.WhoWillBeNext(deserializedNextMove);

        //Pozniej na froncie dać że jak jest info od tego co zrobil ruch to nie robic zmiany
        await Clients.Group(groupName).SendAsync("MadeMove", updatedBoard, whoIsNext, "Board has been updated!");
        logger.LogInformation("Method 'UpdateBoardAfterMove' has been invoked.");
    }

    public async Task SendPlayerDataToOpponent(string opponentConnectionId, DataForOpponent opponentData)
    {
        await Clients.Client(opponentConnectionId).SendAsync("ReceiveOpponentData", opponentData);
    }

    public async Task InformOpponentYouAreReady(string opponentConnectionId , string groupName, ILogger<GameHub> logger)
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

            await Clients.Client(connectionId).SendAsync("AssignPiece", piece1);
            await Clients.Client(randomPlayerConnectionId).SendAsync("AssignPiece", piece2);

            await Clients.Client(connectionId).SendAsync("WhoIsMyOpponent", randomPlayerConnectionId, groupName);
            await Clients.Client(randomPlayerConnectionId).SendAsync("WhoIsMyOpponent", connectionId, groupName);

            //await Clients.Group(groupName).S
            //A i jak sie robią te rzeczy to wtedy dac jakis napis ze Game is being prepared.
            //Albo {WAITNIG FOR PLAYERS TO BE READY.} w jakims popupie
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