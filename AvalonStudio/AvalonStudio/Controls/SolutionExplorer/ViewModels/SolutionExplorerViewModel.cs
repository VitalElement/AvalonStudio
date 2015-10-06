namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.Models.Solutions;
    using AvalonStudio.MVVM;
    using Perspex.MVVM;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class SolutionExplorerViewModel : ToolViewModel
    {
        public SolutionExplorerViewModel () : base (SolutionExplorerViewModel.ToolId, "Solution Explorer")
        {                                    
                        
        }                           

        new private Solution model = null;
        public Solution Model
        {
            get { return model; }
            set
            {
                this.ContentId = ToolId + value.OpenedLocation;
                Projects = new ObservableCollection<ProjectItemViewModel> ();
                model = value;

                if (this.Model != null)
                {
                    this.Projects.Bind (this.Model.Children,
                        (p) => { return ProjectItemViewModel.Create(p); },
                        ((pvm, p) => pvm.BaseModel == p));

                    if (Model.LoadedProjects.Count > 0)
                    {
                        SelectedProject = this.Model.LoadedProjects.FirstOrDefault();
                    }
                }

                var sol = new ObservableCollection<SolutionViewModel>();
                sol.Add(new SolutionViewModel(model));

                Solution = sol;

                OnPropertyChanged ();
                OnPropertyChanged (() => Projects);                
            }
        }

        private ObservableCollection<SolutionViewModel> solution;
        public ObservableCollection<SolutionViewModel> Solution
        {
            get { return solution; }
            set { solution = value; OnPropertyChanged(); }
        }


        private Project selectedProject;
        public Project SelectedProject
        {
            get
            {
                return selectedProject;
            }

            set { selectedProject = value; OnPropertyChanged (); }
        }


        public void CreateNewSolution (string location)
        {
            var solution = new Solution();


            solution.OpenedLocation = location;
        }

        private ViewModelBase selectedItem;
        public ViewModelBase SelectedItem 
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    if (selectedItem is ProjectFileViewModel)
                    {
                        (selectedItem as ProjectFileViewModel).IsEditingTitle = false;
                        (selectedItem as ProjectFileViewModel).NumberOfSelections = 0;
                    }

                    if (selectedItem is ProjectFolderViewModel)
                    {
                        (selectedItem as ProjectFolderViewModel).IsEditingTitle = false;
                        (selectedItem as ProjectFolderViewModel).NumberOfSelections = 0;
                    }

                    if (selectedItem is ProjectViewModel)
                    {
                        (selectedItem as ProjectViewModel).IsEditingTitle = false;
                        (selectedItem as ProjectViewModel).NumberOfSelections = 0;
                    }

                    selectedItem = value;

                    if (selectedItem is ProjectFileViewModel)
                    {
                        SelectedProject = (value as ProjectFileViewModel).Model.Project;
                    }

                    if (selectedItem is ProjectFolderViewModel)
                    {
                        SelectedProject = (value as ProjectFolderViewModel).Model.Project;
                    }

                    if (selectedItem is ProjectViewModel)
                    {
                        SelectedProject = (value as ProjectViewModel).Model.Project;
                    }

                    OnPropertyChanged();

                    if (this.SelectedItemChanged != null)
                    {
                        this.SelectedItemChanged(this, value);
                    }
                }
                else
                {
                    if (selectedItem is ProjectFileViewModel)
                    {                        
                        (selectedItem as ProjectFileViewModel).NumberOfSelections++;
                    }

                    if (selectedItem is ProjectFolderViewModel)
                    {
                        (selectedItem as ProjectFolderViewModel).NumberOfSelections++;
                    }

                    if (selectedItem is ProjectViewModel)
                    {
                        (selectedItem as ProjectViewModel).NumberOfSelections++;
                    }
                }                
            }
        }

        public event EventHandler<ViewModelBase> SelectedItemChanged;

        public ObservableCollection<ProjectItemViewModel> Projects { get; set; }

        public const string ToolId = "CIDSEVM00";        
    }
}
