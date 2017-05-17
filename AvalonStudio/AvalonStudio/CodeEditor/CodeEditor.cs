using Avalonia;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using System.Collections.ObjectModel;
using System;
using System.Collections.Specialized;
using System.Reactive.Subjects;
using AvalonStudio.Extensibility.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Languages;
using System.Collections.Generic;
using AvalonStudio.Projects;
using AvaloniaEdit.Document;
using Avalonia.Threading;
using System.Linq;
using AvalonStudio.Utils;
using AvalonStudio.Shell;

namespace AvalonStudio.CodeEditor
{
    public class CodeEditor : AvaloniaEdit.TextEditor
    {
        private static List<UnsavedFile> _unsavedFiles;

        public static List<UnsavedFile> UnsavedFiles
        {
            get
            {
                if (_unsavedFiles == null)
                {
                    _unsavedFiles = new List<UnsavedFile>();
                }

                return _unsavedFiles;
            }
        }

        public ISourceFile ProjectFile { get; private set; }

        public bool IsDirty { get; set; }

        private readonly IShell _shell;
        private Subject<bool> _analysisTriggerEvents = new Subject<bool>();
        private readonly JobRunner _codeAnalysisRunner;
        private CancellationTokenSource _cancellationSource;
        private CodeAnalysisResults _codeAnalysisResults;

        public event EventHandler<EventArgs> CodeAnalysisCompleted;

        /// <summary>
        ///     Write lock must be held before calling this.
        /// </summary>
        public void TriggerCodeAnalysis()
        {
            _analysisTriggerEvents.OnNext(true);
        }

        public CodeAnalysisResults CodeAnalysisResults
        {
            get
            {
                return _codeAnalysisResults;
            }
            set
            {
                _codeAnalysisResults = value;

                CodeAnalysisCompleted?.Invoke(this, new EventArgs());
            }
        }

        public ILanguageService LanguageService { get; set; }

        public CodeEditor()
        {
            _codeAnalysisRunner = new JobRunner();

            _shell = IoC.Get<IShell>();

            var breakpointMargin = new BreakPointMargin(this, IoC.Get<IDebugManager2>().Breakpoints);

            TextArea.LeftMargins.Add(breakpointMargin);

            var lineNumberMargin = new LineNumberMargin(this) { Margin = new Thickness(0, 0, 10, 0) };

            TextArea.LeftMargins.Add(lineNumberMargin);

            DocumentLineTransformersProperty.Changed.Subscribe(s =>
            {
                if(s.OldValue != null)
                {
                    var list = s.OldValue as ObservableCollection<IVisualLineTransformer>;

                    list.CollectionChanged -= List_CollectionChanged;
                }

                if(s.NewValue != null)
                {
                    var list = s.NewValue as ObservableCollection<IVisualLineTransformer>;

                    list.CollectionChanged += List_CollectionChanged;
                }
            });

            _analysisTriggerEvents.Select(_ => Observable.Timer(TimeSpan.FromMilliseconds(500))
            .SelectMany(o => DoCodeAnalysisAsync())).Switch().Subscribe(_ => { });
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);


        }

        private void StartBackgroundWorkers()
        {
            _cancellationSource = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                _codeAnalysisRunner.RunLoop(_cancellationSource.Token);
                _cancellationSource = null;
            });
        }

        public void ShutdownBackgroundWorkers()
        {
            _cancellationSource?.Cancel();
        }

        public void OpenFile(ISourceFile file, IIntellisenseControl intellisense,
            ICompletionAssistant completionAssistant)
        {
            if (ProjectFile != file)
            {
                if (System.IO.File.Exists(file.Location))
                {
                    using (var fs = System.IO.File.OpenText(file.Location))
                    {
                        Document = new TextDocument(fs.ReadToEnd());
                        Document.FileName = file.Location;
                    }

                    ProjectFile = file;

                    RegisterLanguageService(intellisense, completionAssistant);

                   // DocumentLoaded?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Save()
        {
           /* if (ProjectFile != null && Document != null && IsDirty)
            {
                System.IO.File.WriteAllText(ProjectFile.Location, TextDocument.Text);
                IsDirty = false;

                lock (UnsavedFiles)
                {
                    var unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);

                    if (unsavedFile != null)
                    {
                        UnsavedFiles.Remove(unsavedFile);
                    }
                }
            }*/
        }

        public void OnTextChanged(object param)
        {
            IsDirty = true;

            TriggerCodeAnalysis();
        }


        public async Task<CodeCompletionResults> DoCompletionRequestAsync(int index, int line, int column)
        {
            CodeCompletionResults results = null;

            var completions = await LanguageService.CodeCompleteAtAsync(ProjectFile, index, line, column, UnsavedFiles.ToList());
            results = completions;

            return results;
        }

        private async Task<bool> DoCodeAnalysisAsync()
        {
            await _codeAnalysisRunner.InvokeAsync(async () =>
            {
                if (LanguageService != null)
                {
                    // TODO allow interruption.
                    var result = await LanguageService.RunCodeAnalysisAsync(ProjectFile, UnsavedFiles.ToList(), () => false);

                    Dispatcher.UIThread.InvokeAsync(() => { TextArea.TextView.Redraw(); });
                }
            });

            return true;
        }

        public void RegisterLanguageService(IIntellisenseControl intellisenseControl, ICompletionAssistant completionAssistant)
        {
            UnRegisterLanguageService();

            LanguageService = _shell.LanguageServices.FirstOrDefault(o => o.CanHandle(ProjectFile));

            if (LanguageService != null)
            {
                ShellViewModel.Instance.StatusBar.Language = LanguageService.Title;

                LanguageService.RegisterSourceFile(intellisenseControl, completionAssistant, this, ProjectFile, Document);
            }
            else
            {
                LanguageService = null;
                ShellViewModel.Instance.StatusBar.Language = "Text";
            }

            //IsDirty = false;

            StartBackgroundWorkers();

            Document.TextChanged += TextDocument_TextChanged;

            //OnBeforeTextChanged(null);

            DoCodeAnalysisAsync().GetAwaiter();
        }

        public void UnRegisterLanguageService()
        {
            ShutdownBackgroundWorkers();

            UnsavedFile unsavedFile = null;

            lock (UnsavedFiles)
            {
                unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);
            }

            if (unsavedFile != null)
            {
                lock (UnsavedFiles)
                {
                    UnsavedFiles.Remove(unsavedFile);
                }
            }

            if (LanguageService != null && ProjectFile != null)
            {
                LanguageService.UnregisterSourceFile(this, ProjectFile);
            }

            Document.TextChanged -= TextDocument_TextChanged;
        }

        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            UnsavedFile unsavedFile = null;

            lock (UnsavedFiles)
            {
                unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);
            }

            if (unsavedFile == null)
            {
                lock (UnsavedFiles)
                {
                    UnsavedFiles.InsertSorted(new UnsavedFile(ProjectFile.Location, Document.Text));
                }
            }
            else
            {
                unsavedFile.Contents = Document.Text;
            }

            //IsDirty = true;

           // TextChanged?.Invoke(this, new EventArgs());
        }



        private void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(var transformer in e.NewItems)
                    {
                        TextArea.TextView.LineTransformers.Add(transformer as IVisualLineTransformer);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var transformer in e.OldItems)
                    {
                        TextArea.TextView.LineTransformers.Remove(transformer as IVisualLineTransformer);
                    }
                    break;

            }
        }

        public static readonly StyledProperty<ObservableCollection<IVisualLineTransformer>> DocumentLineTransformersProperty =
            AvaloniaProperty.Register<CodeEditor, ObservableCollection<IVisualLineTransformer>>(nameof(DocumentLineTransformers), new ObservableCollection<IVisualLineTransformer>());

        public ObservableCollection<IVisualLineTransformer> DocumentLineTransformers
        {
            get { return GetValue(DocumentLineTransformersProperty); }
            set { SetValue(DocumentLineTransformersProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<IBackgroundRenderer>> BackgroundRenderersProperty =
            AvaloniaProperty.Register<CodeEditor, ObservableCollection<IBackgroundRenderer>>(nameof(BackgroundRenderers), new ObservableCollection<IBackgroundRenderer>());

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return GetValue(BackgroundRenderersProperty); }
            set { SetValue(BackgroundRenderersProperty, value); }
        }

        public int GetOffsetFromPoint(Point point)
        {
            var position = GetPositionFromPoint(point);

            var offset = position != null ? Document.GetOffset(position.Value.Location) : -1;

            return offset;
        }

    }
}
