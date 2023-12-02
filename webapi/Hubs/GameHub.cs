using Lib.Enums;
using Lib.TicTacToeGame;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace webapi.Hubs;

public class GameHub : Hub
{
    private static readonly ICollection<string> WaitingPlayers = new List<string>();

    public async Task Hello()
    {
        var serializedBoard = JsonConvert.SerializeObject(new Board().TicTacToeBoard, Formatting.Indented);
        var serializedNextMove = JsonConvert.SerializeObject(NextMove.Cross);

        Console.WriteLine(JsonConvert.SerializeObject(new Board().TicTacToeBoard, Formatting.Indented));
        Console.WriteLine(JsonConvert.SerializeObject(NextMove.Circle));

        await Clients.All.SendAsync("MadeMove", serializedBoard, serializedNextMove);
        await Clients.All.SendAsync("ReceiveMessage", "Method has been invoked!");
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;

        if (WaitingPlayers.Count != 0)
        {
            await Groups.AddToGroupAsync(connectionId, "Game");
            await Groups.AddToGroupAsync(WaitingPlayers.First(), "Game");

            Console.WriteLine("Group 'Game' has been created!");

            await Clients.Group("Game").SendAsync("GameStart", $"Group 'Game' has been created! " +
                                                              $"In game we have: {connectionId} and {WaitingPlayers.First()}");
            
            WaitingPlayers.Remove(WaitingPlayers.First());
        }
        else
        {
            WaitingPlayers.Add(connectionId);
        }

        Console.WriteLine("ConnectionId: " + connectionId);
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine(exception);
        return base.OnDisconnectedAsync(exception);
    }
}