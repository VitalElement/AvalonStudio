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
        private ObservableCollection<ErrorViewModel> errors;
        private Dictionary<string, List<Diagnostic>> _errorsLinkedToFiles;
        private ObservableCollection<ErrorViewModel> _fixits;
        private Dictionary<string, TextMarkerService> _markerServices;
        private TextSegmentCollection<Diagnostic> _textSegmentCollection;

        private ErrorViewModel selectedError;
        private IShell shell;

        public ErrorListViewModel()
        {
            Title = "Error List";
            errors = new ObservableCollection<ErrorViewModel>();
            _fixits = new ObservableCollection<ErrorViewModel>();
            _markerServices = new Dictionary<string, TextMarkerService>();
            _errorsLinkedToFiles = new Dictionary<string, List<Diagnostic>>();
            _textSegmentCollection = new TextSegmentCollection<Diagnostic>();
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
            _textSegmentCollection.Add(diagnostic);

            if (!_errorsLinkedToFiles.ContainsKey(diagnostic.File.Location))
            {
                _errorsLinkedToFiles.Add(diagnostic.File.Location, new List<Diagnostic>());
            }

            _errorsLinkedToFiles[diagnostic.File.Location].Add(diagnostic);

            if (_markerServices.ContainsKey(diagnostic.File.Location))
            {
                var markerService = _markerServices[diagnostic.File.Location];

                markerService.Create(diagnostic);
            }

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
            _textSegmentCollection.Remove(diagnostic);

            if (_errorsLinkedToFiles.ContainsKey(diagnostic.File.Location))
            {
                _errorsLinkedToFiles[diagnostic.File.Location].Remove(diagnostic);
            }

            if (_markerServices.ContainsKey(diagnostic.File.Location))
            {
                _markerServices[diagnostic.File.Location].Remove(diagnostic);
            }

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
            _markerServices.Remove(e.File.Location);
        }

        private void Shell_FileOpened(object sender, FileOpenedEventArgs e)
        {
            TextMarkerService currentService;

            if (!_markerServices.ContainsKey(e.File.Location))
            {
                _markerServices.Add(e.File.Location, new TextMarkerService(e.Editor.GetDocument()));

                currentService = _markerServices[e.File.Location];

                if (!_errorsLinkedToFiles.ContainsKey(e.File.Location))
                {
                    _errorsLinkedToFiles.Add(e.File.Location, new List<Diagnostic>());
                }
            }
            else
            {
                currentService = _markerServices[e.File.Location];
            }

            var errorLinks = _errorsLinkedToFiles[e.File.Location];

            foreach (var error in errorLinks)
            {
                currentService.Create(error);
            }

            e.Editor.InstallBackgroundRenderer(currentService);
        }

        public ReadOnlyCollection<Diagnostic> FindDiagnosticsAtOffset(int offset)
        {
            return _textSegmentCollection.FindSegmentsContaining(offset);
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