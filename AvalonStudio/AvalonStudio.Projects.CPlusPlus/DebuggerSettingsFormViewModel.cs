namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.Debugging;
    using AvalonStudio.MVVM;
    using Extensibility;
    using Perspex.Controls;
    using ReactiveUI;
    using System.Collections.Generic;

    public class DebuggerSettingsFormViewModel : ViewModel<CPlusPlusProject>
    {
        public DebuggerSettingsFormViewModel(CPlusPlusProject project) : base(project)
        {
            debuggers = new List<IDebugger>(IoC.Get<IShell>().Debuggers);
            selectedDebugger = project.Debugger;
        }

        public void Save()
        {
            if (selectedDebugger != null)
            {
                Model.DebuggerReference = selectedDebugger?.GetType().ToString();
                selectedDebugger.ProvisionSettings(Model);
                Model.Save();
            }
        }

        private UserControl debugSettingsControl;
        public UserControl DebugSettingsControl
        {
            get { return debugSettingsControl; }
            set { this.RaiseAndSetIfChanged(ref debugSettingsControl, value); }
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
            set
            {
                this.RaiseAndSetIfChanged(ref selectedDebugger, value);

                Save();

                if (value != null)
                {                    
                    DebugSettingsControl = value.GetSettingsControl(Model);
                }
            }
        }
    }
}
