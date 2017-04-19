using Avalonia.Controls;
using AvalonStudio.GlobalSettings;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Projects.OmniSharp
{
    public class ToolchainSettingsFormViewModel : HeaderedViewModel
    {
        private DotNetToolchainSettings _settings;

        public ToolchainSettingsFormViewModel() : base("DotNet Core Location")
        {
            _settings = SettingsBase.GetSettings<DotNetToolchainSettings>();

            DotNetPath = _settings.DotNetPath;

            BrowseCommand = ReactiveCommand.Create();

            BrowseCommand.Subscribe(async _ =>
            {
                var ofd = new OpenFileDialog();
                ofd.Filters.Add(new FileDialogFilter { Name = "Dotnet Executable", Extensions = new List<string> { Platform.ExecutableExtension.Substring(1) } });
                ofd.AllowMultiple = false;
                ofd.Title = "Select Dotnet Executable (dotnet)";

                var result = await ofd.ShowAsync();

                if (result != null && !string.IsNullOrEmpty(result.First()))
                {
                    DotNetPath = result.First();

                    _settings.DotNetPath = DotNetPath;

                    _settings.Save();
                }
            });
        }

        public ReactiveCommand<object> BrowseCommand { get; }

        private string dotnetPath;

        public string DotNetPath
        {
            get { return dotnetPath; }
            set { this.RaiseAndSetIfChanged(ref dotnetPath, value); }
        }

        public void Save()
        {
        }
    }
}