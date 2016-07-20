using System;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;

namespace AvalonStudio.Debugging.GDB.JLink
{
	public class JLinkSettingsFormViewModel : ViewModel<IProject>
	{
		private int interfaceSelectedIndex;

		private JlinkInterfaceType interfaceType;
		private readonly JLinkSettings settings;

		public JLinkSettingsFormViewModel(IProject model) : base(model)
		{
			settings = JLinkDebugAdaptor.GetSettings(model);

			//InterfaceOptions = new List<string>(Enum.GetNames(typeof(JlinkInterfaceType)));
			interfaceSelectedIndex = (int) settings.Interface;
			interfaceType = settings.Interface;
		}

		public string[] InterfaceOptions
		{
			get { return Enum.GetNames(typeof (JlinkInterfaceType)); }
		}

		public int InterfaceSelectedIndex
		{
			get { return interfaceSelectedIndex; }
			set
			{
				interfaceSelectedIndex = value;
				InterfaceType = (JlinkInterfaceType) interfaceSelectedIndex;
			}
		}

		public JlinkInterfaceType InterfaceType
		{
			get { return interfaceType; }
			set
			{
				interfaceType = value;
				Save();
			}
		}

		private void Save()
		{
			settings.Interface = (JlinkInterfaceType) interfaceSelectedIndex;

			JLinkDebugAdaptor.SetSettings(Model, settings);
			Model.Save();
		}
	}
}