namespace AvalonStudio.Controls
{
    using AvalonStudio.Languages;
    using AvalonStudio.MVVM;
    using Perspex.Media;
    using System.IO;
    public class ErrorViewModel : ViewModel<Diagnostic>
    {
        public ErrorViewModel (Diagnostic model) : base (model)
        {
            offset = model.Offset;
        }

        public string File { get { return Path.GetFileName(Model.File); } }

        public string Spelling { get { return Model.Spelling; } }

        public string Project { get { return Model.Project.Name; } }

        public int Line { get { return Model.Line; } }

        private int offset { get; set; }

        public DiagnosticLevel Level { get { return Model.Level; } }

        public IBrush LevelBrush
        {
            get
            {
                switch(Level)
                {
                    case DiagnosticLevel.Error:
                    case DiagnosticLevel.Fatal:
                        return Brush.Parse("#E51400");

                    case DiagnosticLevel.Warning:
                        return Brush.Parse("#FFCC00");

                    default:
                        return Brush.Parse("#1BA1E2");
                }
            }
        }

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
