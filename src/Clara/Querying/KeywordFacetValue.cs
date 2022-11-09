namespace Clara.Querying
{
    public readonly record struct KeywordFacetValue
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
