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
        var randomNumber = RandomGenerator.Draw(0, 1);
        return randomNumber == 0 ? (Piece.Circle, Piece.Cross) : (Piece.Cross, Piece.Circle);
    }

    public static Piece? SearchForTheWInner(Board board)
    {
        if (board.CheckIfCircleWon())
        {
            return Piece.Circle;
        }

        if (board.CheckIfCrossWon())
        {
            return Piece.Cross;
        }

        return null;
    }

    public static Board? DeserializeBoard(string board)
    {
        return JsonConvert.DeserializeObject<Board>(board);
    }

    public static Piece? DeserializeWhoMadeMove(string whoMadeMove)
    {
        return JsonConvert.DeserializeObject<Piece>(whoMadeMove);
    }
}