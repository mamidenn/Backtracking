using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backtracking
{
    public struct Position
    {
        public readonly int Row;
        public readonly int Column;
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public static Position operator +(Position a, Position b)
        {
            return new Position(a.Row + b.Row, a.Column + b.Column);
        }
    }
}
