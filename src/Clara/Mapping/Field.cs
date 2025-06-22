using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class Field
    {
        internal Field()
        {
        }

        public virtual bool IsSearchable
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsFilterable
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsFacetable
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsSortable
        {
            get
            {
                return false;
            }
        }

        internal abstract FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderBuilder tokenEncoderBuilder);
    }
}
