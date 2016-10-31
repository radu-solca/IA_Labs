using System;
using System.Collections.Generic;

namespace PawnChess
{
    class Move
    {
        private readonly Dictionary<char, int> _columnDictionary = new Dictionary<char, int>()
        {
            { 'A', 0 },
            { 'B', 1 },
            { 'C', 2 },
            { 'D', 3 },
            { 'E', 4 },
            { 'F', 5 },
            { 'G', 6 },
            { 'H', 7 },
        };

        private readonly Dictionary<char, int> _lineDictionary = new Dictionary<char, int>()
        {
            { '1', 0 },
            { '2', 1 },
            { '3', 2 },
            { '4', 3 },
            { '5', 4 },
            { '6', 5 },
            { '7', 6 },
            { '8', 7 },
        };
        public Move(string from, string to) //'A7''B4' => 06, 13
        {
            if (   
                from.Length != 2 
                || to.Length != 2
                || !_columnDictionary.ContainsKey(from[0])
                || !_lineDictionary.ContainsKey(from[1])
                || !_columnDictionary.ContainsKey(to[0])
                || !_lineDictionary.ContainsKey(to[1])
            )
            {
                throw new ArgumentException("Invalid Move");
            }

            FromColumn = _columnDictionary[from[0]];
            FromLine = _lineDictionary[from[1]];

            ToColumn = _columnDictionary[to[0]];
            ToLine = _lineDictionary[to[1]];
        }

        public Move(int fromColumn, int fromLine, int toColumn, int toLine)
        {
            FromColumn = fromColumn;
            FromLine = fromLine;
            ToColumn = toColumn;
            ToLine = toLine;
        }

        public int FromColumn { get; private set; }
        public int FromLine { get; private set; }

        public int ToColumn { get; private set; }
        public int ToLine { get; private set; }
    }
}
