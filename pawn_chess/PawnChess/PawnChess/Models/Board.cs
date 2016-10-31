using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PawnChess
{
    class Board
    {
        public Board()
        {
            Whites = new List<Pawn>();
            Blacks = new List<Pawn>();

            for (int i = 0; i < 8; i++)
            {
                Whites.Add(new Pawn(i, 1));
                Blacks.Add(new Pawn(i, 6));
            }
        }

        public Board(char[,] board)
        {
            Whites = new List<Pawn>();
            Blacks = new List<Pawn>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(board[i,j] == 'W')
                        Whites.Add(new Pawn(j,i));
                    if (board[i, j] == 'B')
                        Blacks.Add(new Pawn(j, i));
                }
            }
        }
        public List<Pawn> Whites { get; private set; }
        public List<Pawn> Blacks { get; private set; }

        public Pawn WhiteAt(int column, int line)
        {
            foreach (var pawn in Whites)
            {
                if (pawn.Column == column && pawn.Line == line)
                {
                    return pawn;
                }
            }
            return null;
        }
        public Pawn BlackAt(int column, int line)
        {
            foreach (var pawn in Blacks)
            {
                if (pawn.Column == column && pawn.Line == line)
                {
                    return pawn;
                }
            }
            return null;
        }
        public Pawn PawnAt(int column, int line)
        {
            return WhiteAt(column, line) == null ? BlackAt(column, line) : WhiteAt(column, line);
        }

        /**
         * Horrible mess of a method. Don't look, I'm embarassed.
         */
        public string GetGraphics()
        {
            string graphics = "\n    A  B  C  D  E  F  G  H\n\n";

            //for each line
            for (int line = 0; line < 8; line++)
            {
                //show line name
                graphics += " " + (line + 1) + " ";

                //for each column
                for (int column = 0; column < 8; column++)
                {
                    string character = " - ";

                    foreach (var pawn in Whites)
                    {
                        if (pawn.Column == column && pawn.Line == line)
                        {
                            character = " W ";
                        }
                    }

                    foreach (var pawn in Blacks)
                    {
                        if (pawn.Column == column && pawn.Line == line)
                        {
                            character = " B ";
                        }
                    }

                    graphics += character;
                }

                graphics += '\n';
            }

            return graphics;
        }
    }
}
