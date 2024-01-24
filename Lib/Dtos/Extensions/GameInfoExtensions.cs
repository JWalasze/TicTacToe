namespace Lib.Dtos.Extensions;

public static class GameInfoExtensions
{
    public static bool HaveNullValues(this GameInfo gameInfo)
    {
        return gameInfo.GameId is null || 
               gameInfo.OpponentConnectionId is null ||
               gameInfo.OpponentId is null || 
               gameInfo.OpponentPiece is null || 
               gameInfo.OpponentUsername is null ||
               gameInfo.PlayerId is null || 
               gameInfo.PlayerUsername is null || 
               gameInfo.PlayerPiece is null;
    }
}