using System;

namespace Clara.Mapping
{
    public abstract class FieldValue
    {
        protected internal FieldValue(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            this.Field = field;
        }

        public Field Field { get; }

        public static FieldValue Create(TextField field, string text)
        {
            return new TextFieldValue(field, text);
        }

        public static FieldValue Create(KeywordField field, string[] keywords)
        {
            return new KeywordFieldValue(field, keywords);
        }

        public static FieldValue Create(HierarchyField field, string[] keywords)
        {
            return new HierarchyFieldValue(field, keywords);
        }

        public static FieldValue Create<TValue>(RangeField<TValue> field, TValue[] values)
            where TValue : struct, IComparable<TValue>
        {
            return new RangeFieldValue<TValue>(field, values);
        }
    }
}
