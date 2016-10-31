namespace PawnChess
{
    class Pawn
    {
        public int moves = 0;
        public Pawn(int column, int line)
        {
            Column = column;
            Line = line;
        }
        public int Column { get; set; }
        public int Line { get; set; }

        public int LastColumn { get; set; }
        public int LastLine { get; set; }

        public void move(int toColumn, int ToLine)
        {
            moves++;

            LastColumn = Column;
            Column = toColumn;

            LastLine = Line;
            Line = ToLine;
        }
    }
}
