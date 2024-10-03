namespace Clara.Analysis
{
    public readonly record struct Position : IComparable<Position>
    {
        public Position(int position)
            : this(position, position)
        {
        }

        public Position(int start, int end)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (end < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }

            if (!(start <= end))
            {
                throw new ArgumentException("Position start index must be less than offset end index.", nameof(start));
            }

            this.Start = start;
            this.End = end;
        }

        public int Start { get; }

        public int End { get; }

        public static bool operator <(Position left, Position right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Position left, Position right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Position left, Position right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Position left, Position right)
        {
            return left.CompareTo(right) >= 0;
        }

        public bool Overlaps(int position)
        {
            return this.Start <= position && position <= this.End;
        }

        public int CompareTo(Position other)
        {
            var result = this.Start.CompareTo(other.Start);

            if (result != 0)
            {
                return result;
            }

            return this.End.CompareTo(other.End);
        }
    }
}
