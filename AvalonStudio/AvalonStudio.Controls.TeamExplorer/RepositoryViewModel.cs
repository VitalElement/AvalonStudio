using System.Collections.ObjectModel;
using System.IO;
using AvalonStudio.MVVM;
using LibGit2Sharp;
using ReactiveUI;

namespace AvalonStudio.Controls.TeamExplorer
{
	public class RepositoryViewModel : ViewModel<Repository>
	{
		private ObservableCollection<string> branches;

		private string name;

		private string selectedBranch;

		public RepositoryViewModel(Repository model) : base(model)
		{
			name = Path.GetFileName(model.Info.Path.Replace("\\.git\\", string.Empty));

			branches = new ObservableCollection<string>();
			foreach (var branch in model.Branches)
			{
				branches.Add(branch.FriendlyName);

				if (branch.IsCurrentRepositoryHead)
				{
					selectedBranch = branch.FriendlyName;
				}
			}
		}

		public string Name
		{
			get { return name; }
			set { this.RaiseAndSetIfChanged(ref name, value); }
		}

		public ObservableCollection<string> Branches
		{
			get { return branches; }
			set { this.RaiseAndSetIfChanged(ref branches, value); }
		}

		public string SelectedBranch
		{
			get { return selectedBranch; }
			set { this.RaiseAndSetIfChanged(ref selectedBranch, value); }
		}
	}
}