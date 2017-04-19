using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvalonStudio.Controls;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;

namespace AvalonStudio
{
    public class BootScreen : SplashScreen
    {
        public BootScreen()
        {
            this.InitializeComponent();
            App.AttachDevTools(this);

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Startup();
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Startup()
        {
            var progressBar = this.FindControl<ProgressBar>("StatusProgressBar");

            Platform.Initialise();

            PackageSources.InitialisePackageSources();

            var container = CompositionRoot.CreateContainer();

            ShellViewModel.Instance = container.GetExport<ShellViewModel>();

            var main = new MainWindow();

            main.WindowState = WindowState.Minimized;

            this.Hide();

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                main.WindowState = WindowState.Maximized;
            });

            main.Show();

            ShellViewModel.Instance.Cleanup();
        }
    }
}