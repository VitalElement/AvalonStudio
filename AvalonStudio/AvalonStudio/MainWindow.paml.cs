using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using Mabiavalon.DockNC;

namespace AvalonStudio
{
	public class MainWindow : MetroWindow
	{
        private DockControl _mainDock;

		public MainWindow()
		{
			InitializeComponent();

			DataContext = ShellViewModel.Instance;

			IoC.Get<ICommandKeyGestureService>().BindKeyGestures(this);
			this.AttachDevTools();

            _mainDock = this.Find<DockControl>("mainDock");
		}

        protected override void OnKeyDown(KeyEventArgs e)
		{
			(DataContext as ShellViewModel)?.OnKeyDown(e);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}