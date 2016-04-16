﻿namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Languages;
    using System.Collections.ObjectModel;
    using ReactiveUI;
    using Projects.CPlusPlus;
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

        private ErrorViewModel selectedError;
        public ErrorViewModel SelectedError
        {
            get { return selectedError; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedError, value);

                if (value != null)
                {
                    var document = ShellViewModel.Instance.OpenDocument(ShellViewModel.Instance.SolutionExplorer.Model.FindFile(SourceFile.FromPath(null, null, value.Model.File)), value.Line);

                    if (document != null)
                    {
                        document.GotoOffset(value.Model.Offset);
                    }
                }
            }
        }

    }
}
