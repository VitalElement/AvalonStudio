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

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public class ErrorListViewModel : ToolViewModel, IExtension, IErrorList
    {
        class FileAssociation
        {
            public FileAssociation()
            {
                Diagnostics = new TextSegmentCollection<Diagnostic>();
            }

            public TextSegmentCollection<Diagnostic> Diagnostics { get; set; }
            public TextMarkerService TextMarkerService { get; set; }
        }

        private ObservableCollection<ErrorViewModel> errors;
        private ObservableCollection<ErrorViewModel> _fixits;

        private Dictionary<string, FileAssociation> _fileAssociations;
        

        private ErrorViewModel selectedError;
        private IShell shell;

        public ErrorListViewModel()
        {
            Title = "Error List";
            errors = new ObservableCollection<ErrorViewModel>();
            _fixits = new ObservableCollection<ErrorViewModel>();
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
            if(!_fileAssociations.ContainsKey(diagnostic.File.Location))
            {
                _fileAssociations.Add(diagnostic.File.Location, new FileAssociation());
            }

            var fileAssociation = _fileAssociations[diagnostic.File.Location];

            fileAssociation.Diagnostics.Add(diagnostic);
            fileAssociation.TextMarkerService?.Create(diagnostic);

            foreach(var child in diagnostic.Children)
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

        public ObservableCollection<ErrorViewModel> Errors
        {
            get { return errors; }
            set { this.RaiseAndSetIfChanged(ref errors, value); }
        }

        public ObservableCollection<ErrorViewModel> FixIts
        {
            get { return _fixits; }
            set { this.RaiseAndSetIfChanged(ref _fixits, value); }
        }

        IReadOnlyCollection<ErrorViewModel> IErrorList.Errors => Errors;

        IReadOnlyCollection<ErrorViewModel> IErrorList.FixIts => FixIts;

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
            if (!_fileAssociations.ContainsKey(e.File.Location))
            {
                _fileAssociations.Add(e.File.Location, new FileAssociation());
            }

            var fileAssociation = _fileAssociations[e.File.Location];

            if(fileAssociation.TextMarkerService == null)
            {
                fileAssociation.TextMarkerService = new TextMarkerService(e.Editor.GetDocument());
            }

            var currentService = fileAssociation.TextMarkerService;
            
            foreach (var error in fileAssociation.Diagnostics)
            {
                currentService.Create(error);
            }

            e.Editor.InstallBackgroundRenderer(currentService);
        }

        public ReadOnlyCollection<Diagnostic> FindDiagnosticsAtOffset(ISourceFile file, int offset)
        {
            var fileAssociation = _fileAssociations[file.Location];

            return fileAssociation.Diagnostics.FindSegmentsContaining(offset);
        }

        void IErrorList.AddFixIt(FixIt fixit)
        {
            FixIts.Add(new ErrorViewModel(fixit));
        }

        void IErrorList.ClearFixits(Predicate<Diagnostic> predicate)
        {
            FixIts.RemoveMatching(vm => predicate(vm.Model));
        }
    }
}