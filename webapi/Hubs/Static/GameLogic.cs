using Lib.Dtos;
using Lib.Enums;
using Lib.TicTacToeGame;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace webapi.Hubs.Static;

public static class GameLogic
{
    public static string CreateUniqueGameId()
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

    public static Piece? SearchForTheWinner(TicTacToe ticTacToe)
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

    public static GameStatus ReturnGameStatusBasedOnWhoWon(Piece piece)
    {
        return piece == Piece.Circle ? GameStatus.WonByCircle : GameStatus.WonByCross;
    }

    public static int MatchWinnerPieceToPlayerId(Piece piece, GameInfo gameInfo)
    {
        if (gameInfo.OpponentId is null || gameInfo.PlayerId is null)
        {
            throw new Exception("Error: Incorrect gameInfo values!");
        }

        return piece switch
        {
            Piece.Circle when gameInfo.PlayerPiece == Piece.Circle => (int)gameInfo.PlayerId,
            Piece.Circle when gameInfo.OpponentPiece == Piece.Circle => (int)gameInfo.OpponentId,
            Piece.Cross when gameInfo.PlayerPiece == Piece.Cross => (int)gameInfo.PlayerId,
            Piece.Cross when gameInfo.OpponentPiece == Piece.Cross => (int)gameInfo.OpponentPiece,
            _ => throw new ArgumentOutOfRangeException(nameof(piece), piece, "Error during matching a winner!")
        };
    }
}