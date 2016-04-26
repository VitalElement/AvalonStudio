namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.MVVM;
    using Extensibility;
    using Projects;
    using ReactiveUI;
    using System.Collections.ObjectModel;
    using System;
    using Shell;
    public class SolutionExplorerViewModel : ToolViewModel
    {
        private IShell shell;

        public SolutionExplorerViewModel()
        {            
            Title = "Solution Explorer";
            solution = new ObservableCollection<SolutionViewModel>();            
        }

        public override void Activate()
        {
            shell = IoC.Get<IShell>();

            shell.SolutionChanged += (sender, e) =>
            {
                Model = shell.CurrentSolution;
            };
        }
        
        new private ISolution model = null;
        public ISolution Model
        {
            get { return model; }
            set
            {
                Projects = new ObservableCollection<ProjectItemViewModel>();
                model = value;

                if (this.Model != null)
                {
                    if (Model.Projects.Count > 0)
                    {
                        SelectedProject = this.Model.StartupProject;
                    }
                }

                var sol = new ObservableCollection<SolutionViewModel>();
                sol.Add(new SolutionViewModel(model));

                Solution = sol;

                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(Projects));
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

            set { selectedProject = value; this.RaisePropertyChanged(); }
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

        public override Location DefaultLocation
        {
            get
            {
                return Location.Right;
            }
        }

        public const string ToolId = "CIDSEVM00";
    }
}
