namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia.Controls;
    using Avalonia.Media;
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

    public class SolutionFolderViewModel : SolutionParentViewModel<ISolutionFolder>
    {
        private DrawingGroup _folderOpenIcon;
        private DrawingGroup _folderIcon;

        public SolutionFolderViewModel(ISolutionFolder folder) : base(folder)
        {
            _folderIcon = "FolderIcon".GetIcon();
            _folderOpenIcon = "FolderOpenIcon".GetIcon();
        }

        public override DrawingGroup Icon => IsExpanded ? _folderOpenIcon : _folderIcon;
    }

    public abstract class SolutionParentViewModel<T> : SolutionItemViewModel<T> where T : ISolutionFolder
    {
        private ObservableCollection<SolutionItemViewModel> _items;
        private bool _isExpanded;

        public SolutionParentViewModel(T model) : base(model)
        {
            Items = new ObservableCollection<SolutionItemViewModel>();
            Items.BindCollections(model.Items, p => { return SolutionItemViewModel.Create(p); }, (pvm, p) => pvm.Model == p);

            AddNewFolderCommand = ReactiveCommand.Create(() =>
            {
                Model.Solution.AddFolder(SolutionFolder.Create("New Folder", Model));

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
                    var proj = AvalonStudioSolution.LoadProjectFile(Model.Solution, result[0]);

                    (proj as ISolutionItem).Parent = Model;

                    if (proj != null)
                    {
                        Model.Solution.AddProject(proj);
                        Model.Solution.Save();
                    }
                }
            });

            AddNewProjectCommand = ReactiveCommand.Create(() =>
            {
                
            });

            RemoveCommand = ReactiveCommand.Create(() =>
            {
                Model.Solution.RemoveItem(Model);
                Model.Solution.Save();
            });
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

        public ReactiveCommand AddNewFolderCommand { get; private set; }
        public ReactiveCommand AddNewProjectCommand { get; private set; }
        public ReactiveCommand AddExistingProjectCommand { get; private set; }
        public ReactiveCommand RemoveCommand { get; private set; }        
    }
}