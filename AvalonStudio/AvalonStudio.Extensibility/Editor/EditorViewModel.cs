using AvalonStudio.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Projects;
using ReactiveUI;
using System;

namespace AvalonStudio.Extensibility.Editor
{
    public abstract class EditorViewModel : DocumentTabViewModel
    {
        private ISourceFile _sourceFile;
        private ColorScheme _colorScheme;

        public EditorViewModel(ISourceFile file)
        {
            _sourceFile = file;

            /*var settings = GlobalSettings.Settings.GetSettings<EditorSettings>();

            _colorScheme = ColorScheme.LoadColorScheme(settings.ColorScheme);*/

            Title = file?.Name;
        }

        ~EditorViewModel()
        {
        }

        public ISourceFile SourceFile
        {
            get { return _sourceFile; }
            set { this.RaiseAndSetIfChanged(ref _sourceFile, value); }
        }

        public ColorScheme ColorScheme
        {
            get { return _colorScheme; }
            set { this.RaiseAndSetIfChanged(ref _colorScheme, value); }
        }
    }
}
