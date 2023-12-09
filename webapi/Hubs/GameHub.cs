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

        var deserializedNextMove = GameLogic.DeserializeNextMove(whoMadeMove) ?? throw new JsonException("Deserialization for NEXT_MOVE can't be performed!");
        var whoIsNext = GameLogic.WhoWillBeNext(deserializedNextMove);

        //Pozniej na froncie dać że jak jest info od tego co zrobil ruch to nie robic zmiany
        await Clients.Group(groupName).SendAsync("MadeMove", updatedBoard, whoIsNext, "Board has been updated!");
        logger.LogInformation("Method 'UpdateBoardAfterMove' has been invoked.");
    }

    public async Task SendPlayerDataToTheOpponent(string opponentConnectionId, DataForOpponent opponentData)
    {
        await Clients.Client(opponentConnectionId).SendAsync("ReceiveOpponentData", opponentData);
    }

    public async Task StartTheGame()
    {
        //await Client
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var randomGenerator = new Random();

        string? randomPlayerConnectionId = null;
        var shouldGameStart = false;

        lock (WaitingPlayers)
        {
            if (WaitingPlayers.Count is not 0)
            {
                var randomPlayer = randomGenerator.Next(0, WaitingPlayers.Count - 1);
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

            //Invoke methode to po prostu zapisze juz na poczatku do bazy!!! juz zalazek
            //ALbo czekaj...InvokeAsync żeby dostać informacje o grze????
            //var player1Info = await Clients.Client(connectionId).InvokeAsync<string>("GetPlayerInfo", default);
            //var player2Info = await Clients.Client(randomPlayerConnectionId).InvokeAsync<string>("GetPlayerInfo", default);
            //Console.WriteLine(player1Info);
            //Console.WriteLine(player2Info);
            //i pozniej zapisać to co bedzie zwrocone...

            //Wysylamy zwykly sendAsync i wtedy odbieramy na froncie a oni wysylają info o graczach...
            //jeszcze losowanie kto kolko a kto krzyzyk...
            //osobne calle zeby jeden byl kolko a jeden krzyzyk
            var (piece1, piece2) = GameLogic.DrawRandomPiece();
            await Clients.Client(connectionId).SendAsync("DrawPiece", piece1);
            await Clients.Client(randomPlayerConnectionId).SendAsync("DrawPiece", piece2);

            await Clients.Group(groupName).S
            //A i jak sie robią te rzeczy to wtedy dac jakis napis ze Game is being prepared.
            await Clients.Group(groupName).SendAsync("StartGame", $"Group {groupName} has been created! " +
                                                                  $"In game we have: {connectionId} and {randomPlayerConnectionId}",
                groupName);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        lock (WaitingPlayers)
        {
            var connectionsToDelete = WaitingPlayers.FirstOrDefault(connectionId => connectionId == Context.ConnectionId);
            if (connectionsToDelete is not null)
            {
                WaitingPlayers.Remove(connectionsToDelete);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}