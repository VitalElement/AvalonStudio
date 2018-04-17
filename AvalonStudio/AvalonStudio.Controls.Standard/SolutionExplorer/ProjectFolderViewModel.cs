namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia.Media;
    using AvalonStudio.Extensibility;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using AvalonStudio.Shell;
    using ReactiveUI;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Reactive.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class ProjectFolderViewModel : ProjectItemViewModel<IProjectFolder>
    {
        private readonly IShell _shell;

        private DrawingGroup _folderOpenIcon;
        private DrawingGroup _folderIcon;
        private bool _isExpanded;

        public ProjectFolderViewModel(IProjectFolder model)
            : base(model)
        {
            _shell = IoC.Get<IShell>();

            Items = new ObservableCollection<ProjectItemViewModel>();
            Items.BindCollections(model.Items, p => { return Create(p); }, (pivm, p) => pivm.Model == p);                        

            NewItemCommand = ReactiveCommand.Create(() =>
            {
                _shell.ModalDialog = new NewItemDialogViewModel(model);
                _shell.ModalDialog.ShowDialog();
            });

            NewFileCommand = ReactiveCommand.Create(async () =>
            {
                var observable = Items.ObserveNewItems().OfType<SourceFileViewModel>().FirstOrDefaultAsync();

                using (var subscription = observable.Subscribe(item =>
                 {   
                     item.InEditMode = true;
                 }))
                {
                    File.CreateText(Path.Combine(model.Location, "NewFile1"));

                    await observable;
                }                             
            });

            RemoveCommand = ReactiveCommand.Create(() => model.Project.ExcludeFolder(model));

            _folderIcon = "FolderIcon".GetIcon();
            _folderOpenIcon = "FolderOpenIcon".GetIcon();
        }

        public ObservableCollection<ProjectItemViewModel> Items { get; }

        public ReactiveCommand NewFileCommand { get; }
        public ReactiveCommand NewItemCommand { get; }
        public ReactiveCommand RemoveCommand { get; }

        public override bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                this.RaisePropertyChanged(nameof(Icon));
            }
        }

        public override DrawingGroup Icon => IsExpanded ? _folderOpenIcon : _folderIcon;

        public static ProjectFolderViewModel Create(IProjectFolder model)
        {
            return new ProjectFolderViewModel(model);
        }
    }
}