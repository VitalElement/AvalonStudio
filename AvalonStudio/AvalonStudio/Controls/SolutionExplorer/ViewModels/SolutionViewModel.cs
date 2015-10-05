namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.Models.Solutions;
    using Perspex.MVVM;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;

    public class SolutionViewModel : SolutionParentViewModel<Solution>
    {
        public SolutionViewModel(Solution model) : base(model)
        {
            IsExpanded = true;            

            OpenInExplorerCommand = new RoutingCommand((o) =>
            {
                Process.Start(model.CurrentDirectory);
            });

            ConfigurationCommand = new RoutingCommand((o) =>
            {
                //Workspace.This.ModalDialog = new SolutionConfigurationDialogViewModel(Workspace.This.SolutionExplorer.Model);
                //Workspace.This.ModalDialog.ShowDialog();
            });

            BuildSolutionCommand = new RoutingCommand((o) =>
            {
                BuildSolution();
            });

            CleanSolutionCommand = new RoutingCommand((o) =>
            {
                CleanSolution();
            });

            RebuildSolutionCommand = new RoutingCommand((o) =>
            {
                CleanSolution();

                BuildSolution();
            });

            RunAllTestsCommand = new RoutingCommand((o) =>
            {
                RunTests();
            });
        }

        private async void CleanSolution()
        {
            // await Model.Solution.DefaultProject.SelectedConfiguration.ToolChain.Clean(Workspace.This.StudioConsole, Model.Solution.DefaultProject, Workspace.This.ProcessCancellationToken);
        }

        private async void BuildSolution()
        {
            //Workspace.This.SaveAllCommand.Execute(null);
            //await Model.Solution.DefaultProject.SelectedConfiguration.ToolChain.Build(Workspace.This.StudioConsole, this.Model.Solution.DefaultProject, Workspace.This.ProcessCancellationToken);
        }

        private async void RunTests()
        {
            //var testProjects = Model.Solution.LoadedProjects.OfType<CatchTestProject>();
            //var tests = new List<Test>();

            //foreach(var testProject in testProjects)
            //{
            //    await testProject.Build(Workspace.This.StudioConsole, Workspace.This.ProcessCancellationToken);

            //    tests.AddRange(testProject.EnumerateTests(Workspace.This.StudioConsole));            
            //}

            //Workspace.This.TestRunner.AddTests(tests);
            //Workspace.This.TestRunner.RunTests();

        }

        public ICommand ConfigurationCommand { get; private set; }
        public ICommand CleanSolutionCommand { get; private set; }
        public ICommand BuildSolutionCommand { get; private set; }
        public ICommand RebuildSolutionCommand { get; private set; }
        public ICommand RunAllTestsCommand { get; private set; }

        new public string Title
        {
            get
            {
                if (Model != null)
                {
                    return string.Format("Solution '{0}' ({1} {2})", Model.FileName, Model.Children.Count, StringProjects);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string StringProjects
        {
            get
            {
                if (Model.Children.Count == 1)
                {
                    return "project";
                }
                else
                {
                    return "projects";
                }
            }
        }

        public override bool CanAcceptDrop(Type type)
        {
            bool result = false;

            if (type == typeof(StandardProjectViewModel))
            {
                result = true;
            }

            //if (type == typeof(TestProjectViewModel))
            //{
            //    result = true;
            //}

            if (type == typeof(SolutionFolderViewModel))
            {
                result = true;
            }

            return result;
        }

        public override void Drop(ProjectItemViewModel item)
        {
            var project = item as ProjectViewModel;

            if (project != null)
            {
                var parent = project.Model.Solution.GetParent(project.Model);

                parent.RemoveItem(project.Model);

                Model.AttachItem(project.Model);
            }
            else if (item is SolutionFolderViewModel)
            {
                var folder = item as SolutionFolderViewModel;

                var parent = folder.Model.Solution.GetParent(folder.Model);

                parent.DetachItem(folder.Model);

                Model.AttachItem(folder.Model);
            }
        }
    }
}
