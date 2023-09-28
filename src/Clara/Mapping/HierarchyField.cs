﻿using Clara.Analysis.Synonyms;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class HierarchyField : Field
    {
        public const string DefaultRoot = "0";

        internal HierarchyField(bool isFilterable, bool isFacetable, char separator, string root)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: false)
        {
            if (!isFilterable && !isFacetable)
            {
                throw new InvalidOperationException("Either filtering or faceting must be enabled for given field.");
            }

            if (root is null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (string.IsNullOrWhiteSpace(root))
            {
                throw new ArgumentException("Hierarchy root value must not be empty or whitespace.", nameof(root));
            }

            this.Separator = separator;
            this.Root = root;
        }

        public char Separator { get; }

        public string Root { get; }
    }

    public sealed class HierarchyField<TSource> : HierarchyField
    {
        public HierarchyField(Func<TSource, string?> valueMapper, bool isFilterable = false, bool isFacetable = false, char separator = ',', string root = DefaultRoot)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                separator: separator,
                root: root)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source), trim: true);
        }

        public HierarchyField(Func<TSource, IEnumerable<string?>?> valueMapper, bool isFilterable = false, bool isFacetable = false, char separator = ',', string root = DefaultRoot)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                separator: separator,
                root: root)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source), trim: true);
        }

        internal Func<TSource, StringEnumerable> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new HierarchyFieldStoreBuilder<TSource>(this, tokenEncoderStore);
        }
    }
}
