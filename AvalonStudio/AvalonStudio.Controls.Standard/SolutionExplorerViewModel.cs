namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.MVVM;
    using Extensibility;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class SolutionExplorerViewModel : ToolViewModel
    {
        private IShell shell;

        public SolutionExplorerViewModel ()
        {                                    
                        
        }                           

        new private ISolution model = null;
        public ISolution Model
        {
            get { return model; }
            set
            {                
                Projects = new ObservableCollection<ProjectItemViewModel> ();
                model = value;

                if (this.Model != null)
                {
                    //this.Projects.Bind (this.Model.Children,
                    //    (p) => { return ProjectItemViewModel.Create(p); },
                    //    ((pvm, p) => pvm.BaseModel == p));

                    if (Model.Projects.Count > 0)
                    {
                        SelectedProject = this.Model.StartupProject;
                    }
                }

                var sol = new ObservableCollection<SolutionViewModel>();
                sol.Add(new SolutionViewModel(model));

                Solution = sol;

                this.RaisePropertyChanged ();
                this.RaisePropertyChanged (nameof(Projects));                
            }
        }

        private ObservableCollection<SolutionViewModel> solution;
        public ObservableCollection<SolutionViewModel> Solution
        {
            get { return solution; }
            set { this.RaiseAndSetIfChanged(ref solution, value); }
        }


        private IProject selectedProject;
        public IProject SelectedProject
        {
            get
            {
                return selectedProject;
            }

            set { selectedProject = value; this.RaisePropertyChanged (); }
        }
        
        private ProjectItemViewModel selectedItem;
        public ProjectItemViewModel SelectedItem 
        {
            get { return selectedItem; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);

                if (value is SourceFileViewModel)
                {
                    // might need wait here?
                    shell.OpenDocument(((ISourceFile)(value as SourceFileViewModel).Model), 1);
                }
            }
        }

        public ObservableCollection<ProjectItemViewModel> Projects { get; set; }

        public const string ToolId = "CIDSEVM00";        
    }
}
