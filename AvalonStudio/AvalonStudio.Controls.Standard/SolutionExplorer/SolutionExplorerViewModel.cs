using Avalonia.Controls;
using Avalonia.Input;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    [Export(typeof(ISolutionExplorer))]
    [Export(typeof(IExtension))]
    [Shared]
    public class SolutionExplorerViewModel : ToolViewModel, IActivatableExtension, ISolutionExplorer
    {
        public const string ToolId = "CIDSEVM00";

        private ISolution model;

        private ViewModel selectedItem;

        private IProject selectedProject;
        private IShell _shell;

        private SolutionViewModel solution;

        private IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> _solutionTypes;

        [ImportingConstructor]
        public SolutionExplorerViewModel(
            [ImportMany] IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> solutionTypes)
        {
            IoC.RegisterConstant<ISolutionExplorer>(this);

            _shell = IoC.Get<IShell>();
            _shell.SolutionChanged += (sender, e) => { Model = _shell.CurrentSolution; };

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

        internal void OnKeyDown (Key key, InputModifiers modifiers)
        {
            if(key == Key.Delete && modifiers == InputModifiers.None)
            {
                if(SelectedItem?.Model is IDeleteable deletable)
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
                    _shell.OpenDocument((ISourceFile)sourceFile.Model);
                }
            }
        }

        public override MVVM.Location DefaultLocation
        {
            get { return MVVM.Location.Right; }
        }

        public void NewSolution()
        {
            _shell.ModalDialog = new NewProjectDialogViewModel();
            _shell.ModalDialog.ShowDialog();
        }

        public async void OpenSolution()
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Open Solution";

            dlg.InitialDirectory = Platform.ProjectDirectory;

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
                await _shell.OpenSolutionAsync(result[0]);
            }
        }

        public void BeforeActivation() { }
        public void Activation() { }
    }
}