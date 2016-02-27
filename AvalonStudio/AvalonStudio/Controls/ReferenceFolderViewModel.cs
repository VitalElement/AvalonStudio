namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.MVVM;
    using Projects;
    using ReactiveUI;
    using System.Collections.ObjectModel;

    public class ReferenceFolderViewModel : ProjectItemViewModel
    {
        public ReferenceFolderViewModel (IProject project)
        {
            references = new ObservableCollection<ReferenceViewModel>();
            references.BindCollections<ReferenceViewModel, IProject>(project.References, (p) => { return new ReferenceViewModel(p); }, (rvm, p) => rvm.Model == p);
        }
       
        public string Title
        {
            get { return "References"; }
        }


        private ObservableCollection<ReferenceViewModel> references;
        public ObservableCollection<ReferenceViewModel> References
        {
            get { return references; }
            set { this.RaiseAndSetIfChanged(ref references, value); }
        }

    }
}
