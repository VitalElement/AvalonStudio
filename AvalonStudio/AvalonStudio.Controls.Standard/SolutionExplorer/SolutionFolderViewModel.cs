namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia.Media;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using ReactiveUI;
    using System.Collections.ObjectModel;

    public class SolutionFolderViewModel : SolutionItemViewModel<ISolutionFolder>
    {
        private DrawingGroup _folderOpenIcon;
        private DrawingGroup _folderIcon;
        private ObservableCollection<SolutionItemViewModel> _items;
        private bool _isExpanded;

        public SolutionFolderViewModel(SolutionViewModel solution, ISolutionFolder folder) : base(folder)
        {
            Items = new ObservableCollection<SolutionItemViewModel>();
            Items.BindCollections(folder.Items, p => { return SolutionItemViewModel.Create(solution, p); },(pvm, p) => pvm.Model == p);

            _folderIcon = "FolderIcon".GetIcon();
            _folderOpenIcon = "FolderOpenIcon".GetIcon();
        }

        public ObservableCollection<SolutionItemViewModel> Items
        {
            get
            {
                return _items;
            }

            set
            {
                _items = value; this.RaisePropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                this.RaisePropertyChanged(nameof(Icon));
            }
        }

        public override DrawingGroup Icon => IsExpanded ? _folderOpenIcon : _folderIcon;
    }

    //public abstract class SolutionParentViewModel<T> : SolutionItemViewModel where T : ISolutionFolder
    //{
    //    public SolutionParentViewModel(T model)
    //    {
    //        Children = new ObservableCollection<ViewModel>();
    //        Children.BindCollections(Model.Children, (p) => ReactiveObjectExtensions.Create(p), (vm, m) => vm.Model == m);

    //        AddNewFolderCommand = ReactiveCommand.Create((args) =>
    //        {
    //            //Workspace.Instance.ModalDialog = new NewSolutionFolderViewModel(this.model as SolutionFolder);
    //            //Workspace.Instance.ModalDialog.ShowDialog();
    //        });

    //        AddNewProjectCommand = ReactiveCommand.Create();
    //        AddNewProjectCommand.Subscribe((o) =>
    //        {
    //            //Workspace.Instance.ModalDialog = new NewProjectDialogViewModel(Workspace.Instance, Model, false);
    //            //Workspace.Instance.ModalDialog.ShowDialog();
    //        });

    //        AddExistingProjectCommand = ReactiveCommand.Create();
    //        AddExistingProjectCommand.Subscribe(async (o) =>
    //        {
    //            Avalonia.Controls.OpenFileDialog ofd = new Avalonia.Controls.OpenFileDialog();
    //            ofd.InitialDirectory = model.Solution.CurrentDirectory;

    //            // ofd.Filter = "VEStudio Project Files (*" + VEStudioService.ProjectExtension + ")|*" + VEStudioService.ProjectExtension;

    //            var result = await ofd.ShowAsync();

    //            if (result.Length == 1)
    //            {
    //                var unloadedProject = new UnloadedProject(Model, Model.Solution.CurrentDirectory.MakeRelativePath(result[0]));
    //                var project = Project.LoadProject(Model.Solution, unloadedProject);

    //                if (!Model.Solution.ContainsProject(project))
    //                {
    //                    model.AddProject(project, unloadedProject);
    //                }
    //            }
    //        });

    //        RemoveCommand = ReactiveCommand.Create();
    //        RemoveCommand.Subscribe((o) =>
    //        {
    //            model.Solution.GetParent(Model).RemoveItem(Model);
    //        });
    //    }

    //    public bool VisitAllChildren(Func<ProjectItemViewModel, bool> func)
    //    {
    //        if (func(this))
    //        {
    //            return true;
    //        }

    //        foreach (ProjectItemViewModel item in this.Children)
    //        {
    //            if (item is SolutionFolderViewModel)
    //            {
    //                if ((item as SolutionFolderViewModel).VisitAllChildren(func))
    //                {
    //                    return true;
    //                }

    //                if (func(item))
    //                {
    //                    return true;
    //                }
    //            }
    //            else if (item is ProjectViewModel)
    //            {
    //                if (func(item))
    //                {
    //                    return true;
    //                }
    //            }
    //        }

    //        return false;
    //    }

    //    public ReactiveCommand AddNewFolderCommand { get; private set; }
    //    public ReactiveCommand AddNewProjectCommand { get; private set; }
    //    public ReactiveCommand AddExistingProjectCommand { get; private set; }
    //    public ReactiveCommand RemoveCommand { get; private set; }

    //    new public SolutionFolder Model
    //    {
    //        get { return base.Model as SolutionFolder; }
    //    }


}