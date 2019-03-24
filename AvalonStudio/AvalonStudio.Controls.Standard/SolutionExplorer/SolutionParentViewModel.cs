namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia;
    using Avalonia.Controls;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Studio;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using AvalonStudio.Shell;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;

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
                var observable = Items.ObserveNewItems().OfType<SolutionFolderViewModel>().FirstOrDefaultAsync();

                using (var subscription = observable.Subscribe(item =>
                {
                    item.InEditMode = true;
                }))
                {
                    Model.Solution.AddItem(SolutionFolder.Create("New Folder"), null, Model);

                    Model.Solution.Save();
                }                
            });

            AddExistingProjectCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var dlg = new OpenFileDialog();
                dlg.Title = "Open Project";

                foreach (var projectType in IoC.Get<IStudio>().ProjectTypes)
                {
                    var projectTypeMetadata = projectType.Metadata;
                    var extensions = new List<string>();

                    extensions.Add(projectTypeMetadata.DefaultExtension);
                    extensions.AddRange(projectTypeMetadata.PossibleExtensions);

                    dlg.Filters.Add(new FileDialogFilter() { Name = projectTypeMetadata.Description, Extensions = extensions });
                }

                dlg.InitialDirectory = Model.Solution.CurrentDirectory;

                dlg.AllowMultiple = false;

                var result = await dlg.ShowAsync(Application.Current.MainWindow);

                if (result != null && !string.IsNullOrEmpty(result.FirstOrDefault()))
                {
                    var projectTypeGuid = ProjectUtils.GetProjectTypeGuidForProject(result[0]);

                    if (projectTypeGuid.HasValue)
                    {
                        var proj = await ProjectUtils.LoadProjectFileAsync(Model.Solution, projectTypeGuid.Value, result[0]);

                        if (proj != null)
                        {
                            var observable = Items.ObserveNewItems().OfType<SolutionItemViewModel>().FirstOrDefaultAsync();

                            using (var subscription = observable.Subscribe(item =>
                            {
                                if (item is ProjectViewModel pvm)
                                {
                                    pvm.IsExpanded = true;
                                }
                            }))
                            {
                                Model.Solution.AddItem(proj, projectTypeGuid, Model);
                                Model.Solution.Save();

                                await observable;
                            }
                        }
                    }
                    else
                    {
                        IoC.Get<Utils.IConsole>().WriteLine(
                            $"The project '{result[0]}' isn't supported by any installed project type!");
                    }
                }
            });

            AddNewProjectCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var shell = IoC.Get<IShell>();

                shell.ModalDialog = new NewProjectDialogViewModel(Model);

                if (await shell.ModalDialog.ShowDialogAsync())
                {
                    var observable = Items.ObserveNewItems().OfType<SolutionItemViewModel>().FirstOrDefaultAsync();

                    using (var subscription = observable.Subscribe(item =>
                    {
                        if (item is ProjectViewModel pvm)
                        {
                            pvm.IsExpanded = true;
                        }
                    }))
                    {
                        await observable;
                    }
                }
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

        public ReactiveCommand<Unit, Unit> AddNewFolderCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> AddNewProjectCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> AddExistingProjectCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; private set; }
    }
}