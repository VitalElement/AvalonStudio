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

                if (value != null)
                {
                    Task.Run(async () =>
                    {
                        var document = await shell.OpenDocument(shell.CurrentSolution.FindFile(value.Model.File), value.Line);

                        if (document != null)
                        {
                            document.GotoOffset(value.Model.StartOffset);
                        }
                    });
                }
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