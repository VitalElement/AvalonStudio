namespace AvalonStudio.Controls.Standard.ErrorList
{
    using AvalonStudio.MVVM;
    using Languages;
    using System.Collections.ObjectModel;
    using ReactiveUI;
    using System;
    using Extensibility;
    using Extensibility.Plugin;
    using Extensibility.Utils;
    using Shell;

    public class ErrorListViewModel : ToolViewModel, IExtension, IErrorList
    {
        private IShell shell;

        public ErrorListViewModel()
        {
            Title = "Error List";
            errors = new ObservableCollection<ErrorViewModel>();
        }

        private ObservableCollection<ErrorViewModel> errors;
        public ObservableCollection<ErrorViewModel> Errors
        {
            get { return errors; }
            set { this.RaiseAndSetIfChanged(ref errors, value); }
        }

        private ErrorViewModel selectedError;
        public ErrorViewModel SelectedError
        {
            get { return selectedError; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedError, value);

                if (value != null)
                {
                    var document = shell.OpenDocument(shell.CurrentSolution.FindFile(PathSourceFile.FromPath(null, null, value.Model.File)), value.Line);

                    document.Wait();

                    if (document != null)
                    {
                        document.Result.GotoOffset(value.Model.Offset);
                    }
                }
            }
        }

        public override Location DefaultLocation
        {
            get
            {
                return Location.Bottom;
            }
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
