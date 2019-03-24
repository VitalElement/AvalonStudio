using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.Generic;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class DebuggerSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
    {
        private List<IDebugger> debuggers;

        private object debugSettingsControl;

        private IDebugger selectedDebugger;

        public DebuggerSettingsFormViewModel(CPlusPlusProject project) : base("Debugger", project)
        {
            debuggers = new List<IDebugger>(IoC.GetInstances<IDebugger>());
            selectedDebugger = project.Debugger2;

            if (selectedDebugger != null)
            {
                DebugSettingsControl = selectedDebugger.GetSettingsControl(Model);
            }
        }

        public object DebugSettingsControl
        {
            get { return debugSettingsControl; }
            set { this.RaiseAndSetIfChanged(ref debugSettingsControl, value); }
        }

        public List<IDebugger> Debuggers
        {
            get { return debuggers; }
            set { this.RaiseAndSetIfChanged(ref debuggers, value); }
        }

        public IDebugger SelectedDebugger
        {
            get
            {
                return selectedDebugger;
            }
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

        public void Save()
        {
            if (selectedDebugger != null)
            {
                Model.Debugger2Reference = selectedDebugger.GetType().ToString();
                Model.Save();
            }
        }
    }
}