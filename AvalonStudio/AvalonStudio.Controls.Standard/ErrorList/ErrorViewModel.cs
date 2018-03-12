using Avalonia.Media;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using System;
using System.IO;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public class ErrorViewModel : ViewModel<Diagnostic>, IComparable<ErrorViewModel>
    {
        public ErrorViewModel(Diagnostic model, object tag) : base(model)
        {
            offset = model.StartOffset;
            Tag = tag;
        }

        public object Tag { get; private set; }

        public string File
        {
            get { return Path.GetFileName(Model.File); }
        }

        public string Spelling
        {
            get { return Model.Spelling; }
        }

        public string Project
        {
            get { return Model.Project.Name; }
        }

        public int Line
        {
            get { return Model.Line; }
        }

        private int offset { get; }

        public DiagnosticLevel Level
        {
            get { return Model.Level; }
        }

        public IBrush LevelBrush
        {
            get
            {
                switch (Level)
                {
                    case DiagnosticLevel.Error:
                    case DiagnosticLevel.Fatal:
                        return Brush.Parse("#E34937");

                    case DiagnosticLevel.Warning:
                        return Brush.Parse("#D78A04");

                    default:
                        return Brush.Parse("#1C7CD2");
                }
            }
        }

        public bool IsEqual(ErrorViewModel other)
        {
            var result = false;

            if (File == other.File)
            {
                if (offset == other.offset)
                {
                    if (Level == other.Level)
                    {
                        //if (this.rangeCount == other.rangeCount)
                        {
                            if (Spelling == other.Spelling)
                            {
                                result = true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public int CompareTo(ErrorViewModel other)
        {
            return Line.CompareTo(other.Line);
        }
    }
}