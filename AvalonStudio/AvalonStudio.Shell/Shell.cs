using Avalonia;
using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;

namespace AvalonStudio.Shell
{
    public static class Shell
    {
        public static void StartShellApp<TAppBuilder>(this TAppBuilder builder) where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            builder.UseReactiveUI().AfterSetup(_ =>
            {
                Platform.Initialise();

                var extensionManager = new ExtensionManager();
                var container = CompositionRoot.CreateContainer(extensionManager);

                IoC.Initialise(container);

                ShellViewModel.Instance = IoC.Get<ShellViewModel>();

                ShellViewModel.Instance.Initialise();
            }).Start<MainWindow>();
        }
    }
}
