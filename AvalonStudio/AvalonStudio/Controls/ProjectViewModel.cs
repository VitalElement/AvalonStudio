namespace AvalonStudio.Controls.ViewModels
{
    using MVVM;
    using Perspex.Media;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    public abstract class ProjectViewModel : ViewModel<IProject>
    {
        public ProjectViewModel(IProject model)
            : base(model)
        {
            Items = new ObservableCollection<ProjectItemViewModel>();

            Items.BindCollections(model.Items, (p) => { return ProjectItemViewModel.Create(p); }, (pivm, p) => pivm.Model == p);

            ConfigureCommand = ReactiveCommand.Create();

            ConfigureCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ModalDialog = new ProjectConfigurationDialogViewModel(model, () => { });
                WorkspaceViewModel.Instance.ModalDialog.ShowDialog();
            });

            DebugCommand = ReactiveCommand.Create();
            DebugCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.DebugManager.StartDebug(Model);
            });

            BuildCommand = ReactiveCommand.Create();
            BuildCommand.Subscribe(async (o) =>
            {
                WorkspaceViewModel.Instance.Console.Clear();

                if (model.ToolChain != null)
                {
                    await Task.Factory.StartNew(() => model.ToolChain.Build(WorkspaceViewModel.Instance.Console, model));
                }
                else
                {
                    WorkspaceViewModel.Instance.Console.WriteLine("Project does not define a toolchain.");
                }
            });

            CleanCommand = ReactiveCommand.Create();

            CleanCommand.Subscribe(async (o) =>
            {
                WorkspaceViewModel.Instance.Console.Clear();

                if (model.ToolChain != null)
                {
                    await model.ToolChain.Clean(WorkspaceViewModel.Instance.Console, model);
                }
                else
                {
                    WorkspaceViewModel.Instance.Console.WriteLine("Project does not define a toolchain.");
                }                                
            });

            ManageReferencesCommand = ReactiveCommand.Create();

            ManageReferencesCommand.Subscribe((o) =>
            {
            });

            SetProjectCommand = ReactiveCommand.Create();

            SetProjectCommand.Subscribe((o) =>
            {
                model.Solution.StartupProject = model;
                model.Solution.Save();

                WorkspaceViewModel.Instance.InvalidateCodeAnalysis();

                foreach(var project in WorkspaceViewModel.Instance.SolutionExplorer.Solution.First().Projects)
                {
                    project.Invalidate();
                }
            });

            OpenInExplorerCommand = ReactiveCommand.Create();
            OpenInExplorerCommand.Subscribe((o) =>
            {
                Process.Start(Model.CurrentDirectory);
            });

            RemoveCommand = ReactiveCommand.Create();
            RemoveCommand.Subscribe((o) =>
            {
                Model.Solution.RemoveProject(Model);
                Model.Solution.Save();
            });
        }

        public bool IsExpanded { get; set; }

        public string Title
        {
            get
            {
                return Model.Name;
            }
        }

        public bool IsVisible
        {
            get
            {
                return !Model.Hidden;
            }
        }

        public ObservableCollection<ProjectItemViewModel> Items { get; private set; }        

        public ReactiveCommand<object> BuildCommand { get; protected set; }
        public ReactiveCommand<object> CleanCommand { get; protected set; }
        public ReactiveCommand<object> DebugCommand { get; protected set; }
        public ReactiveCommand<object> ManageReferencesCommand { get; protected set; }
        public ReactiveCommand<object> RemoveCommand { get; protected set; }
        public ReactiveCommand<object> ConfigureCommand { get; private set; }
        public ReactiveCommand<object> SetProjectCommand { get; private set; }
        public ReactiveCommand<object> OpenInExplorerCommand { get; }

        public static ProjectViewModel Create(IProject model)
        {
            ProjectViewModel result = new StandardProjectViewModel(model);

            return result;
        }

        private bool visibility;
        public bool Visibility
        {
            get { return visibility; }
            set { visibility = value; this.RaisePropertyChanged(); }
        }

        public FontWeight FontWeight
        {
            get
            {
                if (Model == Model.Solution.StartupProject)
                {
                    return FontWeight.Bold;
                }
                else
                {
                    return FontWeight.Normal;
                }
            }
        }

        public ObservableCollection<string> IncludePaths { get; private set; }


    }
}
