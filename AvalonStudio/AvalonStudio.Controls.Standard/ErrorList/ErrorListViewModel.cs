using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using ReactiveUI;
using ReactiveUI.Legacy;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    [Export(typeof(IErrorList))]
    [Export(typeof(IExtension))]
    [ExportToolControl]
    [Shared]
    public class ErrorListViewModel : ToolViewModel, IActivatableExtension, IErrorList
    {
        private ObservableCollection<ErrorViewModel> errors;

        private ErrorViewModel selectedError;
        private IStudio studio;
        private bool _showErrors = true;
        private bool _showWarnings = true;
        private bool _fromBuild = true;
        private bool _fromIntellisense = true;
        private bool _showNotes = true;

        /// <inheritdoc/>
        public event EventHandler<DiagnosticsUpdatedEventArgs> DiagnosticsUpdated;

        public ErrorListViewModel() : base("Error List")
        {
            errors = new ObservableCollection<ErrorViewModel>();

            FilteredErrors = errors.CreateDerivedCollection(x=>x, (error) =>
            {
                if(error.Level == DiagnosticLevel.Error && !ShowErrors)
                {
                    return false;
                }

                if(error.Level == DiagnosticLevel.Warning && !ShowWarnings)
                {
                    return false;
                }

                if(error.Level == DiagnosticLevel.Info && !ShowNotes)
                {
                    return false;
                }

                if(error.Source == DiagnosticSourceKind.Build && !FromBuild)
                {
                    return false;
                }

                if(error.Source == DiagnosticSourceKind.Analysis && !FromIntellisense)
                {
                    return false;
                }

                return true;
            }, null, this.WhenAnyValue(x=>x.ShowErrors, x=>x.ShowWarnings, x=>x.FromBuild, x=>x.FromIntellisense, x=>x.ShowNotes));
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

                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    if (value != null)
                    {
                        var currentDocument = studio.CurrentSolution.FindFile(value.Model.File);

                        if (currentDocument != null)
                        {
                            var document = await studio.OpenDocumentAsync(currentDocument, value.Line);

                            if (document != null)
                            {
                                document.GotoOffset(value.Model.StartOffset);
                            }
                        }
                    }
                });
            }
        }

        public override Location DefaultLocation
        {
            get { return Location.Bottom; }
        }

        public bool ShowErrors
        {
            get { return _showErrors; }
            set { this.RaiseAndSetIfChanged(ref _showErrors, value); }
        }

        public bool ShowWarnings
        {
            get { return _showWarnings; }
            set { this.RaiseAndSetIfChanged(ref _showWarnings, value); }
        }

        public bool ShowNotes
        {
            get { return _showNotes; }
            set { this.RaiseAndSetIfChanged(ref _showNotes, value); }
        }


        public bool FromBuild
        {
            get { return _fromBuild; }
            set { this.RaiseAndSetIfChanged(ref _fromBuild, value); }
        }

        public bool FromIntellisense
        {
            get { return _fromIntellisense; }
            set { this.RaiseAndSetIfChanged(ref _fromIntellisense, value); }
        }

        public IReactiveDerivedList<ErrorViewModel> FilteredErrors { get; }

        /// <inheritdoc/>
        public ObservableCollection<ErrorViewModel> Errors
        {
            get { return errors; }
            set { this.RaiseAndSetIfChanged(ref errors, value); }
        }

        /// <inheritdoc/>
        public void Remove(object tag)
        {
            Dispatcher.UIThread.Post(() =>
            {
                var toRemove = Errors.Where(e => Equals(e.Tag, tag)).ToList();

                foreach (var error in toRemove)
                {
                    Errors.Remove(error);
                }

                DiagnosticsUpdated?.Invoke(this, new DiagnosticsUpdatedEventArgs(tag, DiagnosticsUpdatedKind.DiagnosticsRemoved));
            });
        }

        /// <inheritdoc/>
        public void Create(object tag, string filePath, DiagnosticSourceKind source, ImmutableArray<Diagnostic> diagnostics, SyntaxHighlightDataList diagnosticHighlights = null)
        {
            Dispatcher.UIThread.Post(() =>
            {
                foreach (var diagnostic in diagnostics)
                {
                    if (diagnostic.Level != DiagnosticLevel.Hidden)
                    {
                        Errors.InsertSorted(new ErrorViewModel(diagnostic, tag));
                    }
                }

                DiagnosticsUpdated?.Invoke(this, new DiagnosticsUpdatedEventArgs(tag, filePath, DiagnosticsUpdatedKind.DiagnosticsCreated, source, diagnostics, diagnosticHighlights));
            });
        }

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
            studio = IoC.Get<IStudio>();

            IoC.Get<IShell>().CurrentPerspective.AddOrSelectTool(this);
        }
    }
}