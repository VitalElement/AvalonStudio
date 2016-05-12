namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects.Standard;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DefinitionViewModel : ViewModel<Definition>
    {
        private IProject project;

        public DefinitionViewModel(IProject project, Definition model) : base(model)
        {
            this.project = project;
            this.exported = model.Exported;
            this.global = model.Global;
        }

        public string Value
        {
            get { return Model.Value; }
        }

        private bool exported;
        public bool Exported
        {
            get { return exported; }
            set { this.RaiseAndSetIfChanged(ref exported, value); Model.Exported = value; project.Save(); }
        }

        private bool global;
        public bool Global
        {
            get { return global; }
            set { this.RaiseAndSetIfChanged(ref global, value); Model.Global = value; project.Save(); }
        }
    }
}
