using Lib.Enums;
using Lib.TicTacToeGame;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace webapi.Hubs.Static;

public static class GameLogic
{
    public static string CreateUniqueGroupName()
    {
        var groupName = Guid.NewGuid().ToByteArray();
        var encodedGroupName = WebEncoders.Base64UrlEncode(groupName);
        return "Game:" + encodedGroupName;
    }

    public static NextMove WhoWillBeNext(NextMove whoMadeMove)
    {
        return whoMadeMove is NextMove.Circle ? NextMove.Circle : NextMove.Cross;
    }

    public static NextMove? SearchForTheWInner(Board board)
    {
        if (board.CheckIfCircleWon())
        {
            return NextMove.Circle;
        }

        if (board.CheckIfCrossWon())
        {
            return NextMove.Cross;
        }

        return null;
    }

    public static Board? DeserializeBoard(string board)
    {
        return JsonConvert.DeserializeObject<Board>(board);
    }

    public static NextMove? DeserializeNextMove(string nextMove)
    {
        return JsonConvert.DeserializeObject<NextMove>(nextMove);
    }
}