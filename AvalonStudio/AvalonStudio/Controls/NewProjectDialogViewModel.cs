namespace AvalonStudio.Controls.ViewModels
{
    using Extensibility;
    using Languages;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NewProjectDialogViewModel : ModalDialogViewModelBase
    {
        public NewProjectDialogViewModel() : base("New Project", true, true)
        {
            projectTemplates = new ObservableCollection<IProjectTemplate>();
            Languages = new List<ILanguageService>(WorkspaceViewModel.Instance.Model.LanguageServices);            
        }

        private IProjectTemplate selectedTemplate;
        public IProjectTemplate SelectedTemplate
        {
            get { return selectedTemplate; }
            set { this.RaiseAndSetIfChanged(ref selectedTemplate, value); }
        }

        private void GetTemplates (ILanguageService languageService)
        {
            var templates = Workspace.Instance.ProjectTemplates.Where(t => languageService.BaseTemplateType.IsAssignableFrom(t.GetType()));

            ProjectTemplates = new ObservableCollection<IProjectTemplate>(templates);
        }

        private ObservableCollection<IProjectTemplate> projectTemplates;
        public ObservableCollection<IProjectTemplate> ProjectTemplates
        {
            get { return projectTemplates; }
            set { this.RaiseAndSetIfChanged(ref projectTemplates, value); }
        }

        public List<ILanguageService> Languages { get; set; }

        private ILanguageService selectedLanguage;
        public ILanguageService SelectedLanguage
        {
            get { return selectedLanguage; }
            set { this.RaiseAndSetIfChanged(ref selectedLanguage, value); GetTemplates(value); }
        }

    }
}
