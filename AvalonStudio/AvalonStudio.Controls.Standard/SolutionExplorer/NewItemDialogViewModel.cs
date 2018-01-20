using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using AvalonStudio.Extensibility.Templating;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class NewItemDialogViewModel : ModalDialogViewModelBase
    {
        private IProjectFolder folder;

        private ITemplate selectedTemplate;

        private ObservableCollection<ITemplate> templates;

        public NewItemDialogViewModel(IProjectFolder folder) : base("New Item")
        {
            var shell = IoC.Get<IShell>();
            var templateManager = IoC.Get<TemplateManager>();            

            templates = new ObservableCollection<ITemplate>(templateManager.ListItemTemplates(""));

            SelectedTemplate = templates.FirstOrDefault();

            this.folder = folder;

            OKCommand = ReactiveCommand.Create(async () =>
            {
                await templateManager.CreateTemplate(SelectedTemplate, folder.LocationDirectory);                

                Close();
            });
        }

        public ITemplate SelectedTemplate
        {
            get { return selectedTemplate; }
            set { this.RaiseAndSetIfChanged(ref selectedTemplate, value); }
        }

        public ObservableCollection<ITemplate> Templates
        {
            get { return templates; }
            set { this.RaiseAndSetIfChanged(ref templates, value); }
        }

        public IProjectFolder Folder
        {
            get { return folder; }
            set { this.RaiseAndSetIfChanged(ref folder, value); }
        }

        public override ReactiveCommand OKCommand { get; protected set; }
    }
}