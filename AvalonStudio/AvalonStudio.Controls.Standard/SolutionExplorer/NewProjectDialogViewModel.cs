using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Extensibility.Templating;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class NewProjectDialogViewModel : ModalDialogViewModelBase
    {
        private string location;

        private string name;

        private Lazy<IDictionary<string, IEnumerable<ITemplate>>> _allProjectTemplates;
        private IEnumerable<ITemplate> _projectTemplates;

        private string _selectedLanguage;

        private ITemplate selectedTemplate;
        private ISolutionFolder _solutionFolder;

        private IStudio _studio;        
        private TemplateManager _templateManager;

        private string solutionName;

        private IEnumerable<string> GetProjectFiles(string path)
        {
            var files = Directory.EnumerateFiles(path);

            var ptExtensions = IoC.Get<IStudio>().ProjectTypes.Select(pt => pt.Metadata.DefaultExtension);

            var result = files.Where(f => ptExtensions.Contains(Path.GetExtension(f).Replace(".", "")));

            var directories = Directory.EnumerateDirectories(path);

            foreach (var directory in directories)
            {
                result = result.Concat(GetProjectFiles(directory));
            }

            return result;
        }

        public NewProjectDialogViewModel(ISolutionFolder solutionFolder) : this()
        {
            _solutionFolder = solutionFolder;

            if (_solutionFolder != null)
            {
                location = _solutionFolder.Solution.CurrentDirectory;
            }
        }

        public NewProjectDialogViewModel()
            : base("New Project", true, true)
        {
            _studio = IoC.Get<IStudio>();
            _templateManager = IoC.Get<TemplateManager>();

            _allProjectTemplates = new Lazy<IDictionary<string, IEnumerable<ITemplate>>>(_templateManager.GetProjectTemplates);

            location = Platform.ProjectDirectory;

            SelectedLanguage = Languages.FirstOrDefault();
            SelectedTemplate = ProjectTemplates.FirstOrDefault();

            BrowseLocationCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var ofd = new OpenFolderDialog
                {
                    InitialDirectory = location
                };

                var result = await ofd.ShowAsync(Application.Current.MainWindow);

                if (!string.IsNullOrEmpty(result))
                {
                    Location = result;
                }
            });

            OKCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                Close();

                bool loadNewSolution = false;

                if (_solutionFolder == null)
                {
                    IoC.Get<IStatusBar>().SetText("Creating new Solution...");
                    loadNewSolution = true;

                    var destination = Path.Combine(location, solutionName);
                    location = destination;
                    _solutionFolder = VisualStudioSolution.Create(destination, solutionName, false, VisualStudioSolution.Extension);
                }
                else
                {
                    IoC.Get<IStatusBar>().SetText("Creating new project...");
                }

                var templateManager = IoC.Get<TemplateManager>();

                var templateDestination = Path.Combine(Location, name);

                if (await templateManager.CreateTemplate(selectedTemplate, templateDestination, ("name", name)) == CreationResult.Success)
                {
                    var projectFiles = GetProjectFiles(templateDestination);

                    bool defaultSet = _solutionFolder.Solution.StartupProject != null;

                    foreach (var projectFile in projectFiles)
                    {
                        var projectTypeGuid = ProjectUtils.GetProjectTypeGuidForProject(projectFile);

                        if (projectTypeGuid.HasValue)
                        {
                            var project = await ProjectUtils.LoadProjectFileAsync(
                                _solutionFolder.Solution, projectTypeGuid.Value, projectFile);

                            if (project != null)
                            {
                                _solutionFolder.Solution.AddItem(project, projectTypeGuid, _solutionFolder);
                            }

                            if (!defaultSet)
                            {
                                defaultSet = true;
                                _solutionFolder.Solution.StartupProject = project;
                            }

                            if (!loadNewSolution)
                            {
                                await project.LoadFilesAsync();

                                await project.ResolveReferencesAsync();
                            }
                        }
                        else
                        {
                            IoC.Get<Utils.IConsole>().WriteLine(
                                $"The project '{projectFile}' isn't supported by any installed project type!");
                        }
                    }
                }

                _solutionFolder.Solution.Save();

                if (loadNewSolution)
                {
                    await _studio.OpenSolutionAsync(_solutionFolder.Solution.Location);
                }
                else
                {
                    await _solutionFolder.Solution.RestoreSolutionAsync();
                }

                _solutionFolder = null;

                IoC.Get<IStatusBar>().ClearText();
            },
            this.WhenAny(x => x.Location, x => x.SolutionName, (location, solution) => solution.Value != null && !Directory.Exists(Path.Combine(location.Value, solution.Value))));

            UpdateTemplatesCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var templateManager = IoC.Get<TemplateManager>();

                await Task.Run(() =>
                {                    
                    templateManager.UpdateDefaultTemplates();

                    _allProjectTemplates = new Lazy<IDictionary<string, IEnumerable<ITemplate>>>(_templateManager.GetProjectTemplates);

                    SelectedLanguage = SelectedLanguage;
                });
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
            get { return _solutionFolder == null; }
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

        public ITemplate SelectedTemplate
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
                    Name = Platform.NextAvailableDirectoryName(value.DefaultName);
                }

                SolutionName = name;
            }
        }

        public IEnumerable<ITemplate> ProjectTemplates
        {
            get { return _projectTemplates; }
            set { this.RaiseAndSetIfChanged(ref _projectTemplates, value); }
        }

        public IEnumerable<string> Languages => _allProjectTemplates.Value.Keys;

        public string SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedLanguage, value);

                if (value != null && _allProjectTemplates.Value.TryGetValue(value, out var templates))
                {
                    ProjectTemplates = templates;
                }
            }
        }

        public ReactiveCommand<Unit, Unit> BrowseLocationCommand { get; }
        public override ReactiveCommand<Unit, Unit> OKCommand { get; protected set; }
        public ReactiveCommand<Unit, Unit> UpdateTemplatesCommand { get; }
    }
}