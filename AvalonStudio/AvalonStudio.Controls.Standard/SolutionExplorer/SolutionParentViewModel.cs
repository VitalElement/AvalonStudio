namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia.Controls;
    using AvalonStudio.Extensibility;
    using AvalonStudio.MVVM;
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Shell;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public abstract class SolutionParentViewModel<T> : SolutionItemViewModel<T>, ISolutionParentViewModel
        where T : ISolutionFolder
    {
        private ObservableCollection<SolutionItemViewModel> _items;
        private bool _isExpanded;

        public SolutionParentViewModel(ISolutionParentViewModel parent, T model) : base(parent, model)
        {
            Items = new ObservableCollection<SolutionItemViewModel>();
            Items.BindCollections(Model.Items, p => { return SolutionItemViewModel.Create(this, p); }, (pvm, p) => pvm.Model == p);

            AddNewFolderCommand = ReactiveCommand.Create(() =>
            {
                Model.Solution.AddItem(SolutionFolder.Create("New Folder"), Model);

                Model.Solution.Save();
            });

            AddExistingProjectCommand = ReactiveCommand.Create(async () =>
            {
                var dlg = new OpenFileDialog();
                dlg.Title = "Open Project";

                var extensions = new List<string>();

                var shell = IoC.Get<IShell>();

                foreach (var projectType in shell.ProjectTypes)
                {
                    extensions.AddRange(projectType.Extensions);
                }

                dlg.Filters.Add(new FileDialogFilter { Name = "AvalonStudio Project", Extensions = extensions });

                if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
                {
                    dlg.InitialDirectory = Model.Solution.CurrentDirectory;
                }
                else
                {
                    dlg.InitialFileName = Model.Solution.CurrentDirectory;
                }

                dlg.AllowMultiple = false;

                var result = await dlg.ShowAsync();

                if (result != null && !string.IsNullOrEmpty(result.FirstOrDefault()))
                {
                    var proj = Project.LoadProjectFile(Model.Solution, result[0]);

                    if (proj != null)
                    {
                        Model.Solution.AddItem(proj, Model);
                        Model.Solution.Save();
                    }
                }
            });

            AddNewProjectCommand = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();

                shell.ModalDialog = new NewProjectDialogViewModel(Model);
                shell.ModalDialog.ShowDialog();
            });

            RemoveCommand = ReactiveCommand.Create(() =>
            {
                Model.Solution.RemoveItem(Model);
                Model.Solution.Save();
            });
        }

        public void VisitChildren(Action<SolutionItemViewModel> visitor)
        {
            foreach (var child in Items)
            {
                if (child is ISolutionParentViewModel folder)
                {
                    folder.VisitChildren(visitor);
                }

                visitor(child);
            }
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

                this.RaiseAndSetIfChanged(ref _isExpanded, value);

                this.RaisePropertyChanged(nameof(Icon));
            }
        }

        public ReactiveCommand AddNewFolderCommand { get; private set; }
        public ReactiveCommand AddNewProjectCommand { get; private set; }
        public ReactiveCommand AddExistingProjectCommand { get; private set; }
        public ReactiveCommand RemoveCommand { get; private set; }
    }
}