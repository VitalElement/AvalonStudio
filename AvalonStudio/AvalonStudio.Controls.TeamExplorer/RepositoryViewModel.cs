namespace AvalonStudio.Controls.TeamExplorer
{
    using LibGit2Sharp;
    using MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RepositoryViewModel : ViewModel<Repository>
    {
        public RepositoryViewModel(Repository model) : base(model)
        {
            name = Path.GetFileName(model.Info.Path.Replace("\\.git\\",string.Empty));

            branches = new ObservableCollection<string>();
            foreach(var branch in model.Branches)
            {
                branches.Add(branch.FriendlyName);

                if(branch.IsCurrentRepositoryHead)
                {
                    selectedBranch = branch.FriendlyName;
                }
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }

        private ObservableCollection<string> branches;
        public ObservableCollection<string> Branches
        {
            get { return branches; }
            set { this.RaiseAndSetIfChanged(ref branches, value); }
        }

        private string selectedBranch;
        public string SelectedBranch
        {
            get { return selectedBranch; }
            set { this.RaiseAndSetIfChanged(ref selectedBranch, value); }
        }
    }
}
