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

    public class IncludeViewModel : ViewModel<Include>
    {
        private IProject project;

        public IncludeViewModel(IProject project, Include model) : base(model)
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
            set { this.RaiseAndSetIfChanged(ref exported, value); Model.Exported = value;  project.Save(); }
        }

        private bool global;
        public bool Global
        {
            get { return global; }
            set { this.RaiseAndSetIfChanged(ref global, value); Model.Global = value; project.Save(); }
        }
    }
}
