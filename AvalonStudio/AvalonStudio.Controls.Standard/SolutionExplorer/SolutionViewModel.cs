using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class SolutionViewModel : ViewModel<ISolution>
    {
        private ObservableCollection<SolutionItemViewModel> _items;
        private readonly IShell shell;

        public SolutionViewModel(ISolution model) : base(model)
        {
            shell = IoC.Get<IShell>();

            Items = new ObservableCollection<SolutionItemViewModel>();
            IsExpanded = true;

            Items.BindCollections(model.Items, p => { return SolutionItemViewModel.Create(this, p); },
                (pvm, p) => pvm.Model == p);

            AddNewFolderCommand = ReactiveCommand.Create(() =>
            {
                Model.AddFolder(SolutionFolder.Create("New Folder"));

                Model.Save();
            });

            NewProjectCommand = ReactiveCommand.Create(() =>
            {
                shell.ModalDialog = new NewProjectDialogViewModel(model);
                shell.ModalDialog.ShowDialog();
            });

            AddExistingProjectCommand = ReactiveCommand.Create(async () =>
            {
                var dlg = new OpenFileDialog();
                dlg.Title = "Open Project";

                var extensions = new List<string>();

                foreach (var projectType in shell.ProjectTypes)
                {
                    extensions.AddRange(projectType.Extensions);
                }

                dlg.Filters.Add(new FileDialogFilter { Name = "AvalonStudio Project", Extensions = extensions });

                if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
                {
                    dlg.InitialDirectory = Model.CurrentDirectory;
                }
                else
                {
                    dlg.InitialFileName = Model.CurrentDirectory;
                }

                dlg.AllowMultiple = false;

                var result = await dlg.ShowAsync();

                if (result != null && !string.IsNullOrEmpty(result.FirstOrDefault()))
                {
                    var proj = AvalonStudioSolution.LoadProjectFile(model, result[0]);

                    if (proj != null)
                    {
                        model.AddProject(proj);
                        model.Save();
                    }
                }
            });

            OpenInExplorerCommand = ReactiveCommand.Create(() => { Platform.OpenFolderInExplorer(model.CurrentDirectory); });

            ConfigurationCommand = ReactiveCommand.Create(() =>
            {
                //Workspace.Instance.ModalDialog = new SolutionConfigurationDialogViewModel(Workspace.Instance.SolutionExplorer.Model);
                //Workspace.Instance.ModalDialog.ShowDialog();
            });

            BuildSolutionCommand = ReactiveCommand.Create(() => BuildSolution());

            CleanSolutionCommand = ReactiveCommand.Create(() => CleanSolution());

            RebuildSolutionCommand = ReactiveCommand.Create(() =>
            {
                CleanSolution();

                BuildSolution();
            });

            RunAllTestsCommand = ReactiveCommand.Create(() => RunTests());
        }

        public bool IsExpanded { get; set; }

        public ObservableCollection<SolutionItemViewModel> Items
        {
            get { return _items; }
            set { this.RaiseAndSetIfChanged(ref _items, value); }
        }

        public ReactiveCommand ConfigurationCommand { get; }
        public ReactiveCommand CleanSolutionCommand { get; }
        public ReactiveCommand BuildSolutionCommand { get; }
        public ReactiveCommand RebuildSolutionCommand { get; }
        public ReactiveCommand RunAllTestsCommand { get; }
        public ReactiveCommand NewProjectCommand { get; }
        public ReactiveCommand AddExistingProjectCommand { get; }
        public ReactiveCommand OpenInExplorerCommand { get; }

        public ReactiveCommand AddNewFolderCommand { get; private set; }

        private void CleanSolution()
        {
        }

        private void BuildSolution()
        {
        }

        private void RunTests()
        {
        }

        public string Title
        {
            get
            {
                if (Model != null)
                {
                    return string.Format("Solution '{0}' ({1} {2})", Model.Name, Model.Projects.Count(), StringProjects);
                }

                return string.Empty;
            }
        }

        private string StringProjects
        {
            get
            {
                if (Model.Projects.Count() == 1)
                {
                    return "project";
                }

                return "projects";
            }
        }
    }
}