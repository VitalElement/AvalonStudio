using System.Collections.ObjectModel;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;

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

		public string Title
		{
			get { return "References"; }
		}

		public ObservableCollection<ReferenceViewModel> References
		{
			get { return references; }
			set { this.RaiseAndSetIfChanged(ref references, value); }
		}
	}
}