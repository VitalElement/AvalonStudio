namespace AvalonStudio.Controls.ViewModels
{
    using Extensibility;
    using Languages;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NewProjectDialogViewModel : ModalDialogViewModelBase
    {
        public NewProjectDialogViewModel() : base("New Project", true, true)
        {
            Languages = new List<ILanguageService>(WorkspaceViewModel.Instance.Model.LanguageServices);            
        }

        public List<ILanguageService> Languages { get; set; }

        private ILanguageService selectedLanguage;
        public ILanguageService SelectedLanguage
        {
            get { return selectedLanguage; }
            set { this.RaiseAndSetIfChanged(ref selectedLanguage, value); }
        }

    }
}
