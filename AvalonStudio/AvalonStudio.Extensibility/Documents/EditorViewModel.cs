using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.MainMenu;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public abstract class EditorViewModel : DocumentTabViewModel, IFileDocumentTabViewModel
    {
        private bool _isDirty;
        private ISourceFile _sourceFile;
        private ColorScheme _colorScheme;

        public EditorViewModel(ISourceFile file)
        {
            _sourceFile = file;

            var settings = GlobalSettings.Settings.GetSettings<EditorSettings>();

            _colorScheme = ColorScheme.LoadColorScheme(settings.ColorScheme);

            GotoDefinitionCommand = ReactiveCommand.Create(async () =>
            {
                var definition = await Editor.LanguageService?.GotoDefinition(Editor, 1);

                var shell = IoC.Get<IShell>();

                var document = shell.CurrentSolution.FindFile(definition.FileName);

                if(document != null)
                {
                    await shell.OpenDocumentAsync(document, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
                }
            });
        }

        ~EditorViewModel()
        {
            Console.WriteLine("Dispose VM");
        }

        public IMenu ContextMenu => IoC.Get<IShell>().BuildEditorContextMenu();

        public ReactiveCommand GotoDefinitionCommand { get; }

        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                this.RaiseAndSetIfChanged(ref _isDirty, value);

                if (value && IsTemporary)
                {
                    IsTemporary = false;
                }
            }
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

        public abstract IEditor Editor { get; }

        public abstract Task WaitForEditorToLoadAsync();
    }
}
