using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class SolutionViewModel : SolutionParentViewModel<ISolution>
    {
        public SolutionViewModel(ISolution model) : base(null, model)
        {
            Parent = this;

            OpenInExplorerCommand = ReactiveCommand.Create(() => { Platform.OpenFolderInExplorer(model.CurrentDirectory); });

            BuildSolutionCommand = ReactiveCommand.Create(BuildSolution);

            CleanSolutionCommand = ReactiveCommand.Create(CleanSolution);

            RebuildSolutionCommand = ReactiveCommand.Create(() =>
            {
                CleanSolution();

                BuildSolution();
            });

            RunAllTestsCommand = ReactiveCommand.Create(() => RunTests());

            SaveCommand = ReactiveCommand.CreateFromObservable(SaveSolution);

            IsExpanded = true;
        }

        public ReactiveCommand<Unit, Unit> CleanSolutionCommand { get; }
        public ReactiveCommand<Unit, Unit> BuildSolutionCommand { get; }
        public ReactiveCommand<Unit, Unit> RebuildSolutionCommand { get; }
        public ReactiveCommand<Unit, Unit> RunAllTestsCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenInExplorerCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        private void CleanSolution()
        {
        }

        private void BuildSolution()
        {
        }

        private void RunTests()
        {
        }

        private IObservable<Unit> SaveSolution()
        {
            return Observable.Start(() =>
            {
                string location = Platform.ProjectDirectory;

                var ofd = new SaveFileDialog
                {
                    Directory = location,
                    InitialFileName = Model.Name,
                    DefaultExtension = Path.GetExtension(Model.Name)
                };

                var mainWindow = AvaloniaLocator.CurrentMutable.GetService<Window>();

                var result = ofd.ShowAsync(mainWindow).GetAwaiter().GetResult();

                if (!string.IsNullOrEmpty(result))
                {
                    Model.Save(result);
                }
            });
        }

        public override string Title
        {
            get
            {
                if (Model != null)
                {
                    if (InEditMode)
                    {
                        return Model.Name;
                    }
                    else
                    {
                        return string.Format("Solution '{0}' ({1} {2})", Model.Name, Model.Solution.Projects.Count(), StringProjects);
                    }
                }

                return string.Empty;
            }

            set
            {
                if(InEditMode && Model.CanRename && value != Model.Name && !string.IsNullOrEmpty(value))
                {
                    Model.Name = value;
                }
            }
        }

        private string StringProjects
        {
            get
            {
                if (Model.Solution.Projects.Count() == 1)
                {
                    return "project";
                }

                return "projects";
            }
        }

        public override DrawingGroup Icon => null;

        public override bool InEditMode
        {
            get => base.InEditMode;
            set
            {
                base.InEditMode = value;
                this.RaisePropertyChanged(nameof(Title));
            }
        }
    }
}