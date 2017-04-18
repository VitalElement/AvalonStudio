using ReactiveUI;
using System;

namespace AvalonStudio.Controls
{
    public class MainMenuViewModel : ReactiveObject
    {
        public MainMenuViewModel()
        {
            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ => { ShellViewModel.Instance.Save(); });

            SaveAllCommand = ReactiveCommand.Create();
            SaveAllCommand.Subscribe(_ => { ShellViewModel.Instance.SaveAll(); });

            CleanProjectCommand = ReactiveCommand.Create();
            CleanProjectCommand.Subscribe(_ => { ShellViewModel.Instance.Clean(); });

            BuildProjectCommand = ReactiveCommand.Create();
            BuildProjectCommand.Subscribe(_ => { ShellViewModel.Instance.Build(); });

            PackagesCommand = ReactiveCommand.Create();
            PackagesCommand.Subscribe(o => { ShellViewModel.Instance.ShowPackagesDialog(); });

            ProjectPropertiesCommand = ReactiveCommand.Create();
            ProjectPropertiesCommand.Subscribe(o => { ShellViewModel.Instance.ShowProjectPropertiesDialog(); });

            NewProjectCommand = ReactiveCommand.Create();
            NewProjectCommand.Subscribe(o =>
            {
                // ShellViewModel.Instance.ShowNewProjectDialog();
            });

            ExitCommand = ReactiveCommand.Create();
            ExitCommand.Subscribe(o => { ShellViewModel.Instance.ExitApplication(); });
        }

        public ReactiveCommand<object> NewProjectCommand { get; }
        public ReactiveCommand<object> SaveCommand { get; }
        public ReactiveCommand<object> SaveAllCommand { get; }
        public ReactiveCommand<object> ExitCommand { get; }

        public ReactiveCommand<object> CleanProjectCommand { get; }
        public ReactiveCommand<object> BuildProjectCommand { get; }
        public ReactiveCommand<object> PackagesCommand { get; }
        public ReactiveCommand<object> ProjectPropertiesCommand { get; }
    }
}