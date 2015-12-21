
namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.Models.Solutions;
    using AvalonStudio.MVVM;
    using Perspex.Controls;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Input;

    public abstract class ProjectItemViewModel : ViewModel
    {
        public ProjectItemViewModel()
        { 
            ToggleEditingModeCommand = ReactiveCommand.Create();

            ToggleEditingModeCommand.Subscribe(args =>
            {
                if (((object)Workspace.Instance.SolutionExplorer.SelectedItem) == (object)this && NumberOfSelections > 1)
                {
                    IsEditingTitle = (bool)args;
                }
            });

            RemoveItemCommand = ReactiveCommand.Create();
            RemoveItemCommand.Subscribe((o) =>
            {
                //if (model is EditorViewModel)
                //{
                //    //(this.model as EditorViewModel).CloseCommand.Execute (null);
                //}

                //if (model is ProjectItem)
                //{
                //    (model as ProjectItem).Container.RemoveItem(model as ProjectItem);
                //}
            });

            OpenInExplorerCommand = ReactiveCommand.Create();
            OpenInExplorerCommand.Subscribe((o) =>
            {
                //if (model is ProjectItem)
                //{
                //    Process.Start((model as ProjectItem).CurrentDirectory);
                //}
            });

            textBoxVisibility = false;
            labelVisibility = true;
        }

        public abstract bool CanAcceptDrop(Type type);

        public abstract void Drop(ProjectItemViewModel item);

        public string Title
        {
            get { return this.Model.FileName; }
            set { this.Model.FileName = value; this.RaisePropertyChanged(); IsEditingTitle = false; }
        }

        public int NumberOfSelections { get; set; }

        public string TitleWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Title);
            }
        }

        private bool isEditingTitle;
        public bool IsEditingTitle
        {
            get { return isEditingTitle; }
            set
            {
                this.RaiseAndSetIfChanged(ref isEditingTitle, value);
                LabelVisibility = !value;
                TextBoxVisibility = value;
            }
        }

        public bool IsExpanded
        {
            get { return Model.UserData.IsExpanded; }
            set
            {
                if (!IsEditingTitle)
                {
                    Model.UserData.IsExpanded = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        
        new public Item Model
        {
            get { return base.Model as Item; }            
        }

        public static ProjectItemViewModel Create(Item item)
        {
            ProjectItemViewModel result = null;

            //if (item is Project)
            //{
            //    result = ProjectViewModel.Create(item as IProject);
            //}

            //if (item is SolutionFolder)
            //{
            //    result = SolutionFolderViewModel.Create(item as SolutionFolder);
            //}

            return result;
        }

        public ReactiveCommand<object> RemoveItemCommand { get; private set; }
        public ReactiveCommand<object> ToggleEditingModeCommand { get; private set; }
        public ReactiveCommand<object> OpenInExplorerCommand { get; protected set; }

        private bool textBoxVisibility;
        public bool TextBoxVisibility
        {
            get { return textBoxVisibility; }
            set { this.RaiseAndSetIfChanged(ref textBoxVisibility, value); }
        }

        private bool labelVisibility;
        public bool LabelVisibility
        {
            get { return labelVisibility; }
            set { this.RaiseAndSetIfChanged(ref labelVisibility, value); }
        }


    }

    public abstract class ProjectParentViewModel<T> : ProjectItemViewModel where T : ProjectFolder
    {
        public ProjectParentViewModel(T model)
        {
            Children = new ObservableCollection<ViewModel>();
            Children.BindCollections((model as ProjectFolder).Children, (p) => ReactiveObjectExtensions.Create(p), (vm, m) => vm.Model == m);

            AddNewFolderCommand = ReactiveCommand.Create();
            AddNewFolderCommand.Subscribe((args) =>
          {
               //Workspace.Instance.ModalDialog = new NewFolderDialogViewModel (this.model as ProjectFolder);
               // Workspace.Instance.ModalDialog.ShowDialog ();
           });


            AddNewFileCommand = ReactiveCommand.Create();
            AddNewFileCommand.Subscribe((args) =>
           {
               //Workspace.Instance.ModalDialog = new NewFileDialogViewModel (this.model as ProjectFolder);
               //Workspace.Instance.ModalDialog.ShowDialog ();
           });


            AddExistingFileCommand = ReactiveCommand.Create();
            AddExistingFileCommand.Subscribe(async (o) =>
          {
              var ofd = new OpenFileDialog();

              ofd.InitialDirectory = (model as ProjectFolder).CurrentDirectory;
               //ofd.Filters.Add(new FileDialogFilter() {  = "C Source Files (*.c;*.h;*.cpp;*.hpp)|*.c;*.h;*.cpp;*.hpp|All Files (*.*)|*.*";

               var result = await ofd.ShowAsync();

              if (result.Length == 1)
              {
                  (this.Model as ProjectFolder).AddExistingFile(result[0]);
              }
          });

            ImportFolderCommand = ReactiveCommand.Create();
            ImportFolderCommand.Subscribe((o) =>
           {
               //FolderBrowserDialog fbd = new FolderBrowserDialog();

               //fbd.SelectedPath = (model as ProjectFolder).CurrentDirectory;

               //if (fbd.ShowDialog () == DialogResult.OK)
               //{
               //    (model as ProjectFolder).ImportFolder (fbd.SelectedPath);
               //}
           });

            AddNewItemCommand = ReactiveCommand.Create();
            AddNewItemCommand.Subscribe((o) =>
           {
               //Workspace.Instance.ModalDialog = new NewItemDialogViewModel (model as ProjectFolder);
               //Workspace.Instance.ModalDialog.ShowDialog ();
           });
        }

        public ReactiveCommand<object> AddNewFolderCommand { get; private set; }
        public ReactiveCommand<object> AddNewFileCommand { get; private set; }
        public ReactiveCommand<object> AddNewItemCommand { get; private set; }
        public ReactiveCommand<object> AddExistingFileCommand { get; private set; }
        public ReactiveCommand<object> ImportFolderCommand { get; private set; }


        private ObservableCollection<ViewModel> children;
        public ObservableCollection<ViewModel> Children
        {
            get { return children; }
            set { this.RaiseAndSetIfChanged(ref children, value); }
        }
    }
}
