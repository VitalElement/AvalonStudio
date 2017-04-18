using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class NewItemDialogViewModel : ModalDialogViewModelBase
    {
        private IProjectFolder folder;

        private ICodeTemplate selectedTemplate;

        private ObservableCollection<ICodeTemplate> templates;

        public NewItemDialogViewModel(IProjectFolder folder) : base("New Item")
        {
            var shell = IoC.Get<IShell>();
            templates = new ObservableCollection<ICodeTemplate>();

            var compatibleTemplates = shell.CodeTemplates.Where(t => t.IsCompatible(folder.Project));

            foreach (var template in compatibleTemplates)
            {
                templates.Add(template);
            }

            SelectedTemplate = templates.FirstOrDefault();

            this.folder = folder;

            OKCommand = ReactiveCommand.Create();

            OKCommand.Subscribe(_ =>
            {
                SelectedTemplate?.Generate(folder);

                Close();
            });
        }

        public ICodeTemplate SelectedTemplate
        {
            get { return selectedTemplate; }
            set { this.RaiseAndSetIfChanged(ref selectedTemplate, value); }
        }

        public ObservableCollection<ICodeTemplate> Templates
        {
            get { return templates; }
            set { this.RaiseAndSetIfChanged(ref templates, value); }
        }

        public IProjectFolder Folder
        {
            get { return folder; }
            set { this.RaiseAndSetIfChanged(ref folder, value); }
        }

        public override ReactiveCommand<object> OKCommand { get; protected set; }
    }
}