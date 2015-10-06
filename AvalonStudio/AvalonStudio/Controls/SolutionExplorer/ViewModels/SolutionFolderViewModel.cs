namespace AvalonStudio.Controls.ViewModels
{
    using Microsoft.Win32;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using AvalonStudio.Models;
    using AvalonStudio.Models.Solutions;
    using AvalonStudio.MVVM;
    using AvalonStudio.Utils;
    using Perspex.Controls;
    using ReactiveUI;

    public class SolutionFolderViewModel : SolutionParentViewModel<SolutionFolder>
    {
        public SolutionFolderViewModel(SolutionFolder folder) : base(folder)
        {

        }

        public static SolutionFolderViewModel Create(SolutionFolder folder)
        {
            var result = new SolutionFolderViewModel(folder);

            return result;
        }

        public override bool CanAcceptDrop(Type type)
        {
            bool result = false;

            if (type == typeof(StandardProjectViewModel))
            {
                result = true;
            }

            //if (type == typeof(TestProjectViewModel))
            //{
            //    result = true;
            //}

            if (type == typeof(SolutionFolderViewModel))
            {
                result = true;
            }

            return result;
        }

        public override void Drop(ProjectItemViewModel item)
        {
            var project = item as ProjectViewModel;

            if (project != null)
            {
                var parent = project.Model.Solution.GetParent(project.Model);

                parent.RemoveItem(project.Model);

                Model.AttachItem(project.Model);
            }
            else if (item is SolutionFolderViewModel)
            {
                var folder = item as SolutionFolderViewModel;

                var parent = folder.Model.Solution.GetParent(folder.Model);

                parent.DetachItem(folder.Model);

                Model.AttachItem(folder.Model);
            }
        }
    }

    public abstract class SolutionParentViewModel<T> : ProjectItemViewModel where T : SolutionFolder
    {
        public SolutionParentViewModel(T model)
            : base(model)
        {
            Children = new ObservableCollection<ReactiveObject>();
            //Children.Bind(Model.Children, (p) => ReactiveObjectExtensions.Create(p), (vm, m) => vm.BaseModel == m);

            AddNewFolderCommand = ReactiveCommand.Create();
            AddNewFolderCommand.Subscribe((args) =>
            {
                //Workspace.This.ModalDialog = new NewSolutionFolderViewModel(this.model as SolutionFolder);
                //Workspace.This.ModalDialog.ShowDialog();
            });

            AddNewProjectCommand = ReactiveCommand.Create();
            AddNewProjectCommand.Subscribe((o) =>
            {
                //Workspace.This.ModalDialog = new NewProjectDialogViewModel(Workspace.This, Model, false);
                //Workspace.This.ModalDialog.ShowDialog();
            });

            AddExistingProjectCommand = ReactiveCommand.Create();
            AddExistingProjectCommand.Subscribe(async (o) =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = model.Solution.CurrentDirectory;

                // ofd.Filter = "VEStudio Project Files (*" + VEStudioService.ProjectExtension + ")|*" + VEStudioService.ProjectExtension;

                var result = await ofd.ShowAsync();

                if (result.Length == 1)
                {
                    var unloadedProject = new UnloadedProject(Model, Model.Solution.CurrentDirectory.MakeRelativePath(result[0]));
                    var project = Project.LoadProject(Model.Solution, unloadedProject);

                    if (!Model.Solution.ContainsProject(project))
                    {
                        model.AddProject(project, unloadedProject);
                    }
                }
            });


            RemoveCommand = ReactiveCommand.Create();
            RemoveCommand.Subscribe((o) =>
            {
                model.Solution.GetParent(Model).RemoveItem(Model);
            });
        }

        public bool VisitAllChildren(Func<ProjectItemViewModel, bool> func)
        {
            if (func(this))
            {
                return true;
            }

            foreach (ProjectItemViewModel item in this.Children)
            {
                if (item is SolutionFolderViewModel)
                {
                    if ((item as SolutionFolderViewModel).VisitAllChildren(func))
                    {
                        return true;
                    }

                    if (func(item))
                    {
                        return true;
                    }
                }
                else if (item is ProjectViewModel)
                {
                    if (func(item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public ReactiveCommand<object> AddNewFolderCommand { get; private set; }
        public ReactiveCommand<object> AddNewProjectCommand { get; private set; }
        public ReactiveCommand<object> AddExistingProjectCommand { get; private set; }
        public ReactiveCommand<object> RemoveCommand { get; private set; }

        private SolutionFolder model;
        public SolutionFolder Model
        {
            get { return model; }
            set { model = value; }
        }


        private ObservableCollection<ReactiveObject> children;
        public ObservableCollection<ReactiveObject> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value; this.RaisePropertyChanged();
            }
        }
    }
}
