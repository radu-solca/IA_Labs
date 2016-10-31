using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnChess
{
    interface Player
    {
        bool White { get; set; }
        Move GetMove(Board board);
    }
}
