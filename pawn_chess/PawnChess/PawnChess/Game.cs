using System;
using System.Collections.Generic;

namespace PawnChess
{
    class Game
    {

        public Game(Player whitePlayer, Player blackPlayer)
        {
            WhitePlayer = whitePlayer;
            WhitePlayer.White = true;

            BlackPlayer = blackPlayer;
            BlackPlayer.White = false;

            Board = new Board();
        }
        public Board Board { get; private set; }

        public void SetBoard(char[,] board)
        {
            Board = new Board(board);
        }
        public Player WhitePlayer { get; private set; }
        public Player BlackPlayer { get; private set; }


        private bool _whiteTurn = true;
        public bool WhiteTurn => _whiteTurn;
        public bool BlackTurn => !_whiteTurn;

        public bool WhiteWon()
        {
            foreach (var pawn in Board.Whites)
            {
                if (pawn.Line == 7)
                    return true;
            }
            return false;
        }
        public bool BlackWon()
        {
            foreach (var pawn in Board.Blacks)
            {
                if (pawn.Line == 0)
                    return true;
            }
            return false;
        }

        public bool Draw()
        {
            foreach (var pawn in Board.Whites)
            {
                if (Board.PawnAt(pawn.Column + 1, pawn.Line + 1) != null || Board.PawnAt(pawn.Column - 1, pawn.Line + 1) != null)
                    return false;

                if ( Board.PawnAt(pawn.Column, pawn.Line + 1) == null )
                    return false;
            }

            foreach (var pawn in Board.Blacks)
            {
                if (Board.PawnAt(pawn.Column + 1, pawn.Line - 1) != null || Board.PawnAt(pawn.Column - 1, pawn.Line - 1) != null)
                    return false;

                if (Board.PawnAt(pawn.Column, pawn.Line - 1) == null)
                    return false;
            }

            return true;
        }

        public bool Validate(Move move)
        {
            List<Pawn> pawns = WhiteTurn ? Board.Whites : Board.Blacks;

            Pawn subjectPawn = null;
            foreach (var pawn in pawns)
            {
                if (pawn.Line == move.FromLine && pawn.Column == move.FromColumn)
                {
                    subjectPawn = pawn;
                }
            }

            if (subjectPawn == null)
            {
                return false;
            }

            //Pawn is selected;

            //Valid moves:
            //Standard Move:
            if (
                (
                    (
                        WhiteTurn
                        &&
                        move.ToLine == move.FromLine + 1
                    )
                    ||
                    (
                        BlackTurn
                        &&
                        move.ToLine == move.FromLine - 1
                    )
                )
                &&
                Board.PawnAt(move.ToColumn, move.ToLine) == null
                &&
                move.ToColumn == move.FromColumn
            )
            {
                Console.Write("standard move");
                return true;
            }

            //Skip tile (if first move)
            if (
                (
                    (
                        WhiteTurn
                        &&
                        move.ToLine == move.FromLine + 2
                        &&
                        move.FromLine == 1
                    )
                    ||
                    (
                        BlackTurn
                        &&
                        move.ToLine == move.FromLine - 2
                        &&
                        move.FromLine == 6
                    )
                )
                &&
                Board.PawnAt(move.ToColumn, move.ToLine) == null
                &&
                move.ToColumn == move.FromColumn
            )
            {
                Console.Write("skip move");
                return true;
            }

            //Capture
            if (
                (
                    WhiteTurn
                    &&
                    move.ToLine == move.FromLine + 1
                    &&
                    (
                        move.ToColumn == move.FromColumn + 1
                        ||
                        move.ToColumn == move.FromColumn - 1
                    )
                    &&
                    Board.BlackAt(move.ToColumn, move.ToLine) != null
                        
                )
                ||
                (
                    BlackTurn
                    &&
                    move.ToLine == move.FromLine - 1
                    &&
                    (
                        move.ToColumn == move.FromColumn + 1
                        ||
                        move.ToColumn == move.FromColumn - 1
                    )
                    &&
                    Board.WhiteAt(move.ToColumn, move.ToLine) != null
                )
            )
            {
                Console.Write("capture");
                return true;
            }

            //Capture (En Passant)
            if (
                (
                    WhiteTurn
                    &&
                    move.ToLine == move.FromLine + 1
                    &&
                    (
                        move.ToColumn == move.FromColumn + 1
                        ||
                        move.ToColumn == move.FromColumn - 1
                    )
                    &&
                    Board.BlackAt(move.ToColumn, move.ToLine - 1) != null
                    &&
                    move.ToLine == 5
                    &&
                    Board.BlackAt(move.ToColumn, move.ToLine - 1).LastLine == 6

                )
                ||
                (
                    BlackTurn
                    &&
                    move.ToLine == move.FromLine - 1
                    &&
                    (
                        move.ToColumn == move.FromColumn + 1
                        ||
                        move.ToColumn == move.FromColumn - 1
                    )
                    &&
                    Board.WhiteAt(move.ToColumn, move.ToLine + 1) != null
                    &&
                    move.ToLine == 2
                    &&
                    Board.WhiteAt(move.ToColumn, move.ToLine + 1).LastLine == 1
                )
            )
            {
                Console.Write("capture (en passant)");
                return true;
            }

            return false;
        }

        public void Execute(Move move)
        {
            if (!Validate(move))
            {
                throw new ArgumentException("Invalid move");
            }

            if (Board.PawnAt(move.ToColumn, move.ToLine) == null && move.FromColumn != move.ToColumn)
            {
                Console.WriteLine("AN PAISSANT EH");
                Pawn pawn = WhiteTurn
                    ? Board.PawnAt(move.ToColumn, move.ToLine - 1)
                    : Board.PawnAt(move.ToColumn, move.ToLine + 1);
                Board.Whites.Remove(pawn);
                Board.Blacks.Remove(pawn);
            }

            Board.Whites.Remove(Board.PawnAt(move.ToColumn, move.ToLine));
            Board.Blacks.Remove(Board.PawnAt(move.ToColumn, move.ToLine));

            Board.PawnAt(move.FromColumn, move.FromLine).move(move.ToColumn, move.ToLine);
        }

        public void GetMoveFromPlayer()
        {
            try
            {
                Move move = WhiteTurn ? WhitePlayer.GetMove(Board) : BlackPlayer.GetMove(Board);
                Execute(move);
            }
            catch (ArgumentException)
            {
                GetMoveFromPlayer();
            }
        }

        public void Start()
        {
            while (
                !WhiteWon() &&
                !BlackWon() &&
                !Draw()
            )
            {
                Console.Write(Board.GetGraphics());
                Console.Write(WhiteTurn ? "White's turn!" : "Black's Turn!");
                GetMoveFromPlayer();

                _whiteTurn = _whiteTurn ? false : true;
            }

            Console.Write(Board.GetGraphics());

            if (WhiteWon())
                Console.Write("\n\nWhite Won!");

            if (BlackWon())
                Console.Write("\n\nBlack Won!");

            if (Draw())
                Console.Write("\n\nDraw!");
        }
    }
}
