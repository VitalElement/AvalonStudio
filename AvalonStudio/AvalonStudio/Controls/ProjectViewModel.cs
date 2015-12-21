namespace AvalonStudio.Controls.ViewModels
{
    using MVVM;
    using Perspex.Media;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;

    public abstract class ProjectViewModel : ViewModel<IProject>
    {
        public ProjectViewModel(IProject model)
            : base(model)
        {
            references = new ObservableCollection<ReferenceViewModel>();

            foreach(var reference in model.References)
            {
                references.Add(new ReferenceViewModel(reference));
            }
        }             

        public string Title
        {
            get
            {
                return Model.Name;
            }
        }

        private ObservableCollection<ReferenceViewModel> references;
        public ObservableCollection<ReferenceViewModel> References
        {
            get { return references; }
            set { this.RaiseAndSetIfChanged(ref references, value); }
        }

        public ReactiveCommand<object> BuildCommand { get; protected set; }
        public ReactiveCommand<object> CleanCommand { get; protected set; }
        public ReactiveCommand<object> DebugCommand { get; protected set; }
        public ReactiveCommand<object> ManageReferencesCommand { get; protected set; }
        public ReactiveCommand<object> RemoveCommand { get; protected set; }


        public static ProjectViewModel Create(IProject model)
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

        public FontWeight FontWeight
        {
            get
            {
                if (Model == Model.Solution.StartupProject)
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
    }
}
