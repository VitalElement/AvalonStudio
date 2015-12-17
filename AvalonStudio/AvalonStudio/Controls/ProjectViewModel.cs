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

             //DebugCommand = ReactiveCommand.Create(this.WhenAnyValue(x => CanExecuteCompileTask(x)));
           // DebugCommand.Subscribe((o) =>
           //{
           //    //Workspace.Instance.DebugManager.StartDebuggingCommand.Execute(Model);            
           //});

           // CleanCommand = ReactiveCommand.Create(this.WhenAnyValue(x => CanExecuteCompileTask(x)));
           // CleanCommand.Subscribe((o) =>
           //{
           //    Clean(model);
           //});

           // BuildCommand = ReactiveCommand.Create(this.WhenAnyValue(x => CanExecuteCompileTask(x)));
           // BuildCommand.Subscribe(async (o) =>
           //{
           //    await Build(model);
           //});

            ManageReferencesCommand = ReactiveCommand.Create();
            ManageReferencesCommand.Subscribe((o) =>
            {
                //Workspace.Instance.ModalDialog = new ManageReferencesDialogViewModel(Model);
                // Workspace.Instance.ModalDialog.ShowDialog();
            });
        }

        bool CanExecuteCompileTask(object parameter)
        {
            bool result = true;

            // if (Workspace.Instance.ExecutingCompileTask || Workspace.Instance.CurrentPerspective == Perspective.Debug)
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
            //Workspace.Instance.SaveAllCommand.Execute(null);

            //if (project.SelectedConfiguration.ToolChain is CustomProcessToolChain)
            //{
            //    await project.Build(Workspace.Instance.StudioConsole, Workspace.Instance.ProcessCancellationToken);
            //}
            //else
            //{
            //    if (project.SelectedConfiguration.ToolChain != null)
            //    {
            //        if (project.SelectedConfiguration.ToolChain.Settings != null)
            //        {
            //            await project.Build(Workspace.Instance.StudioConsole, Workspace.Instance.ProcessCancellationToken);
            //        }
            //        else
            //        {
            //            Workspace.Instance.StudioConsole.WriteLine("Tool chain has not been configured. Please check VEStudio Settings.");
            //        }
            //    }
            //    else
            //    {
            //        Workspace.Instance.StudioConsole.WriteLine("No compiler has been selected. Check project settings.");
            //    }
            //}

            //Workspace.Instance.ExecutingCompileTask = false;
        }

        public void Clean(Project project)
        {
            if (project.SelectedConfiguration.ToolChain != null)
            {
                try
                {
                    project.SelectedConfiguration.ToolChain.Clean(Workspace.Instance.Console, this.Model as Project, Workspace.Instance.ProcessCancellationToken);
                }
                catch (Exception e)
                {
                    Workspace.Instance.Console.WriteLine(e.Message);
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
        
        new public Project Model
        {
            get { return base.Model as Project; }            
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
