namespace Clara.Analysis
{
    public readonly record struct Offset : IComparable<Offset>
    {
        public Offset(int start, int end, int position)
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
                throw new ArgumentException("Offset start index must be less than offset end index.", nameof(start));
            }

            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            this.Start = start;
            this.End = end;
            this.Position = position;
        }

        public int Start { get; }

        public int End { get; }

        public int Position { get; }

        public static bool operator <(Offset left, Offset right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Offset left, Offset right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Offset left, Offset right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Offset left, Offset right)
        {
            return left.CompareTo(right) >= 0;
        }

        public int CompareTo(Offset other)
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
