using AvalonStudio.MVVM;
using AvalonStudio.Projects;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	public class ReferenceViewModel : ViewModel<IProject>
	{
		public ReferenceViewModel(IProject model) : base(model)
		{
		}

		public string Name
		{
			get { return Model.Name; }
		}
	}
}