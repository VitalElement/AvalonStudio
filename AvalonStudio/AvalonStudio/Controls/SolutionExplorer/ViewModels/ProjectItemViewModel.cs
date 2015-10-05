namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.Models.Solutions;
    using AvalonStudio.MVVM;
    using Perspex.Controls;
    using Perspex.MVVM;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Input;

    public abstract class ProjectItemViewModel : ViewModelBase
    {
        public ProjectItemViewModel(object model)
            : base(model)
        {
            this.ToggleEditingMode = new RoutingCommand((args) =>
           {
               if (((object)Workspace.This.SolutionExplorer.SelectedItem) == (object)this && NumberOfSelections > 1)
               {
                   IsEditingTitle = (bool)args;
               }
           });

            this.RemoveItemCommand = new RoutingCommand((o) =>
           {
               if (this.model is EditorViewModel)
               {
                   //(this.model as EditorViewModel).CloseCommand.Execute (null);
               }

               if (model is ProjectItem)
               {
                   (model as ProjectItem).Container.RemoveItem(model as ProjectItem);
               }
           });

            OpenInExplorerCommand = new RoutingCommand((o) =>
           {
               if (model is ProjectItem)
               {
                   Process.Start((model as ProjectItem).CurrentDirectory);
               }
           });

            textBoxVisibility = false;
            labelVisibility = true;
        }

        public abstract bool CanAcceptDrop(Type type);

        public abstract void Drop(ProjectItemViewModel item);

        public string Title
        {
            get { return this.Model.FileName; }
            set { this.Model.FileName = value; OnPropertyChanged(); IsEditingTitle = false; }
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
                isEditingTitle = value;

                if (value)
                {
                    LabelVisibility = false;
                    TextBoxVisibility = true;
                }
                else
                {
                    LabelVisibility = true;
                    TextBoxVisibility = false;
                }

                OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public Item Model
        {
            get
            {
                return BaseModel as Item;
            }
        }

        public static ProjectItemViewModel Create(Item item)
        {
            ProjectItemViewModel result = null;

            if (item is Project)
            {
                result = ProjectViewModel.Create(item as Project);
            }

            if (item is SolutionFolder)
            {
                result = SolutionFolderViewModel.Create(item as SolutionFolder);
            }

            return result;
        }

        public ICommand RemoveItemCommand { get; private set; }
        public ICommand ToggleEditingMode { get; private set; }
        public ICommand OpenInExplorerCommand { get; protected set; }

        private bool textBoxVisibility;
        public bool TextBoxVisibility
        {
            get { return textBoxVisibility; }
            set { textBoxVisibility = value; OnPropertyChanged(); }
        }

        private bool labelVisibility;
        public bool LabelVisibility
        {
            get { return labelVisibility; }
            set { labelVisibility = value; OnPropertyChanged(); }
        }


    }

    public abstract class ProjectParentViewModel<T> : ProjectItemViewModel where T : ProjectFolder
    {
        public ProjectParentViewModel(T model)
            : base(model)
        {
            Children = new ObservableCollection<ViewModelBase>();
            Children.Bind((this.model as ProjectFolder).Children, (p) => ViewModelBaseExtensions.Create(p), (vm, m) => vm.BaseModel == m);

            this.AddNewFolderCommand = new RoutingCommand((args) =>
           {
               //Workspace.This.ModalDialog = new NewFolderDialogViewModel (this.model as ProjectFolder);
               // Workspace.This.ModalDialog.ShowDialog ();
           });


            this.AddNewFileCommand = new RoutingCommand((args) =>
           {
               //Workspace.This.ModalDialog = new NewFileDialogViewModel (this.model as ProjectFolder);
               //Workspace.This.ModalDialog.ShowDialog ();
           });


            this.AddExistingFileCommand = new RoutingCommand(async (o) =>
           {
               var ofd = new OpenFileDialog();

               ofd.InitialDirectory = (model as ProjectFolder).CurrentDirectory;
               //ofd.Filters.Add(new FileDialogFilter() {  = "C Source Files (*.c;*.h;*.cpp;*.hpp)|*.c;*.h;*.cpp;*.hpp|All Files (*.*)|*.*";

               var result = await ofd.ShowAsync();

               if (result.Length == 1)
               {
                   (this.model as ProjectFolder).AddExistingFile(result[0]);
               }
           });

            this.ImportFolderCommand = new RoutingCommand((o) =>
           {
               //FolderBrowserDialog fbd = new FolderBrowserDialog();

               //fbd.SelectedPath = (model as ProjectFolder).CurrentDirectory;

               //if (fbd.ShowDialog () == DialogResult.OK)
               //{
               //    (model as ProjectFolder).ImportFolder (fbd.SelectedPath);
               //}
           });

            this.AddNewItemCommand = new RoutingCommand((o) =>
           {
               //Workspace.This.ModalDialog = new NewItemDialogViewModel (model as ProjectFolder);
               //Workspace.This.ModalDialog.ShowDialog ();
           });
        }

        public ICommand AddNewFolderCommand { get; private set; }
        public ICommand AddNewFileCommand { get; private set; }
        public ICommand AddNewItemCommand { get; private set; }
        public ICommand AddExistingFileCommand { get; private set; }
        public ICommand ImportFolderCommand { get; private set; }


        private ObservableCollection<ViewModelBase> children;
        public ObservableCollection<ViewModelBase> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value; OnPropertyChanged();
            }
        }
    }
}
