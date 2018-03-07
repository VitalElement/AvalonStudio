namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia.Controls;
    using AvalonStudio.Extensibility;
    using AvalonStudio.MVVM;
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
                Model.Solution.AddItem(SolutionFolder.Create("New Folder"), null, Model);

                Model.Solution.Save();
            });

            AddExistingProjectCommand = ReactiveCommand.Create(async () =>
            {
                var dlg = new OpenFileDialog();
                dlg.Title = "Open Project";

                var shell = IoC.Get<IShell>();

                foreach (var projectType in shell.ProjectTypes)
                {
                    var projectTypeMetadata = projectType.Metadata;
                    var extensions = new List<string>();

                    extensions.Add(projectTypeMetadata.DefaultExtension);
                    extensions.AddRange(projectTypeMetadata.PossibleExtensions);

                    dlg.Filters.Add(new FileDialogFilter() { Name = projectTypeMetadata.Description, Extensions = extensions });
                }
                
                dlg.InitialDirectory = Model.Solution.CurrentDirectory;

                dlg.AllowMultiple = false;

                var result = await dlg.ShowAsync();

                if (result != null && !string.IsNullOrEmpty(result.FirstOrDefault()))
                {
                    var projectTypeGuid = Project.GetProjectTypeGuidForProject(result[0]);

                    if (projectTypeGuid.HasValue)
                    {
                        var proj = await Project.LoadProjectFileAsync(Model.Solution, projectTypeGuid.Value, result[0]);

                        if (proj != null)
                        {
                            Model.Solution.AddItem(proj, projectTypeGuid, Model);
                            Model.Solution.Save();
                        }
                    }
                    else
                    {
                        IoC.Get<Utils.IConsole>().WriteLine(
                            $"The project '{result[0]}' isn't supported by any installed project type!");
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