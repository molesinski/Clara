using System;
using Clara.Analysis.Synonyms;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class TextFieldStore : FieldStore
    {
        private readonly ISynonymMap synonymMap;
        private readonly ITokenEncoder tokenEncoder;
        private readonly TokenDocumentStore tokenDocumentStore;
        private bool isDisposed;

        public TextFieldStore(
            ISynonymMap synonymMap,
            ITokenEncoder tokenEncoder,
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
                if (this.isDisposed)
                {
                    throw new InvalidOperationException("Current instance is already disposed.");
                }

                return double.MinValue;
            }
        }

        public override void Filter(FilterExpression filterExpression, DocumentSet documentSet)
        {
            if (this.isDisposed)
            {
                throw new InvalidOperationException("Current instance is already disposed.");
            }

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
            if (!this.isDisposed)
            {
                this.tokenEncoder.Dispose();
                this.tokenDocumentStore.Dispose();

                this.isDisposed = true;
            }
        }
    }
}
