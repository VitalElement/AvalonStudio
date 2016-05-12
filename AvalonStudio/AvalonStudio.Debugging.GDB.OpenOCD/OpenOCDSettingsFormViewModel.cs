namespace AvalonStudio.Debugging.GDB.OpenOCD
{
    using System;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using ReactiveUI;
    using Perspex.Controls;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using AvalonStudio.Utils;
    using Platforms;

    public class OpenOCDSettingsFormViewModel : ViewModel<IProject>
    {
        private OpenOCDSettings settings;

        public OpenOCDSettingsFormViewModel(IProject model) : base(model)
        {
            settings = OpenOCDDebugAdaptor.GetSettings(model);
            interfaceConfigFile = settings.InterfaceConfigFile;
            targetConfigFile = settings.TargetConfigFile;

            BrowseInterfaceConfigFileCommand = ReactiveCommand.Create();
            BrowseInterfaceConfigFileCommand.Subscribe(async _ =>
            {
                var ofd = new OpenFileDialog();
                ofd.InitialDirectory = Path.Combine(OpenOCDDebugAdaptor.BaseDirectory, "scripts", "interface");
                ofd.Filters.Add(new FileDialogFilter { Name = "OpenOCD Config File", Extensions = new List<string> { "cfg" } });
                ofd.AllowMultiple = false;
                ofd.Title = "Open OpenOCD Interface Config File";

                var result = await ofd.ShowAsync();

                if(result != null && !string.IsNullOrEmpty(result.First()))
                {
                    InterfaceConfigFile = OpenOCDDebugAdaptor.BaseDirectory.MakeRelativePath(result.First());
                }
            });

            BrowseTargetConfigFileCommand = ReactiveCommand.Create();
            BrowseTargetConfigFileCommand.Subscribe(async _ =>
            {
                var ofd = new OpenFileDialog();
                ofd.InitialDirectory = Path.Combine(OpenOCDDebugAdaptor.BaseDirectory, "scripts", "target");
                ofd.Filters.Add(new FileDialogFilter { Name = "OpenOCD Config File", Extensions = new List<string> { "cfg" } });
                ofd.AllowMultiple = false;
                ofd.Title = "Open OpenOCD Target Config File";

                var result = await ofd.ShowAsync();

                if (result != null && !string.IsNullOrEmpty(result.First()))
                {
                    TargetConfigFile = OpenOCDDebugAdaptor.BaseDirectory.MakeRelativePath(result.First());
                }
            });
        }

        private void Save()
        {
            settings.InterfaceConfigFile = interfaceConfigFile?.ToAvalonPath();
            settings.TargetConfigFile = targetConfigFile?.ToAvalonPath();

            OpenOCDDebugAdaptor.SetSettings(Model, settings);
            Model.Save();
        }

        private string interfaceConfigFile;
        public string InterfaceConfigFile
        {
            get { return interfaceConfigFile; }
            set { this.RaiseAndSetIfChanged(ref interfaceConfigFile, value); Save(); }
        }

        private string targetConfigFile;
        public string TargetConfigFile
        {
            get { return targetConfigFile; }
            set { this.RaiseAndSetIfChanged(ref targetConfigFile, value); Save(); }
        }

        public ReactiveCommand<object> BrowseInterfaceConfigFileCommand { get; }
        public ReactiveCommand<object> BrowseTargetConfigFileCommand { get; }

    }
}
