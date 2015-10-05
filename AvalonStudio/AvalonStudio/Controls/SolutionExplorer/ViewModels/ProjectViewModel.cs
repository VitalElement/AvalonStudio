namespace AvalonStudio.Controls.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using AvalonStudio.Models.Solutions;
    using AvalonStudio.Models.Tools.Compiler;
    using AvalonStudio.MVVM;
    using Perspex.MVVM;
    using Perspex.Media;

    public abstract class ProjectViewModel : ProjectParentViewModel<Project>
    {
        public ProjectViewModel(Project model)
            : base(model)
        {
            if (model is HiddenProject)
            {
                visibility = false;
            }
            else
            {
                visibility = true;
            }

            this.IncludePaths = new ObservableCollection<string>();
            // this.IncludePaths.Bind (model.IncludePaths, (p) => p, (s, so) => (s == so));

            RemoveCommand = new RoutingCommand((o) =>
            {
                model.Parent.RemoveItem(model);
            });

            DebugCommand = new RoutingCommand((o) =>
           {
               //Workspace.This.DebugManager.StartDebuggingCommand.Execute(Model);            
           }, CanExecuteCompileTask);

            CleanCommand = new RoutingCommand((o) =>
           {
               Clean(model);
           }, CanExecuteCompileTask);

            BuildCommand = new RoutingCommand(async (o) =>
           {
               await Build(model);
           }, CanExecuteCompileTask);

            ManageReferencesCommand = new RoutingCommand((o) =>
            {
                //Workspace.This.ModalDialog = new ManageReferencesDialogViewModel(Model);
                // Workspace.This.ModalDialog.ShowDialog();
            });
        }

        bool CanExecuteCompileTask(object parameter)
        {
            bool result = true;

            // if (Workspace.This.ExecutingCompileTask || Workspace.This.CurrentPerspective == Perspective.Debug)
            /// {
            //     result = false;
            // }

            return result;
        }


        protected void InvalidateFontWeight()
        {
            OnPropertyChanged(() => FontWeight);
        }

        public async Task Build(Project project)
        {
            //Workspace.This.SaveAllCommand.Execute(null);

            //if (project.SelectedConfiguration.ToolChain is CustomProcessToolChain)
            //{
            //    await project.Build(Workspace.This.StudioConsole, Workspace.This.ProcessCancellationToken);
            //}
            //else
            //{
            //    if (project.SelectedConfiguration.ToolChain != null)
            //    {
            //        if (project.SelectedConfiguration.ToolChain.Settings != null)
            //        {
            //            await project.Build(Workspace.This.StudioConsole, Workspace.This.ProcessCancellationToken);
            //        }
            //        else
            //        {
            //            Workspace.This.StudioConsole.WriteLine("Tool chain has not been configured. Please check VEStudio Settings.");
            //        }
            //    }
            //    else
            //    {
            //        Workspace.This.StudioConsole.WriteLine("No compiler has been selected. Check project settings.");
            //    }
            //}

            //Workspace.This.ExecutingCompileTask = false;
        }

        public void Clean(Project project)
        {
            if (project.SelectedConfiguration.ToolChain != null)
            {
                try
                {
                    project.SelectedConfiguration.ToolChain.Clean(Workspace.This.Console, this.model as Project, Workspace.This.ProcessCancellationToken);
                }
                catch (Exception e)
                {
                    Workspace.This.Console.WriteLine(e.Message);
                }
            }
        }

        public ICommand BuildCommand { get; protected set; }
        public ICommand CleanCommand { get; protected set; }
        public ICommand DebugCommand { get; protected set; }

        public ICommand ManageReferencesCommand { get; protected set; }
        public ICommand RemoveCommand { get; protected set; }


        public static ProjectViewModel Create(Project model)
        {
            ProjectViewModel result = new StandardProjectViewModel(model);

            return result;
        }

        private bool visibility;
        public bool Visibility
        {
            get { return visibility; }
            set { visibility = value; OnPropertyChanged(); }
        }


        new public Project Model
        {
            get
            {
                return BaseModel as Project;
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                if (Model == Model.Solution.DefaultProject)
                {
                    return FontWeight.Bold;
                }
                else
                {
                    return FontWeight.Normal;
                }
            }
        }

        public ObservableCollection<string> IncludePaths { get; private set; }

        public override bool CanAcceptDrop(Type type)
        {
            bool result = false;

            if (type == typeof(ProjectFileViewModel))
            {
                result = true;
            }

            return result;
        }

        public override void Drop(ProjectItemViewModel item)
        {
            if (item is ProjectFileViewModel)
            {
                var file = item as ProjectFileViewModel;

                file.Model.MoveTo(this.Model);
            }
        }
    }
}
