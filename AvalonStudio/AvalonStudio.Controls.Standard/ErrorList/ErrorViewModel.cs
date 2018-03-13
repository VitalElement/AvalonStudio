using Avalonia.Media;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using System;
using System.IO;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public class ErrorViewModel : ViewModel<Diagnostic>, IComparable<ErrorViewModel>
    {
        public ErrorViewModel(Diagnostic model, object tag, ISourceFile associatedFile) : base(model)
        {            
            Tag = tag;
            AssociatedFile = associatedFile;
        }

        public object Tag { get; private set; }

        public ISourceFile AssociatedFile { get; }

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