using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using Dock.Model;
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
        private string _zoomLevelText;
        private double _fontSize;
        private double _zoomLevel;
        private double _visualFontSize;        
        private IStudio _studio;

        public EditorViewModel(ISourceFile file)
        {            
            _studio = IoC.Get<IStudio>();
            _visualFontSize = _fontSize = 14;
            _zoomLevel = 1;

            _sourceFile = file;

            var settings = GlobalSettings.Settings.GetSettings<EditorSettings>();

            _colorScheme = ColorScheme.LoadColorScheme(settings.ColorScheme);

            GotoDefinitionCommand = ReactiveCommand.Create(async () =>
            {
                var definition = await Editor.LanguageService?.GotoDefinition(Editor, 1);

                var studio = IoC.Get<IStudio>();

                if (definition.MetaDataFile == null)
                {
                    var document = studio.CurrentSolution.FindFile(definition.FileName);

                    if (document != null)
                    {
                        await studio.OpenDocumentAsync(document, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
                    }
                }
                else
                {
                    await studio.OpenDocumentAsync(definition.MetaDataFile, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
                }
            });

            RenameSymbolCommand = ReactiveCommand.Create(async () =>
            {
                Editor.RenameSymbol(Editor.CaretOffset);
            });

            ZoomLevel = _studio.GlobalZoomLevel;
        }

        ~EditorViewModel()
        {
        }

        public double ZoomLevel
        {
            get
            {
                return _zoomLevel;
            }
            set
            {
                if (value != _zoomLevel)
                {
                    _zoomLevel = value;
                    _studio.GlobalZoomLevel = value;
                    InvalidateVisualFontSize();

                    ZoomLevelText = $"{ZoomLevel:0} %";
                }
            }
        }

        public string ZoomLevelText
        {
            get { return _zoomLevelText; }
            set { this.RaiseAndSetIfChanged(ref _zoomLevelText, value); }
        }

        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    InvalidateVisualFontSize();
                }
            }
        }

        public double VisualFontSize
        {
            get { return _visualFontSize; }
            set { this.RaiseAndSetIfChanged(ref _visualFontSize, value); }
        }

        private void InvalidateVisualFontSize()
        {
            VisualFontSize = (ZoomLevel / 100) * FontSize;
        }

        public ReactiveCommand GotoDefinitionCommand { get; }

        public ReactiveCommand RenameSymbolCommand { get; }

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

        public override bool OnClose()
        {
            bool result = base.OnClose();

            Editor.Dispose();

            return result;
        }

        /// <summary>
        /// Gets or sets view id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets view context.
        /// </summary>
        public object Context { get; set; }

        /// <summary>
        /// Gets or sets view width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets view height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets view parent.
        /// </summary>
        /// <remarks>If parrent is <see cref="null"/> than view is root.</remarks>
        public IView Parent { get; set; }
    }
}
