namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.Models.Solutions;
    using MVVM;
    using Perspex.Media;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

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

            RemoveCommand = ReactiveCommand.Create();
            RemoveCommand.Subscribe((o) =>
            {
                model.Parent.RemoveItem(model);
            });

            DebugCommand = ReactiveCommand.Create(this.WhenAnyValue(x => CanExecuteCompileTask(x)));
            DebugCommand.Subscribe((o) =>
           {
               //Workspace.This.DebugManager.StartDebuggingCommand.Execute(Model);            
           });

            CleanCommand = ReactiveCommand.Create(this.WhenAnyValue(x => CanExecuteCompileTask(x)));
            CleanCommand.Subscribe((o) =>
           {
               Clean(model);
           });

            BuildCommand = ReactiveCommand.Create(this.WhenAnyValue(x => CanExecuteCompileTask(x)));
            BuildCommand.Subscribe(async (o) =>
           {
               await Build(model);
           });

            ManageReferencesCommand = ReactiveCommand.Create();
            ManageReferencesCommand.Subscribe((o) =>
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
            this.RaisePropertyChanged(() => FontWeight);
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

        public ReactiveCommand<object> BuildCommand { get; protected set; }
        public ReactiveCommand<object> CleanCommand { get; protected set; }
        public ReactiveCommand<object> DebugCommand { get; protected set; }

        public ReactiveCommand<object> ManageReferencesCommand { get; protected set; }
        public ReactiveCommand<object> RemoveCommand { get; protected set; }


        public static ProjectViewModel Create(Project model)
        {
            ProjectViewModel result = new StandardProjectViewModel(model);

            return result;
        }

        private bool visibility;
        public bool Visibility
        {
            get { return visibility; }
            set { visibility = value; this.RaisePropertyChanged(); }
        }

        private Project model;
        public Project Model
        {
            get { return model; }
            set { model = value; }
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
