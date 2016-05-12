namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.MVVM;
    using Projects;
    using ReactiveUI;
    using System.Collections.ObjectModel;

    public class ReferenceFolderViewModel : ProjectItemViewModel
    {
        public ReferenceFolderViewModel (IReferenceFolder folder)
        {
            references = new ObservableCollection<ReferenceViewModel>();
            references.BindCollections(folder.References, (p) => { return new ReferenceViewModel(p); }, (rvm, p) => rvm.Model == p);
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
