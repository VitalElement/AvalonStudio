using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class SolutionExplorerViewModel : ToolViewModel, IExtension, ISolutionExplorer
    {
        public const string ToolId = "CIDSEVM00";

        private ISolution model;

        private ProjectItemViewModel selectedItem;

        private IProject selectedProject;
        private IShell shell;

        private ObservableCollection<SolutionViewModel> solution;

        public SolutionExplorerViewModel()
        {
            Title = "Solution Explorer";
            solution = new ObservableCollection<SolutionViewModel>();
        }

        public new ISolution Model
        {
            get
            {
                return model;
            }
            set
            {
                if (model != null)
                {
                    foreach(var model in model.Projects)
                    {
                        model.Dispose();
                    }

                    model.Projects.Clear();
                    GC.Collect();
                }

                SelectedProject = null;
                
                model = value;

                if (Model != null)
                {
                    if (Model.Projects.Count > 0)
                    {
                        SelectedProject = Model.StartupProject;
                    }

                    var sol = new ObservableCollection<SolutionViewModel>();
                    sol.Add(new SolutionViewModel(model));

                    Solution = sol;
                }
                else
                {
                    Solution = null;
                }

                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(Projects));
            }
        }

        public ObservableCollection<SolutionViewModel> Solution
        {
            get { return solution; }
            set { this.RaiseAndSetIfChanged(ref solution, value); }
        }

        public IProject SelectedProject
        {
            get
            {
                return selectedProject;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedProject, value);
            }
        }

        public ProjectItemViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);

                if (value is SourceFileViewModel)
                {
                    shell.OpenDocument((ISourceFile)(value as SourceFileViewModel).Model, 1);
                }
            }
        }

        public override Location DefaultLocation
        {
            get { return Location.Right; }
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<ISolutionExplorer>(this, typeof(ISolutionExplorer));
        }

        public void Activation()
        {
            shell = IoC.Get<IShell>();

            shell.SolutionChanged += async (sender, e) =>
            {
                Model = shell.CurrentSolution;

                if (Model != null)
                {
                    var languageService = shell.LanguageServices.FirstOrDefault(o => o.CanHandle(Model.StartupProject));

                    if (languageService != null)
                    {
                        await languageService.AnalyseProjectAsync(Model.StartupProject);
                    }
                }
            };
		}

        public void NewSolution()
        {
            shell.ModalDialog = new NewProjectDialogViewModel(shell.CurrentSolution);
            shell.ModalDialog.ShowDialog();
        }

        public async void OpenSolution()
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Open Solution";

            if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
            {
                dlg.InitialDirectory = Platform.ProjectDirectory;
            }
            else
            {
                dlg.InitialFileName = Platform.ProjectDirectory;
            }

            var allExtensions = new List<string>();

            foreach (var solutionType in shell.SolutionTypes)
            {
                allExtensions.AddRange(solutionType.Extensions);
            }

            dlg.Filters.Add(new FileDialogFilter
            {
                Name = "All Supported Solution Types",
                Extensions = allExtensions
            });

            foreach (var solutionType in shell.SolutionTypes)
            {
                dlg.Filters.Add(new FileDialogFilter
                {
                    Name = solutionType.Description,
                    Extensions = solutionType.Extensions
                });
            }
            
            var result = await dlg.ShowAsync();

            if (result != null && !string.IsNullOrEmpty(result.FirstOrDefault()))
            {
                await shell.OpenSolutionAsync(result[0]);
            }
        }
    }
}