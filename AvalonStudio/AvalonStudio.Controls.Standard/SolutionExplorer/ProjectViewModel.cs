using Avalonia.Media;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public abstract class ProjectViewModel : SolutionItemViewModel<IProject>
    {
        private readonly IStudio studio;
        private readonly IShell shell;
        private ProjectConfigurationDialogViewModel configuration;
        private bool _isExpanded;
        private bool visibility;

        public ProjectViewModel(ISolutionParentViewModel parent, IProject model)
            : base(parent, model)
        {
            studio = IoC.Get<IStudio>();
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

                    shell.AddDocument(configuration, false);
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

            BuildCommand = ReactiveCommand.CreateFromTask(async () => await studio.BuildAsync(model));

            CleanCommand = ReactiveCommand.Create(() => studio.Clean(model));

            ManageReferencesCommand = ReactiveCommand.Create(() => { });

            SetProjectCommand = ReactiveCommand.Create(() =>
            {
                model.Solution.StartupProject = model;
                model.Solution.Save();

                studio.InvalidateCodeAnalysis();

                var root = this.FindRoot();

                if (root != null)
                {
                    root.VisitChildren(solutionItem =>
                    {
                        solutionItem.RaisePropertyChanged(nameof(FontWeight));
                    });
                }

            });

            OpenInExplorerCommand = ReactiveCommand.Create(() => Platform.OpenFolderInExplorer(Model.CurrentDirectory));

            NewItemCommand = ReactiveCommand.Create(() =>
            {
                shell.ModalDialog = new NewItemDialogViewModel(model);
                shell.ModalDialog.ShowDialogAsync();
            });

            RemoveCommand = ReactiveCommand.Create(() =>
            {
                studio.CloseDocumentsForProject(Model);
                Model.Solution.RemoveItem(Model);
                Model.Solution.Save();
            });

            DevConsoleCommand = ReactiveCommand.Create(() =>
            {
                PlatformSupport.LaunchShell(Model.CurrentDirectory, Model.ToolChain?.BinDirectory, Model.Debugger2?.BinDirectory);
            });
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { this.RaiseAndSetIfChanged(ref _isExpanded, value); }
        }

        public bool IsVisible => !Model.Hidden;

        public ObservableCollection<ProjectItemViewModel> Items { get; }

        public ReactiveCommand<Unit, bool> BuildCommand { get; protected set; }
        public ReactiveCommand<Unit, Unit> CleanCommand { get; protected set; }
        public ReactiveCommand<Unit, Unit> DebugCommand { get; protected set; }
        public ReactiveCommand<Unit, Unit> ManageReferencesCommand { get; protected set; }
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; protected set; }
        public ReactiveCommand<Unit, Unit> ConfigureCommand { get; }
        public ReactiveCommand<Unit, Unit> SetProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenInExplorerCommand { get; }
        public ReactiveCommand<Unit, Unit> NewItemCommand { get; }

        public ReactiveCommand<Unit, Unit> DevConsoleCommand { get; }

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