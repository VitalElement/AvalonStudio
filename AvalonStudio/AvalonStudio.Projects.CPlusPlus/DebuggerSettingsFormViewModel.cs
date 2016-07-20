using System.Collections.Generic;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Projects.CPlusPlus
{
	public class DebuggerSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
	{
		private List<IDebugger> debuggers;

		private object debugSettingsControl;

		private IDebugger selectedDebugger;

		public DebuggerSettingsFormViewModel(CPlusPlusProject project) : base("Debugger", project)
		{
			debuggers = new List<IDebugger>(IoC.Get<IShell>().Debuggers);
			selectedDebugger = project.Debugger;
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

		public void Save()
		{
			if (selectedDebugger != null)
			{
				Model.DebuggerReference = selectedDebugger?.GetType().ToString();
				selectedDebugger.ProvisionSettings(Model);
				Model.Save();
			}
		}
	}
}