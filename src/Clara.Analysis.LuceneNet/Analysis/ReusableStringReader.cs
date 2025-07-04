﻿namespace Clara.Analysis
{
    public sealed class ReusableStringReader : TextReader
    {
        private string text;
        private int position;

        public ReusableStringReader()
        {
            this.text = string.Empty;
            this.position = 0;
        }

        public void SetText(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.text = text;
            this.position = 0;
        }

        public override void Close()
        {
            this.Dispose(disposing: true);
        }

        public override int Peek()
        {
            if (this.position == this.text.Length)
            {
                return -1;
            }

            return this.text[this.position];
        }

        public override int Read()
        {
            if (this.position == this.text.Length)
            {
                return -1;
            }

            return this.text[this.position++];
        }

        public override int Read(char[] buffer, int index, int count)
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (buffer.Length - index < count)
            {
                throw new ArgumentException("Read character count would exceed provided buffer length.");
            }

            var num = this.text.Length - this.position;

            if (num > 0)
            {
                if (num > count)
                {
                    num = count;
                }

                this.text.CopyTo(this.position, buffer, index, num);
                this.position += num;
            }

            return num;
        }

        public override string ReadToEnd()
        {
            var result = this.text;

            if (this.position != 0)
            {
                result = this.text.Substring(this.position);
            }

            this.position = this.text.Length;

            return result;
        }

        public override string? ReadLine()
        {
            int i;

            for (i = this.position; i < this.text.Length; i++)
            {
                var c = this.text[i];

                if (c == '\r' || c == '\n')
                {
                    var result = this.text.Substring(this.position, i - this.position);

                    this.position = i + 1;

                    if (c == '\r' && this.position < this.text.Length && this.text[this.position] == '\n')
                    {
                        this.position++;
                    }

                    return result;
                }
            }

            if (i > this.position)
            {
                var result = this.text.Substring(this.position, i - this.position);

                this.position = i;

                return result;
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            this.text = string.Empty;
            this.position = 0;

            base.Dispose(disposing);
        }
    }
}
