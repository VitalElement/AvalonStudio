using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using ReactiveUI;
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

        public ErrorListViewModel()
        {
            Title = "Error List";
            errors = new ObservableCollection<ErrorViewModel>();
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

        public ObservableCollection<ErrorViewModel> Errors
        {
            get { return errors; }
            set { this.RaiseAndSetIfChanged(ref errors, value); }
        }

        public void UpdateDiagnostics(DiagnosticsUpdatedEventArgs diagnostics)
        {
            var toRemove = Errors.Where(e => Equals(e.Tag, diagnostics.Tag) && e.AssociatedFile == diagnostics.AssociatedSourceFile).ToList();

            foreach (var error in toRemove)
            {
                Errors.Remove(error);
            }

            foreach (var diagnostic in diagnostics.Diagnostics)
            {
                if (diagnostic.Level != DiagnosticLevel.Hidden)
                {
                    Errors.InsertSorted(new ErrorViewModel(diagnostic, diagnostics.Tag, diagnostics.AssociatedSourceFile));
                }
            }
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