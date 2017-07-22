using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public class ErrorListViewModel : ToolViewModel, IExtension, IErrorList
    {
        class FileAssociation
        {
            public FileAssociation()
            {
                Diagnostics = new List<Diagnostic>();
                FixIts = new TextSegmentCollection<FixIt>();
                Replacements = new TextSegmentCollection<Replacement>();
            }

            public List<Diagnostic> Diagnostics { get; set; }
            public TextSegmentCollection<FixIt> FixIts { get; set; }
            public TextSegmentCollection<Replacement> Replacements { get; set; }
            public TextMarkerService TextMarkerService { get; set; }
        }

        private ObservableCollection<ErrorViewModel> errors;

        private Dictionary<string, FileAssociation> _fileAssociations;

        private ErrorViewModel selectedError;
        private IShell shell;

        public ErrorListViewModel()
        {
            Title = "Error List";
            errors = new ObservableCollection<ErrorViewModel>();
            _fileAssociations = new Dictionary<string, FileAssociation>();
        }

        public ErrorViewModel SelectedError
        {
            get
            {
                return selectedError;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedError, value);

                if (value != null)
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        var document = await shell.OpenDocument(value.Model.File, value.Line);

                        if (document != null)
                        {
                            if (value.Model.Line == -1 || value.Model.Column == -1)
                            {
                                document.GotoOffset(value.Model.StartOffset);
                            }
                            else
                            {
                                document.GotoPosition(value.Model.Line, value.Model.Column);
                            }
                        }
                    });
                }
            }
        }

        public override Location DefaultLocation
        {
            get { return Location.Bottom; }
        }

        public void AddDiagnostic(ErrorViewModel error)
        {
            Errors.Add(error);

            AddDiagnostic(error.Model);
        }

        private void AddDiagnostic(Diagnostic diagnostic)
        {
            lock (_fileAssociations)
            {
                if (!_fileAssociations.ContainsKey(diagnostic.File.Location))
                {
                    _fileAssociations.Add(diagnostic.File.Location, new FileAssociation());
                }
            }

            var fileAssociation = _fileAssociations[diagnostic.File.Location];

            fileAssociation.Diagnostics.Add(diagnostic);
            fileAssociation.TextMarkerService?.Create(diagnostic);

            foreach (var child in diagnostic.Children)
            {
                AddDiagnostic(child);
            }
        }

        public void RemoveDiagnostic(ErrorViewModel error)
        {
            Errors.Remove(error);

            RemoveDiagnostic(error.Model);
        }

        private void RemoveDiagnostic(Diagnostic diagnostic)
        {
            var fileAssociation = _fileAssociations[diagnostic.File.Location];
            fileAssociation?.TextMarkerService?.Remove(diagnostic);
            fileAssociation?.Diagnostics.Remove(diagnostic);

            foreach (var child in diagnostic.Children)
            {
                RemoveDiagnostic(child);
            }
        }

        void IErrorList.AddFixIt(FixIt fixit)
        {
            lock (_fileAssociations)
            {
                if (!_fileAssociations.ContainsKey(fixit.File.Location))
                {
                    _fileAssociations.Add(fixit.File.Location, new FileAssociation());
                }
            }

            var fileAssociation = _fileAssociations[fixit.File.Location];

            fileAssociation.FixIts.Add(fixit);

            foreach(var replacement in fixit.Replacements)
            {
                fileAssociation.Replacements.Add(replacement);
            }
        }

        void IErrorList.RemoveFixIt(FixIt fixit)
        {
            var fileAssociation = _fileAssociations[fixit.File.Location];

            fileAssociation.FixIts.Remove(fixit);

            foreach(var replacement in fixit.Replacements)
            {
                fileAssociation.Replacements.Remove(replacement);
            }

            var diagnostics = fileAssociation.TextMarkerService.FindDiagnosticsAtOffset(fixit.StartOffset, true);

            foreach (var diagnostic in diagnostics)
            {
                RemoveDiagnostic(Errors.FirstOrDefault(e => e.Model == diagnostic));
            }
        }

        void IErrorList.ClearFixits(Predicate<Diagnostic> predicate)
        {
            // FixIts.RemoveMatching(vm => predicate(vm));
        }

        public TextSegmentCollection<FixIt> GetFixits(ISourceFile file)
        {
            var fileAssociation = _fileAssociations[file.Location];

            return fileAssociation.FixIts;
        }

        public ObservableCollection<ErrorViewModel> Errors
        {
            get { return errors; }
            set { this.RaiseAndSetIfChanged(ref errors, value); }
        }

        IReadOnlyCollection<ErrorViewModel> IErrorList.Errors => Errors;

        public void BeforeActivation()
        {
            IoC.RegisterConstant(this, typeof(IErrorList));
        }

        public void Activation()
        {
            shell = IoC.Get<IShell>();

            shell.FileOpened += Shell_FileOpened;
            shell.FileClosed += Shell_FileClosed;
        }

        private void Shell_FileClosed(object sender, FileOpenedEventArgs e)
        {
            var fileAssociation = _fileAssociations[e.File.Location];

            fileAssociation.TextMarkerService = null;
        }

        private void Shell_FileOpened(object sender, FileOpenedEventArgs e)
        {
            lock (_fileAssociations)
            {
                if (!_fileAssociations.ContainsKey(e.File.Location))
                {
                    _fileAssociations.Add(e.File.Location, new FileAssociation());
                }
            }

            var fileAssociation = _fileAssociations[e.File.Location];

            if (fileAssociation.TextMarkerService == null)
            {
                fileAssociation.TextMarkerService = new TextMarkerService(e.Editor.GetDocument());
            }

            var currentService = fileAssociation.TextMarkerService;

            foreach (var error in fileAssociation.Diagnostics)
            {
                currentService.Create(error);
            }

            e.Editor.InstallBackgroundRenderer(currentService);

            e.Editor.InstallMargin(new QuickActionsMargin(e.Editor, this, e.File));

            var document = e.Editor.GetDocument();

            TextDocumentWeakEventManager.Changed.AddHandler(document, OnDocumentChanged);
        }

        private void OnDocumentChanged(object sender, DocumentChangeEventArgs e)
        {
            var fileAssociation = _fileAssociations[(sender as TextDocument).FileName];

            fileAssociation.FixIts.UpdateOffsets(e);
            fileAssociation.Replacements.UpdateOffsets(e);
        }

        public IEnumerable<Diagnostic> FindDiagnosticsAtOffset(ISourceFile file, int offset)
        {
            return _fileAssociations[file.Location]?.TextMarkerService?.FindDiagnosticsAtOffset(offset);
        }
    }
}