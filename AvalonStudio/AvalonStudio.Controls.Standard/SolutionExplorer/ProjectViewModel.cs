using Avalonia.Media;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public abstract class ProjectViewModel : SolutionItemViewModel<IProject>
    {
        private readonly IShell shell;
        private ProjectConfigurationDialogViewModel configuration;

        private bool visibility;

        public ProjectViewModel(IProject model)
            : base(model)
        {
            shell = IoC.Get<IShell>();

            Items = new ObservableCollection<ProjectItemViewModel>();

            Items.BindCollections(model.Items, p => { return ProjectItemViewModel.Create(p); }, (pivm, p) => pivm.Model == p);

            ConfigureCommand = ReactiveCommand.Create(() =>
            {
                if (configuration == null)
                {
                    configuration = new ProjectConfigurationDialogViewModel(model, () =>
                    {
                        configuration = null;
                    });

                    shell.AddDocument(configuration);
                }
                else
                {
                    shell.SelectedDocument = configuration;
                }
                //shell.ModalDialog.ShowDialog();
            });

            DebugCommand = ReactiveCommand.Create(() =>
            {
                //shell.Debug(model);
            });

            BuildCommand = ReactiveCommand.Create(() => shell.Build(model));

            CleanCommand = ReactiveCommand.Create(()=>shell.Clean(model));

            ManageReferencesCommand = ReactiveCommand.Create(() => { });

            SetProjectCommand = ReactiveCommand.Create(()=>
            {
                model.Solution.StartupProject = model;
                model.Solution.Save();

                shell.InvalidateCodeAnalysis();

                //foreach (var project in solutionViewModel.Items)
                //{
                //    project.Invalidate();
                //}
            });

            OpenInExplorerCommand = ReactiveCommand.Create(() => Platform.OpenFolderInExplorer(Model.CurrentDirectory));

            NewItemCommand = ReactiveCommand.Create(() =>
            {
                shell.ModalDialog = new NewItemDialogViewModel(model);
                shell.ModalDialog.ShowDialog();
            });

            RemoveCommand = ReactiveCommand.Create(async () =>
            {
                await shell.CloseDocumentsForProjectAsync(Model);
                Model.Solution.RemoveProject(Model);
                Model.Solution.Save();
            });

            DevConsoleCommand = ReactiveCommand.Create(() => 
            {
                PlatformSupport.LaunchShell(Model.CurrentDirectory, Model.ToolChain?.BinDirectory, Model.Debugger2?.BinDirectory);
            });
        }

        public bool IsExpanded { get; set; }

        public string Title => Model.Name;

        public bool IsVisible => !Model.Hidden;

        public ObservableCollection<ProjectItemViewModel> Items { get; }

        public ReactiveCommand BuildCommand { get; protected set; }
        public ReactiveCommand CleanCommand { get; protected set; }
        public ReactiveCommand DebugCommand { get; protected set; }
        public ReactiveCommand ManageReferencesCommand { get; protected set; }
        public ReactiveCommand RemoveCommand { get; protected set; }
        public ReactiveCommand ConfigureCommand { get; }
        public ReactiveCommand SetProjectCommand { get; }
        public ReactiveCommand OpenInExplorerCommand { get; }
        public ReactiveCommand NewItemCommand { get; }

        public ReactiveCommand DevConsoleCommand { get; }

        public bool Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                this.RaisePropertyChanged();
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                if (Model == Model.Solution.StartupProject)
                {
                    return FontWeight.Bold;
                }

                return FontWeight.Normal;
            }
        }

        public ObservableCollection<string> IncludePaths { get; private set; }
    }
}