using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using ReactiveUI;
using AvalonStudio.GlobalSettings;

namespace AvalonStudio.Projects.OmniSharp
{
	public class ToolchainSettingsFormViewModel : HeaderedViewModel
	{
        private DotNetToolchainSettings _settings;

		public ToolchainSettingsFormViewModel() : base("DotNet Core Location")
		{
            _settings = SettingsBase.GetSettings<DotNetToolchainSettings>();

			
		}

		public ReactiveCommand<object> AddIncludePathCommand { get; }

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