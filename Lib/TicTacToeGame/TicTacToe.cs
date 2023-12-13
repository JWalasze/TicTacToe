using Lib.Enums;

namespace Lib.TicTacToeGame;

public class TicTacToe
{
    public Tile[][] Board;

    public TicTacToe()
    {
        Board = new Tile[3][];

        for (var i = 0; i < 3; i++)
        {
            Board[i] = new[] { Tile.Empty, Tile.Empty, Tile.Empty };
        }
    }

    public void SetEmptyBoard()
    {
        Board = new Tile[3][];

        for (var i = 0; i < 3; i++)
        {
            Board[i] = new[] { Tile.Empty, Tile.Empty, Tile.Empty };
        }
    }

    public void AddPiece(int row, int column, Piece tile)
    {
        Board[row][column] = (Tile)tile;
    }

    public Tile GetTile(int row, int column)
    {
        return Board[row][column];
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

        for (var i = 0; i < Board.Length; i++)
        {
            for (var j = 0; j < Board.Length; j++)
            {
                if (GetTile(i, j) == (Tile)piece)
                {
                    ++counter;
                }
            }

            if (counter == Board.Length)
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

        for (var i = 0; i < Board.Length; i++)
        {
            for (var j = 0; j < Board.Length; j++)
            {
                if (GetTile(j, i) == (Tile)piece)
                {
                    ++counter;
                }
            }

            if (counter == Board.Length)
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

        for (var i = 0; i < Board.Length; i++)
        {
            if (GetTile(i, i) == (Tile)piece)
            {
                ++counter;
            }
        }

        if (counter == Board.Length)
        {
            return true;
        }

        counter = 0;

        for (var i = 0; i < Board.Length; i++)
        {
            if (GetTile(Board.Length - 1 - i, i) == (Tile)piece)
            {
                ++counter;
            }
        }

        if (counter == Board.Length)
        {
            return true;
        }

        return false;
    }

    public bool CheckIfBoardIsFull()
    {
        this.PrintBoard();
        for (var i = 0; i < Board.Length; i++)
        {
            for (var j = 0; j < Board.Length; j++)
            {
                if (GetTile(i, j) == Tile.Empty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void PrintBoard()
    {
        foreach (var piece in Board)
        {
            foreach (var p in piece)
            {
                Console.Write(p + "\t");
            }

            Console.WriteLine();
        }
    }
}