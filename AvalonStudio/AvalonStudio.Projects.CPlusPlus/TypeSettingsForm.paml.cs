using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
	public class TypeSettingsFormView : UserControl
	{
		public TypeSettingsFormView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}