namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Extensibility;
    using Extensibility.Dialogs;
    using Languages;
    using Avalonia.Controls;
    using Platforms;
    using Projects;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    public class NewProjectDialogViewModel : ModalDialogViewModelBase
    {
        private ISolution solution = null;
        private IShell shell;

        public NewProjectDialogViewModel(ISolution solution) : this()
        {            
            this.solution = solution;
        }

        public NewProjectDialogViewModel() : base("New Project", true, true)
        {
            shell = IoC.Get<IShell>();
            projectTemplates = new ObservableCollection<IProjectTemplate>();
            Languages = new List<ILanguageService>(shell.LanguageServices);

            location = Platform.ProjectDirectory;
            SelectedLanguage = Languages.FirstOrDefault();
            SelectedTemplate = ProjectTemplates.FirstOrDefault();

            BrowseLocationCommand = ReactiveCommand.Create();
            BrowseLocationCommand.Subscribe(async (o) =>
            {
                OpenFolderDialog ofd = new OpenFolderDialog();
                ofd.InitialDirectory = location;

                string result = await ofd.ShowAsync();

                if(result != string.Empty)
                {
                    Location = result;
                }
            });

            OKCommand = ReactiveCommand.Create();
            OKCommand.Subscribe((o) =>
            {               
                if (solution == null)
                {
                    var destination = Path.Combine(location, solutionName);

                    if (!Directory.Exists(destination))
                    {
                        Directory.CreateDirectory(destination);
                    }

                    solution = Solution.Create(destination, solutionName);
                }

                selectedTemplate.Generate(solution, name);

                shell.CurrentSolution = solution;
                solution = null;        

                Close();
            });                
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (solutionName == name)
                {
                    SolutionName = value;
                }

                this.RaiseAndSetIfChanged(ref name, value);                
            }
        }

        
        public bool SolutionControlsVisible
        {
            get
            {
                return solution == null;
            }
        }


        private string location;
        public string Location
        {
            get { return location; }
            set { this.RaiseAndSetIfChanged(ref location, value); }
        }

        private string solutionName;
        public string SolutionName
        {
            get { return solutionName; }
            set { this.RaiseAndSetIfChanged(ref solutionName, value); }
        }

        private IProjectTemplate selectedTemplate;
        public IProjectTemplate SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedTemplate, value);

                if (value != null)
                {
                    Name = value.DefaultProjectName + "1";
                }

                SolutionName = name;
            }
        }

        private void GetTemplates (ILanguageService languageService)
        {
            var templates = shell.ProjectTemplates.Where(t => languageService.BaseTemplateType.IsAssignableFrom(t.GetType()));

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
            set
            {
                this.RaiseAndSetIfChanged(ref selectedLanguage, value);
                if (value != null)
                {
                    GetTemplates(value);
                }
            }
        }

        public ReactiveCommand<object> BrowseLocationCommand { get; }
        public override ReactiveCommand<object> OKCommand { get; protected set; }

    }
}
