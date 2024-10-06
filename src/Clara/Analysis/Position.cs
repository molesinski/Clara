namespace Clara.Analysis
{
    public readonly struct Position : IEquatable<Position>, IComparable<Position>
    {
        private readonly int start;
        private readonly int end;

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

            this.start = start;
            this.end = end;
        }

        public int Start
        {
            get
            {
                return this.start;
            }
        }

        public int End
        {
            get
            {
                return this.end;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

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

        public override bool Equals(object? obj)
        {
            return obj is Position other && this == other;
        }

        public bool Equals(Position other)
        {
            return this.start == other.start
                && this.end == other.end;
        }

        public override int GetHashCode()
        {
            var hash = default(HashCode);

            hash.Add(this.start);
            hash.Add(this.end);

            return hash.ToHashCode();
        }
    }
}
