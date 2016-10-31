using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnChess
{
    class HumanPlayer : Player
    {
        public bool White { get; set; }
        public Move GetMove(Board board)
        {
            Console.Write("\n\nIt's your turn. Make your move!\n");
            Console.Write("\nPawn at: ");
            string pawnAt = Console.ReadLine();

            Console.Write("\nTo: ");
            string to = Console.ReadLine();

            return new Move(pawnAt, to);
        }
    }
}
