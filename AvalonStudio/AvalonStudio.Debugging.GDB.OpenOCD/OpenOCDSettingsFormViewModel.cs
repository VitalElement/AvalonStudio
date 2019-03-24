using Avalonia;
using Avalonia.Controls;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AvalonStudio.Extensibility;
using System.Reactive;

namespace AvalonStudio.Debugging.GDB.OpenOCD
{
    public class OpenOCDSettingsFormViewModel : ViewModel<IProject>
    {
        public static string BaseDirectory
        {
            get { return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Debugging.OpenOCD\\").ToPlatformPath(); }
        }

        private string interfaceConfigFile;
        private readonly OpenOCDSettings settings;

        private string targetConfigFile;

        public OpenOCDSettingsFormViewModel(IProject model) : base(model)
        {
            settings = model.GetDebuggerSettings<OpenOCDSettings>();
            interfaceConfigFile = settings.InterfaceConfigFile;
            targetConfigFile = settings.TargetConfigFile;

            BrowseInterfaceConfigFileCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var ofd = new OpenFileDialog();
                ofd.InitialDirectory = Path.Combine(BaseDirectory, "scripts", "interface");
                ofd.Filters.Add(new FileDialogFilter { Name = "OpenOCD Config File", Extensions = new List<string> { "cfg" } });
                ofd.AllowMultiple = false;
                ofd.Title = "Open OpenOCD Interface Config File";

                var result = await ofd.ShowAsync(Application.Current.MainWindow);

                if (result != null && !string.IsNullOrEmpty(result.FirstOrDefault()))
                {
                    InterfaceConfigFile = BaseDirectory.MakeRelativePath(result.First());
                }
            });

            BrowseTargetConfigFileCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var ofd = new OpenFileDialog();
                ofd.InitialDirectory = Path.Combine(BaseDirectory, "scripts", "target");
                ofd.Filters.Add(new FileDialogFilter { Name = "OpenOCD Config File", Extensions = new List<string> { "cfg" } });
                ofd.AllowMultiple = false;
                ofd.Title = "Open OpenOCD Target Config File";

                var result = await ofd.ShowAsync(Application.Current.MainWindow);

                if (result != null && !string.IsNullOrEmpty(result.FirstOrDefault()))
                {
                    TargetConfigFile = BaseDirectory.MakeRelativePath(result.First());
                }
            });
        }

        public string InterfaceConfigFile
        {
            get
            {
                return interfaceConfigFile;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref interfaceConfigFile, value);
                Save();
            }
        }

        public string TargetConfigFile
        {
            get
            {
                return targetConfigFile;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref targetConfigFile, value);
                Save();
            }
        }

        public ReactiveCommand<Unit, Unit> BrowseInterfaceConfigFileCommand { get; }
        public ReactiveCommand<Unit, Unit> BrowseTargetConfigFileCommand { get; }

        private void Save()
        {
            settings.InterfaceConfigFile = interfaceConfigFile?.ToAvalonPath();
            settings.TargetConfigFile = targetConfigFile?.ToAvalonPath();

            Model.SetDebuggerSettings(settings);
            Model.Save();
        }
    }
}