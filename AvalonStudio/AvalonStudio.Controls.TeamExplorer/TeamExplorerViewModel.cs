namespace AvalonStudio.Controls.TeamExplorer
{
    using System;
    using AvalonStudio.Extensibility.Plugin;
    using AvalonStudio.MVVM;
    using Shell;
    using Extensibility;
    using ReactiveUI;
    using System.Collections.ObjectModel;
    using LibGit2Sharp;

    public class TeamExplorerViewModel : ToolViewModel, AvalonStudio.Extensibility.Plugin.IPlugin
    {
        private IShell shell;

        public TeamExplorerViewModel()
        {
            Title = "Team Explorer";
            branches = new ObservableCollection<string>();
                      
        }

        private ObservableCollection<string> branches;
        public ObservableCollection<string> Branches
        {
            get { return branches; }
            set { this.RaiseAndSetIfChanged(ref branches, value); }
        }

        private RepositoryViewModel repository;
        public RepositoryViewModel Repository
        {
            get { return repository; }
            set { this.RaiseAndSetIfChanged(ref repository, value); }
        }

        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set { this.RaiseAndSetIfChanged(ref currentView, value); }
        }


        public override Location DefaultLocation
        {
            get
            {
                return Location.Right;
            }
        }

        public override void Activate()
        {
            
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

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public System.Version Version
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
