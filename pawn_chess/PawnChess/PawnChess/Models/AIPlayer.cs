using System;
using System.Collections.Generic;

namespace PawnChess.Models
{
    class AIPlayer : Player
    {
        public bool White { get; set; } //daca nu-i alba ii neagra
        public Move GetMove(Board board)
        {
            
            Pawn opponentsRushPawn = ShortestRush(board, true);
            Pawn rushPawn = ShortestRush(board);


            //Adopt a defensive stance if needed
            if(opponentsRushPawn != null// && 
                //(
                //    (rushPawn != null && opponentsRushPawn.moves > rushPawn.moves)
                //    || rushPawn == null
                //)
            )
            {

                Console.Write("IFFFFFFFFFFFFFFFFFFFFFF");

                foreach (var pawn in White ? board.Whites : board.Blacks)
                {
                    Console.Write("\n\n" + pawn.Line + " " + pawn.Column + " : " + opponentsRushPawn.Line + " " + opponentsRushPawn.Column + "\n\n");
                    if (
                        pawn.Line == opponentsRushPawn.Line + (White ? -1 : 1)
                        &&
                        (
                            pawn.Column == opponentsRushPawn.Column + 1 ||
                            pawn.Column == opponentsRushPawn.Column - 1
                        )
                    )
                    {
                        Console.Write("IFFFFFFFFFFFFFFFFFFFFFFEEEEEEEEEEEEEEAEAEFAFAEF");
                        return new Move(pawn.Column, pawn.Line, opponentsRushPawn.Column, opponentsRushPawn.Line);
                    }
                }
            }

            //Attempt a rush if it is possible;
            if (rushPawn != null)
            {
                return new Move(rushPawn.Column, rushPawn.Line, rushPawn.Column, rushPawn.Line + (White ? 1 : -1));
            }

            return GetRandomMove(board);
        }

        private Move GetRandomMove(Board board)
        {
            List<Pawn> pawns = White ? board.Whites : board.Blacks;

            Random r = new Random(DateTime.Now.Millisecond);
            int rInt = r.Next(0, pawns.Count);

            Pawn pawn = pawns[rInt];

            //Console.Write("\n" + pawn.Line + " " + pawn.Column + "\n");
            int lineOffset; //1 sau 2
            if (pawn.Line == (White ? 1 : 6))
                lineOffset = r.Next(1, 3);
            else
                lineOffset = 1;

            int columnOffset = r.Next(-1, 2); //-1 0 1

            if (!White)
            {
                lineOffset *= -1;
                columnOffset *= -1;
            }

            return new Move(pawn.Column, pawn.Line, pawn.Column + columnOffset, pawn.Line + lineOffset);
        }

        private Pawn ShortestRush(Board board, bool opponent = false)
        {
            int minRushLength = 99;
            Pawn bestRushablePawn = null;

            List<Pawn> pawns = White ? board.Whites : board.Blacks;

            foreach (Pawn pawn in pawns)
            {
                int rushLength = 0;
                bool rushable = true;

                if ( (White && !opponent) || (!White && opponent))
                { //Look for white shortest path
                    //Console.Write("\nnew pawn: ");
                    for (int i = pawn.Line + 1; i < 8; i++)
                    {

                        if (
                            !(
                                board.PawnAt(pawn.Column, i) != null
                                ||
                                (board.BlackAt(pawn.Column - 1, i) != null && i != pawn.Line + 1)
                                ||
                                (board.BlackAt(pawn.Column + 1, i) != null && i != pawn.Line + 1)
                            )
                        )
                        {
                            rushLength++;
                        }
                        else
                        {
                            rushable = false;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = pawn.Line - 1; i >= 0; i--)
                    {
                        if (
                            !(
                                board.PawnAt(pawn.Column, i) != null
                                ||
                                (board.WhiteAt(pawn.Column - 1, i) != null && i != pawn.Line - 1 && !opponent)
                                ||
                                (board.WhiteAt(pawn.Column + 1, i) != null && i != pawn.Line - 1 && !opponent)
                            )
                        )
                        {
                            rushLength++;
                        }
                        else
                        {
                            rushable = false;
                            break;
                        }
                    }
                }

                if (rushable && rushLength < minRushLength)
                {
                    bestRushablePawn = pawn;
                }
            }

            return bestRushablePawn;
        }
    }
}
