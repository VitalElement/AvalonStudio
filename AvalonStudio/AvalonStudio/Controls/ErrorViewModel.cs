namespace AvalonStudio.Controls
{
    using AvalonStudio.Languages;
    using AvalonStudio.MVVM;

    public class ErrorViewModel : ViewModel<Diagnostic>
    {
        public ErrorViewModel (Diagnostic model) : base (model)
        {
            offset = model.Offset;
        }

        public string File { get { return Model.File; } }

        public string Spelling { get { return Model.Spelling; } }

        private int offset { get; set; }

        public DiagnosticLevel Level { get { return Model.Level; } }

        public bool IsEqual(ErrorViewModel other)
        {
            bool result = false;

            if (this.File == other.File)
            {
                if (this.offset == other.offset)
                {
                    if (this.Level == other.Level)
                    {
                        //if (this.rangeCount == other.rangeCount)
                        {
                            if (this.Spelling == other.Spelling)
                            {
                                result = true;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
