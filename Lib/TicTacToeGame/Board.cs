using Lib.Enums;

namespace Lib.TicTacToeGame;

public class Board
{
    public Piece[][] TicTacToeBoard;

    public Board()
    {
        TicTacToeBoard = new Piece[3][];

        for (var i = 0; i < 3; i++)
        {
            TicTacToeBoard[i] = new[] { Piece.Empty, Piece.Empty, Piece.Empty };
        }
    }

    public void SetEmptyBoard()
    {
        TicTacToeBoard = new Piece[3][];

        for (var i = 0; i < 3; i++)
        {
            TicTacToeBoard[i] = new[] { Piece.Empty, Piece.Empty, Piece.Empty };
        }
    }

    public void AddPiece(int row, int column, Piece piece)
    {
        TicTacToeBoard[row][column] = piece;
    }

    public Piece GetPiece(int row, int column)
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
                if (GetPiece(i, j) == piece)
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
                if (GetPiece(j, i) == piece)
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
            if (GetPiece(i, i) == piece)
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
            if (GetPiece(TicTacToeBoard.Length - 1 - i, i) == piece)
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