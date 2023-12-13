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
        return encodedGroupName;
    }

    public static Piece WhoWillBeNext(Piece whoMadeMove)
    {
        return whoMadeMove is Piece.Circle ? Piece.Cross : Piece.Circle;
    }

    public static (Piece Piece1, Piece Piece2) DrawRandomPiece()
    {
        var randomNumber = RandomGenerator.Draw(0, 2);
        return randomNumber == 0 ? (Piece.Circle, Piece.Cross) : (Piece.Cross, Piece.Circle);
    }

    public static bool CheckIfBoardIsFull(TicTacToe ticTacToe)
    {
        return ticTacToe.CheckIfBoardIsFull();
    }

    public static Piece? SearchForTheWInner(TicTacToe ticTacToe)
    {
        if (ticTacToe.CheckIfCircleWon())
        {
            return Piece.Circle;
        }

        if (ticTacToe.CheckIfCrossWon())
        {
            return Piece.Cross;
        }

        return null;
    }

    public static TicTacToe? DeserializeBoard(string board)
    {
        return JsonConvert.DeserializeObject<TicTacToe>(board);
    }

    public static Piece? DeserializeWhoMadeMove(string whoMadeMove)
    {
        return JsonConvert.DeserializeObject<Piece>(whoMadeMove);
    }
}