using Avalonia.Media;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using System;
using System.IO;

namespace AvalonStudio.Utils
{
    public class ErrorViewModel : ViewModel<Diagnostic>, IComparable<ErrorViewModel>
    {
        public ErrorViewModel(Diagnostic model, object tag) : base(model)
        {
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
            get { return Model.Project; }
        }

        public int Line
        {
            get { return Model.Line; }
        }

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
                        return ColorTheme.CurrentTheme.ErrorListError;

                    case DiagnosticLevel.Warning:
                        return ColorTheme.CurrentTheme.ErrorListWarning;

                    default:
                        return ColorTheme.CurrentTheme.ErrorListInfo;
                }
            }
        }

        public int CompareTo(ErrorViewModel other)
        {
            return Line.CompareTo(other.Line);
        }
    }
}