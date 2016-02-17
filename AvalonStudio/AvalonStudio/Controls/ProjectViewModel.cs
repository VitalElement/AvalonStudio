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
    public abstract class ProjectViewModel : ViewModel<IProject>
    {
        public ProjectViewModel(IProject model)
            : base(model)
        {
            items = new ObservableCollection<ViewModel>();

            items.Add(new ReferenceFolderViewModel(model));

            foreach (var item in model.Items)
            {
                items.Add(ProjectItemViewModel.Create(item));
            }

            ConfigureCommand = ReactiveCommand.Create();

            ConfigureCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ModalDialog = new ProjectConfigurationDialogViewModel(model, () => { });
                WorkspaceViewModel.Instance.ModalDialog.ShowDialog();
            });

            BuildCommand = ReactiveCommand.Create();

            BuildCommand.Subscribe(async (o) =>
            {
                WorkspaceViewModel.Instance.Console.Clear();
                await model.ToolChain?.Build(WorkspaceViewModel.Instance.Console, model);
            });

            CleanCommand = ReactiveCommand.Create();

            CleanCommand.Subscribe(async (o) =>
            {
                WorkspaceViewModel.Instance.Console.Clear();
                await model.ToolChain?.Clean(WorkspaceViewModel.Instance.Console, model);
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

        private ObservableCollection<ViewModel> items;
        public ObservableCollection<ViewModel> Items
        {
            get { return items; }
            set { this.RaiseAndSetIfChanged(ref items, value); }
        }

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
