﻿using Avalonia.Threading;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.Studio
{
    [Export(typeof(IStudio))]
    [Shared]
    class StudioViewModel : ReactiveObject, IStudio
    {
        private WorkspaceTaskRunner _taskRunner;
        private Perspective currentPerspective;
        private bool debugControlsVisible;

        [ImportingConstructor]
        public StudioViewModel([ImportMany] IEnumerable<Lazy<IEditorProvider>> editorProviders,
            [ImportMany] IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> languageServices,
            [ImportMany] IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> solutionTypes,
            [ImportMany] IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> projectTypes,
            [ImportMany] IEnumerable<Lazy<ITestFramework>> testFrameworks)
        {
            EditorProviders = editorProviders;
            LanguageServices = languageServices;
            SolutionTypes = solutionTypes;
            ProjectTypes = projectTypes;
            TestFrameworks = testFrameworks;

            _taskRunner = new WorkspaceTaskRunner();

            OnSolutionChanged = Observable.FromEventPattern<SolutionChangedEventArgs>(this, nameof(SolutionChanged)).Select(s => s.EventArgs.NewValue);

            CurrentPerspective = Perspective.Editor;
        }

        public IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> SolutionTypes { get; }

        public IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes { get; }

        public IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> LanguageServices { get; }

        public IEnumerable<Lazy<ITestFramework>> TestFrameworks { get; }

        public IEnumerable<Lazy<IEditorProvider>> EditorProviders { get; }

        public IWorkspaceTaskRunner TaskRunner => _taskRunner;

        public event EventHandler<BuildEventArgs> BuildStarting;
        public event EventHandler<BuildEventArgs> BuildCompleted;

        private ISolution currentSolution;
        public event EventHandler<SolutionChangedEventArgs> SolutionChanged;
        public IObservable<ISolution> OnSolutionChanged { get; }

        public bool DebugVisible
        {
            get { return debugControlsVisible; }
            set { this.RaiseAndSetIfChanged(ref debugControlsVisible, value); }
        }

        public Perspective CurrentPerspective
        {
            get
            {
                return currentPerspective;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref currentPerspective, value);

                switch (value)
                {
                    case Perspective.Editor:
                        DebugVisible = false;
                        break;

                    case Perspective.Debug:
                        // TODO close intellisense, and tooltips.
                        // disable documents, get rid of error list, solution explorer, etc.    (isreadonly)
                        DebugVisible = true;
                        break;
                }
            }
        }

        public ISolution CurrentSolution
        {
            get
            {
                return currentSolution;
            }
            set
            {
                var oldValue = CurrentSolution;

                this.RaiseAndSetIfChanged(ref currentSolution, value);

                SolutionChanged?.Invoke(this, new SolutionChangedEventArgs() { OldValue = oldValue, NewValue = currentSolution });
            }
        }

        public void Save()
        {
            var shell = IoC.Get<IShell>();

            if (shell.Layout.FocusedView is IFileDocumentTabViewModel document)
            {
                document.Editor.Save();
            }
        }

        public void SaveAll()
        {
            var shell = IoC.Get<IShell>();

            foreach (var document in shell.Documents.OfType<IFileDocumentTabViewModel>())
            {
                document.Editor?.Save();
            }
        }

        public void Clean()
        {
            var project = GetDefaultProject();

            if (project != null)
            {
                Clean(project);
            }
        }

        public void Build()
        {
            var project = GetDefaultProject();

            if (project != null)
            {
                BuildAsync(project).GetAwaiter();
            }
        }

        public void Clean(IProject project)
        {
            var console = IoC.Get<IConsole>();

            console.Clear();

            if (project.ToolChain != null)
            {
                BuildStarting?.Invoke(this, new BuildEventArgs(BuildType.Clean, project));

                TaskRunner.RunTask(() =>
                {
                    project.ToolChain.Clean(IoC.Get<IConsole>(), project).Wait();

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        BuildCompleted?.Invoke(this, new BuildEventArgs(BuildType.Clean, project));
                    });
                });
            }
            else
            {
                console.WriteLine($"No toolchain selected for {project.Name}");
            }
        }

        public async Task<bool> BuildAsync(IProject project)
        {
            bool result = false;

            SaveAll();

            var console = IoC.Get<IConsole>();

            console.Clear();

            if (project.ToolChain != null)
            {
                BuildStarting?.Invoke(this, new BuildEventArgs(BuildType.Build, project));

                await TaskRunner.RunTask(() =>
                {
                    result = project.ToolChain.BuildAsync(IoC.Get<IConsole>(), project).Result;
                });

                Dispatcher.UIThread.Post(() =>
                {
                    BuildCompleted?.Invoke(this, new BuildEventArgs(BuildType.Build, project));
                });
            }
            else
            {
                console.WriteLine($"No toolchain selected for {project.Name}");
            }

            return result;
        }

        public void CloseDocumentsForProject(IProject project)
        {
            var shell = IoC.Get<IShell>();

            var documentsToClose = shell.Documents.ToList();

            foreach (var document in documentsToClose)
            {
                if (document is EditorViewModel evm && evm.SourceFile.Project == project)
                {
                    shell.RemoveDocument(evm);
                }
            }
        }

        public IFileDocumentTabViewModel OpenDocument(ISourceFile file)
        {
            var shell = IoC.Get<IShell>();

            var currentTab = shell.Documents.OfType<IFileDocumentTabViewModel>().FirstOrDefault(t => t.SourceFile?.FilePath == file.FilePath);

            if (currentTab == null)
            {
                var provider = IoC.Get<IStudio>().EditorProviders.FirstOrDefault(p => p.Value.CanEdit(file))?.Value;

                if (provider != null)
                {
                    currentTab = provider.CreateViewModel(file);

                    shell.AddDocument(currentTab);
                }
                else
                {
                    var newTab = new TextEditorViewModel(file);

                    shell.AddDocument(newTab);

                    currentTab = newTab;
                }
            }
            else
            {
                shell.AddDocument(currentTab);
            }

            return currentTab;
        }

        public async Task<IEditor> OpenDocumentAsync(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true)
        {
            var shell = IoC.Get<IShell>();

            var currentTab = OpenDocument(file);

            if (shell.SelectedDocument is IFileDocumentTabViewModel fileTab)
            {
                await fileTab.WaitForEditorToLoadAsync();

                if (debugHighlight)
                {
                    fileTab.Editor.SetDebugHighlight(line, startColumn, endColumn);
                }

                if (selectLine || debugHighlight)
                {
                    fileTab.Editor.GotoPosition(line, startColumn != -1 ? startColumn : 1);
                }

                if (focus)
                {
                    shell.Layout.Factory.Select(fileTab);
                    fileTab.Editor.Focus();
                }

                if (currentTab is TextEditorViewModel editor)
                {
                    return editor.DocumentAccessor;
                }
            }

            return null;
        }

        public void RemoveDocument(ISourceFile file)
        {
            var shell = IoC.Get<IShell>();

            var document = shell.Documents.OfType<IFileDocumentTabViewModel>().FirstOrDefault(d => d.SourceFile == file);

            if (document != null)
            {
                shell.RemoveDocument(document);
            }
        }

        public IEditor GetDocument(string path)
        {
            var shell = IoC.Get<IShell>();

            return shell.Documents.OfType<TextEditorViewModel>().Where(d => d.SourceFile?.FilePath == path).Select(d => d.DocumentAccessor).FirstOrDefault();
        }

        public IProject GetDefaultProject()
        {
            IProject result = null;

            if (CurrentSolution != null)
            {
                if (CurrentSolution.StartupProject != null)
                {
                    result = CurrentSolution.StartupProject;
                }
                else
                {
                    //Console.WriteLine("No Default project is set in the solution.");
                }
            }
            else
            {
                //Console.WriteLine("No Solution is loaded.");
            }

            return result;
        }

        public void InvalidateCodeAnalysis()
        {
            //foreach (var document in Documents)
            {
                //TODO implement code analysis trigger.
            }
        }

        public async Task OpenSolutionAsync(string path)
        {
            if (CurrentSolution != null)
            {
                await CloseSolutionAsync();
            }

            if (System.IO.File.Exists(path))
            {
                var extension = System.IO.Path.GetExtension(path);
                var solutionType = IoC.Get<IStudio>().SolutionTypes.FirstOrDefault(
                    s => s.Metadata.SupportedExtensions.Any(e => extension.EndsWith(e)));

                if (solutionType != null)
                {
                    var statusBar = IoC.Get<IStatusBar>();

                    statusBar.SetText($"Loading Solution: {path}");

                    var solution = await solutionType.Value.LoadAsync(path);

                    await solution.LoadSolutionAsync();

                    await solution.RestoreSolutionAsync();

                    statusBar.ClearText();

                    CurrentSolution = solution;

                    await CurrentSolution.LoadProjectsAsync();
                }
            }
        }

        public async Task CloseSolutionAsync()
        {
            // TODO clear error list?
            //IoC.Get<IErrorList>().Errors.Clear();

            if (CurrentSolution != null)
            {
                foreach (var project in CurrentSolution.Projects)
                {
                    CloseDocumentsForProject(project);
                }

                await CurrentSolution.UnloadProjectsAsync();

                await CurrentSolution.UnloadSolutionAsync();

                CurrentSolution = null;
            }
        }
    }
}
