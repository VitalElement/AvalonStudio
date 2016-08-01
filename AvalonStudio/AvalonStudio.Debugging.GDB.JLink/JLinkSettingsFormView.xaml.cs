using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Debugging.GDB.JLink
{
	public class JLinkSettingsFormView : UserControl
	{
		public JLinkSettingsFormView()
		{
			InitializeComponent();

            var lb = this.FindControl<ListBox>("deviceListBox");

            //if(lb != null)
            //{
            //    lb.it
            //}

		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}