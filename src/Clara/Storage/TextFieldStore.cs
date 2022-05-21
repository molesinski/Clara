using System;
using Clara.Analysis;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class TextFieldStore : FieldStore
    {
        private readonly ISynonymMap synonymMap;
        private readonly TokenEncoder tokenEncoder;
        private readonly TokenDocumentStore tokenDocumentStore;

        public TextFieldStore(
            ISynonymMap synonymMap,
            TokenEncoder tokenEncoder,
            TokenDocumentStore tokenDocumentStore)
        {
            if (synonymMap is null)
            {
                throw new ArgumentNullException(nameof(synonymMap));
            }

            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (tokenDocumentStore is null)
            {
                throw new ArgumentNullException(nameof(tokenDocumentStore));
            }

            this.synonymMap = synonymMap;
            this.tokenEncoder = tokenEncoder;
            this.tokenDocumentStore = tokenDocumentStore;
        }

        public override double FilterOrder
        {
            get
            {
                return double.MinValue;
            }
        }

        public override void Filter(FilterExpression filterExpression, DocumentSet documentSet)
        {
            if (filterExpression is TextFilterExpression textFilterExpression)
            {
                var matchExpression = textFilterExpression.MatchExpression;

                matchExpression = this.synonymMap.Filter(matchExpression);

                this.tokenDocumentStore.Filter(textFilterExpression.Field, matchExpression, documentSet);
                return;
            }

            base.Filter(filterExpression, documentSet);
        }

        public override void Dispose()
        {
            this.tokenEncoder.Dispose();
            this.tokenDocumentStore.Dispose();

            base.Dispose();
        }
    }
}
