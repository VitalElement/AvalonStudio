using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public class ErrorListViewModel : ToolViewModel, IExtension, IErrorList
    {
        private ObservableCollection<ErrorViewModel> errors;

        private ErrorViewModel selectedError;
        private IShell shell;

        public ErrorListViewModel() : base("Error List")
        {
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
                        var currentDocument = shell.CurrentSolution.FindFile(value.Model.File);

                        if (currentDocument != null)
                        {
                            var document = await shell.OpenDocumentAsync(currentDocument, value.Line);

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

        public void BeforeActivation()
        {
            IoC.RegisterConstant(this, typeof(IErrorList));
        }

        public void Activation()
        {
            shell = IoC.Get<IShell>();
        }
    }
}