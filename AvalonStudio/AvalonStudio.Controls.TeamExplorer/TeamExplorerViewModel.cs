using System.Collections.ObjectModel;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using LibGit2Sharp;
using ReactiveUI;

namespace AvalonStudio.Controls.TeamExplorer
{
	public class TeamExplorerViewModel : ToolViewModel, IExtension
	{
		private ObservableCollection<string> branches;

		private object currentView;

		private RepositoryViewModel repository;
		private IShell shell;

		public TeamExplorerViewModel()
		{
			Title = "Team Explorer";
			branches = new ObservableCollection<string>();
		}

		public ObservableCollection<string> Branches
		{
			get { return branches; }
			set { this.RaiseAndSetIfChanged(ref branches, value); }
		}

		public RepositoryViewModel Repository
		{
			get { return repository; }
			set { this.RaiseAndSetIfChanged(ref repository, value); }
		}

		public object CurrentView
		{
			get { return currentView; }
			set { this.RaiseAndSetIfChanged(ref currentView, value); }
		}


		public override Location DefaultLocation
		{
			get { return Location.Right; }
		}

		public void Activation()
		{
			shell = IoC.Get<IShell>();

			shell.SolutionChanged += (sender, e) =>
			{
				var solution = shell.CurrentSolution;

				if (solution != null)
				{
					var gitPath = LibGit2Sharp.Repository.Discover(solution.CurrentDirectory);

					if (!string.IsNullOrEmpty(gitPath))
					{
						Repository = new RepositoryViewModel(new Repository(gitPath));
						CurrentView = Repository;
					}
				}
			};
		}

		public void BeforeActivation()
		{
		}
	}
}