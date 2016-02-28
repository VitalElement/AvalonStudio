namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.Debugging;
    using AvalonStudio.MVVM;
    using Extensibility;
    using ReactiveUI;
    using System.Collections.Generic;

    public class DebuggerSettingsFormViewModel : ViewModel<CPlusPlusProject>
    {
        public DebuggerSettingsFormViewModel(CPlusPlusProject project) : base(project)
        {
            debuggers = new List<IDebugger>(Workspace.Instance.Debuggers);
            selectedDebugger = project.Debugger;
        }

        public void Save()
        {
            Model.DebuggerReference = selectedDebugger?.GetType().ToString();
            selectedDebugger.ProvisionSettings(Model);
            Model.Save();
        }

        private List<IDebugger> debuggers;
        public List<IDebugger> Debuggers
        {
            get { return debuggers; }
            set { this.RaiseAndSetIfChanged(ref debuggers, value); }
        }

        private IDebugger selectedDebugger;
        public IDebugger SelectedDebugger
        {
            get { return selectedDebugger; }
            set { this.RaiseAndSetIfChanged(ref selectedDebugger, value); Save(); }
        }
    }
}
