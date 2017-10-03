using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvalonStudio.Controls;
using AvalonStudio.Controls.Standard.SettingsDialog;
using AvalonStudio.Extensibility;

namespace AvalonStudio
{
    public class TestSetting : SettingsViewModel
    {
        public TestSetting(string title) : base(title)
        {
        }
    }

    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            IoC.RegisterConstant<Window>(this);

            InitializeComponent();

            DataContext = ShellViewModel.Instance;

            KeyBindings.AddRange(IoC.Get<ShellViewModel>().KeyBindings);

            this.AttachDevTools();

            IoC.Get<ISettingsManager>().RegisterSettingsDialog("IDE", new TestSetting("Test1"));
            IoC.Get<ISettingsManager>().RegisterSettingsDialog("IDE", new TestSetting("Test2"));
            IoC.Get<ISettingsManager>().RegisterSettingsDialog("IDE", new TestSetting("Test3"));

            IoC.Get<ISettingsManager>().RegisterSettingsDialog("Editor", new TestSetting("Test1"));
            IoC.Get<ISettingsManager>().RegisterSettingsDialog("Editor", new TestSetting("Test2"));
            IoC.Get<ISettingsManager>().RegisterSettingsDialog("Editor", new TestSetting("Test3"));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}