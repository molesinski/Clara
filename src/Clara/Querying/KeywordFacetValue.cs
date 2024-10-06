namespace Clara.Querying
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct KeywordFacetValue
    {
        public KeywordFacetValue(string value, int count, bool isSelected)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Value = value;
            this.Count = count;
            this.IsSelected = isSelected;
        }

        public string Value { get; }

        public int Count { get; }

        public bool IsSelected { get; }
    }
}
