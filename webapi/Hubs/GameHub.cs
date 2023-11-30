using Microsoft.AspNetCore.SignalR;

namespace webapi.Hubs;

public class GameHub : Hub
{
    private static readonly ICollection<string> WaitingPlayers = new List<string>(); //Trzymaloby connection Id po to żeby..

    public async Task JoinPreGameQueue(string param)
    {
        if (WaitingPlayers.Count != 0)
        {
            await Groups.AddToGroupAsync(param, "temp"); //tu dodac 1 i 2 playera oraz usunac z queue obydwoch
            await Groups.AddToGroupAsync(WaitingPlayers.First(), "temp");
            Console.WriteLine("...");

            WaitingPlayers.Remove(WaitingPlayers.First());
        }
        Console.WriteLine(param);
        WaitingPlayers.Add(param);
    }

    //public async Task LeavePreGameQueue()
    //{

    //}

    //public Task JoinGame()
    //{

    //}

    //public Task LeaveGame()
    //{

    //}
}