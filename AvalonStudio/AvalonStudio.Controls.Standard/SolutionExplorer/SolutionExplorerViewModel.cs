using Avalonia.Controls;
using Avalonia.Input;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reactive.Linq;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    [Export(typeof(ISolutionExplorer))]
    [Export(typeof(IExtension))]
    [ExportToolControl]
    [Shared]
    public class SolutionExplorerViewModel : ToolViewModel, IActivatableExtension, ISolutionExplorer
    {
        public const string ToolId = "CIDSEVM00";

        private ViewModel selectedItem;

        private IProject selectedProject;
        private IStudio _studio;
        private IShell _shell;

        private ISolution model;
        private SolutionViewModel solution;

        private IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> _solutionTypes;

        [ImportingConstructor]
        public SolutionExplorerViewModel(
            [ImportMany] IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> solutionTypes)
        {
            _shell = IoC.Get<IShell>();
            _studio = IoC.Get<IStudio>();

            _studio.SolutionChanged += (sender, e) => { Model = _studio.CurrentSolution; };

            _solutionTypes = solutionTypes;

            Title = "Solution Explorer";

            Id = ToolId;
        }

        public new ISolution Model
        {
            get
            {
                return model;
            }
            set
            {
                if (model != null)
                {
                    model.Items.Clear();
                    GC.Collect();
                }

                SelectedProject = null;

                model = value;

                if (Model != null)
                {
                    if (Model.Items.Count > 0)
                    {
                        SelectedProject = Model.StartupProject;
                    }

                    Solution = new SolutionViewModel(model);
                }
                else
                {
                    Solution = null;
                }

                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(Projects));
            }
        }

        internal void OnKeyDown(Key key, InputModifiers modifiers)
        {
            if (key == Key.Delete && modifiers == InputModifiers.None)
            {
                if (SelectedItem?.Model is IDeleteable deletable)
                {
                    deletable.Delete();
                }
            }
        }

        public SolutionViewModel Solution
        {
            get { return solution; }
            set { this.RaiseAndSetIfChanged(ref solution, value); }
        }

        public IProject SelectedProject
        {
            get
            {
                return selectedProject;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedProject, value);
            }
        }

        public ViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);

                if (value is SourceFileViewModel sourceFile)
                {
                    IoC.Get<IStudio>().OpenDocument((ISourceFile)sourceFile.Model);
                }
            }
        }

        public override MVVM.Location DefaultLocation
        {
            get { return MVVM.Location.Left; }
        }

        public void NewSolution()
        {
            _shell.ModalDialog = new NewProjectDialogViewModel();
            _shell.ModalDialog.ShowDialogAsync();
        }

        public async void OpenSolution()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Open Solution",

                InitialDirectory = Platform.ProjectDirectory
            };

            var allExtensions = new List<string>();

            foreach (var solutionType in _solutionTypes)
            {
                allExtensions.AddRange(solutionType.Metadata.SupportedExtensions);
            }

            allExtensions = allExtensions.Distinct().ToList();

            dlg.Filters.Add(new FileDialogFilter
            {
                Name = "All Supported Solution Types",
                Extensions = allExtensions
            });

            foreach (var solutionType in _solutionTypes)
            {
                dlg.Filters.Add(new FileDialogFilter
                {
                    Name = solutionType.Value.Description,
                    Extensions = solutionType.Metadata.SupportedExtensions.ToList()
                });
            }

            var result = await dlg.ShowAsync();

            if (result != null && !string.IsNullOrEmpty(result.FirstOrDefault()))
            {
                await IoC.Get<IStudio>().OpenSolutionAsync(result[0]);
            }
        }

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
            IoC.Get<IShell>().AddOrSelectTool(this);
        }
    }
}