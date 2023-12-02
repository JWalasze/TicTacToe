using Lib.Enums;
using Lib.TicTacToeGame;

namespace webapi.Hubs;

public interface IGameHub
{
    public Task SendCurrentBoardAsync(Board board, NextMove whoseIsNextMove);

    public Task SendAsync(string message);
}