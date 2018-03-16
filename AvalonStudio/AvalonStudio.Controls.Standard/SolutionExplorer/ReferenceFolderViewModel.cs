using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;
using System.Collections.ObjectModel;
using Avalonia.Media;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class ReferenceFolderViewModel : ProjectItemViewModel
    {
        private ObservableCollection<ReferenceViewModel> references;

        public ReferenceFolderViewModel(IReferenceFolder folder)
        {
            references = new ObservableCollection<ReferenceViewModel>();
            references.BindCollections(folder.References, p => { return new ReferenceViewModel(p); }, (rvm, p) => rvm.Model == p);
        }

        public override string Title
        {
            get { return "References"; }
            set { }
        }

        public ObservableCollection<ReferenceViewModel> References
        {
            get { return references; }
            set { this.RaiseAndSetIfChanged(ref references, value); }
        }

        public override DrawingGroup Icon => "ReferenceIcon".GetIcon();
    }
}