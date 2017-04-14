using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using ReactiveUI;
using AvalonStudio.GlobalSettings;
using System.Collections.Generic;
using AvalonStudio.Platforms;

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
                ofd.Filters.Add(new FileDialogFilter { Name = "Dotnet Executable", Extensions = new List<string> { Platform.ExecutableExtension } });
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