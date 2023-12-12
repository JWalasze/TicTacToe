using Lib.Enums;

namespace Lib.TicTacToeGame;

public class Board
{
    public Tile[][] TicTacToeBoard;

    public Board()
    {
        TicTacToeBoard = new Tile[3][];

        for (var i = 0; i < 3; i++)
        {
            TicTacToeBoard[i] = new[] { Tile.Empty, Tile.Empty, Tile.Empty };
        }
    }

    public void SetEmptyBoard()
    {
        TicTacToeBoard = new Tile[3][];

        for (var i = 0; i < 3; i++)
        {
            TicTacToeBoard[i] = new[] { Tile.Empty, Tile.Empty, Tile.Empty };
        }
    }

    public void AddPiece(int row, int column, Piece tile)
    {
        TicTacToeBoard[row][column] = (Tile)tile;
    }

    public Tile GetTile(int row, int column)
    {
        return TicTacToeBoard[row][column];
    }

    public bool CheckIfCrossWon()
    {
        return CheckIfPieceWon(Piece.Cross);
    }

    public bool CheckIfCircleWon()
    {
        return CheckIfPieceWon(Piece.Circle);
    }

    private bool CheckIfPieceWon(Piece piece)
    {
        return CheckInColumn(piece) || CheckDiagonally(piece) || CheckInRow(piece);
    }

    private bool CheckInRow(Piece piece)
    {
        var counter = 0;

        for (var i = 0; i < TicTacToeBoard.Length; i++)
        {
            for (var j = 0; j < TicTacToeBoard.Length; j++)
            {
                if (GetTile(i, j) == (Tile)piece)
                {
                    ++counter;
                }
            }

            if (counter == TicTacToeBoard.Length)
            {
                return true;
            }

            counter = 0;
        }

        return false;
    }

    private bool CheckInColumn(Piece piece)
    {
        var counter = 0;

        for (var i = 0; i < TicTacToeBoard.Length; i++)
        {
            for (var j = 0; j < TicTacToeBoard.Length; j++)
            {
                if (GetTile(j, i) == (Tile)piece)
                {
                    ++counter;
                }
            }

            if (counter == TicTacToeBoard.Length)
            {
                return true;
            }

            counter = 0;
        }

        return false;
    }

    private bool CheckDiagonally(Piece piece)
    {
        var counter = 0;

        for (var i = 0; i < TicTacToeBoard.Length; i++)
        {
            if (GetTile(i, i) == (Tile)piece)
            {
                ++counter;
            }
        }

        if (counter == TicTacToeBoard.Length)
        {
            return true;
        }

        counter = 0;

        for (var i = 0; i < TicTacToeBoard.Length; i++)
        {
            if (GetTile(TicTacToeBoard.Length - 1 - i, i) == (Tile)piece)
            {
                ++counter;
            }
        }

        if (counter == TicTacToeBoard.Length)
        {
            return true;
        }

        return false;
    }

    public void PrintBoard()
    {
        foreach (var piece in TicTacToeBoard)
        {
            foreach (var p in piece)
            {
                Console.Write(p + "\t");
            }

            Console.WriteLine();
        }
    }
}