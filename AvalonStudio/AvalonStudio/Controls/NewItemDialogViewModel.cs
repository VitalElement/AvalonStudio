namespace AvalonStudio.Controls.ViewModels
{
    using System;
    using Extensibility;
    using Projects;
    using ReactiveUI;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class NewItemDialogViewModel : ModalDialogViewModelBase
    {
        public NewItemDialogViewModel(IProjectFolder folder) : base("New Item")
        {
            templates = new ObservableCollection<ICodeTemplate>();

            var compatibleTemplates = Workspace.Instance.CodeTempates.Where(t => t.IsCompatible(folder.Project));

            foreach(var template in compatibleTemplates)
            {
                templates.Add(template);
            }

            SelectedTemplate = this.templates.FirstOrDefault();

            this.folder = folder;

            OKCommand = ReactiveCommand.Create();

            OKCommand.Subscribe(_ =>
            {
                if(SelectedTemplate != null)
                {
                    
                }

                Close();
            });
        }

        private ICodeTemplate selectedTemplate;
        public ICodeTemplate SelectedTemplate
        {
            get { return selectedTemplate; }
            set { this.RaiseAndSetIfChanged(ref selectedTemplate, value); }
        }

        private ObservableCollection<ICodeTemplate> templates;
        public ObservableCollection<ICodeTemplate> Templates
        {
            get { return templates; }
            set { this.RaiseAndSetIfChanged(ref templates, value); }
        }

        private IProjectFolder folder;
        public IProjectFolder Folder
        {
            get { return folder; }
            set { this.RaiseAndSetIfChanged(ref folder, value); }
        }

        public override ReactiveCommand<object> OKCommand { get; protected set; }        
    }
}
