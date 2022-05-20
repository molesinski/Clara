﻿using System;

namespace Clara.Querying
{
    public readonly struct KeywordValue
    {
        public KeywordValue(string value, int count, bool isSelected)
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
