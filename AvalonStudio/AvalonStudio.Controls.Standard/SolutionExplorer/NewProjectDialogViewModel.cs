using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Languages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class NewProjectDialogViewModel : ModalDialogViewModelBase
    {
        private string location;

        private string name;

        private ObservableCollection<IProjectTemplate> projectTemplates;

        private ILanguageService selectedLanguage;

        private IProjectTemplate selectedTemplate;
        private readonly IShell shell;
        private ISolution solution;

        private string solutionName;

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
            BrowseLocationCommand.Subscribe(async o =>
            {
                var ofd = new OpenFolderDialog();
                ofd.InitialDirectory = location;

                var result = await ofd.ShowAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    Location = result;
                }
            });

            OKCommand = ReactiveCommand.Create(this.WhenAny(x => x.Location, x => x.SolutionName, (location, solution) => solution.Value != null && !Directory.Exists(Path.Combine(location.Value, solution.Value))));
            OKCommand.Subscribe(async o =>
            {
                bool generateSolutionDirs = false;

                if (solution == null)
                {
                    generateSolutionDirs = true;

                    var destination = Path.Combine(location, solutionName);
                    solution = Solution.Create(destination, solutionName, false);
                }

                if (await selectedTemplate.Generate(solution, name) != null)
                {
                    if (generateSolutionDirs)
                    {
                        if (!Directory.Exists(solution.CurrentDirectory))
                        {
                            Directory.CreateDirectory(solution.CurrentDirectory);
                        }
                    }
                }

                solution.Save();
                shell.CurrentSolution = solution;
                solution = null;

                Close();
            });
        }

        public string Name
        {
            get
            {
                return name;
            }
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
            get { return solution == null; }
        }

        public string Location
        {
            get { return location; }
            set { this.RaiseAndSetIfChanged(ref location, value); }
        }

        public string SolutionName
        {
            get { return solutionName; }
            set { this.RaiseAndSetIfChanged(ref solutionName, value); }
        }

        public IProjectTemplate SelectedTemplate
        {
            get
            {
                return selectedTemplate;
            }
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

        public ObservableCollection<IProjectTemplate> ProjectTemplates
        {
            get { return projectTemplates; }
            set { this.RaiseAndSetIfChanged(ref projectTemplates, value); }
        }

        public List<ILanguageService> Languages { get; set; }

        public ILanguageService SelectedLanguage
        {
            get
            {
                return selectedLanguage;
            }
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

        private void GetTemplates(ILanguageService languageService)
        {
            var templates = shell.ProjectTemplates.Where(t => languageService.BaseTemplateType.IsAssignableFrom(t.GetType()));

            ProjectTemplates = new ObservableCollection<IProjectTemplate>(templates);
        }
    }
}