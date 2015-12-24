namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Languages;
    using System.Collections.ObjectModel;
    using ReactiveUI;

    public class ErrorListViewModel : ViewModel
    {
        public ErrorListViewModel()
        {
            errors = new ObservableCollection<ErrorViewModel>();
        }

        private ObservableCollection<ErrorViewModel> errors;
        public ObservableCollection<ErrorViewModel> Errors
        {
            get { return errors; }
            set { this.RaiseAndSetIfChanged(ref errors, value); }
        }
    }
}
