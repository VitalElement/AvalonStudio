using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Utils;
using ReactiveUI;

namespace AvalonStudio.Projects.CPlusPlus
{
	public class IncludePathSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
	{
		private ObservableCollection<DefinitionViewModel> defines;

		private string defineText;

		private ObservableCollection<IncludeViewModel> includePaths;

		private DefinitionViewModel selectedDefine;

		private IncludeViewModel selectedInclude;

		public IncludePathSettingsFormViewModel(CPlusPlusProject model) : base("Includes / Definitions", model)
		{
			defines = new ObservableCollection<DefinitionViewModel>();
			includePaths = new ObservableCollection<IncludeViewModel>();

			foreach (var define in model.Defines)
			{
				defines.Add(new DefinitionViewModel(model, define));
			}

			foreach (var include in model.Includes)
			{
				includePaths.Add(new IncludeViewModel(model, include));
			}

			AddDefineCommand = ReactiveCommand.Create();
				// new RoutingCommand(AddDefine, (o) => DefineText != string.Empty && DefineText != null && !Defines.Contains(DefineText));
			AddDefineCommand.Subscribe(AddDefine);

			RemoveDefineCommand = ReactiveCommand.Create();
				// new RoutingCommand(RemoveDefine, (o) => SelectedDefine != string.Empty && SelectedDefine != null);
			RemoveDefineCommand.Subscribe(RemoveDefine);

			AddIncludePathCommand = ReactiveCommand.Create();
			AddIncludePathCommand.Subscribe(AddIncludePath);

			RemoveIncludePathCommand = ReactiveCommand.Create();
			RemoveIncludePathCommand.Subscribe(RemoveIncludePath);
		}

		public ReactiveCommand<object> AddIncludePathCommand { get; }
		public ReactiveCommand<object> RemoveIncludePathCommand { get; }
		public ReactiveCommand<object> AddDefineCommand { get; }
		public ReactiveCommand<object> RemoveDefineCommand { get; }

		public string DefineText
		{
			get { return defineText; }
			set { this.RaiseAndSetIfChanged(ref defineText, value); }
		}

		public DefinitionViewModel SelectedDefine
		{
			get { return selectedDefine; }
			set
			{
				this.RaiseAndSetIfChanged(ref selectedDefine, value);

				if (value != null)
				{
					DefineText = value.Model.Value;
				}
			}
		}

		public ObservableCollection<DefinitionViewModel> Defines
		{
			get { return defines; }
			set { this.RaiseAndSetIfChanged(ref defines, value); }
		}

		public IncludeViewModel SelectedInclude
		{
			get { return selectedInclude; }
			set { this.RaiseAndSetIfChanged(ref selectedInclude, value); }
		}

		public ObservableCollection<IncludeViewModel> IncludePaths
		{
			get { return includePaths; }
			set { this.RaiseAndSetIfChanged(ref includePaths, value); }
		}

		private void Save()
		{
			Model.Defines.Clear();

			foreach (var define in Defines)
			{
				Model.Defines.Add(define.Model);
			}

			Model.Includes.Clear();

			foreach (var include in IncludePaths)
			{
				Model.Includes.Add(include.Model);
			}

			Model.Save();
		}

		private void AddDefine(object param)
		{
			Defines.Add(new DefinitionViewModel(Model, new Definition {Value = DefineText}));
			DefineText = string.Empty;
			Save();
		}

		private void RemoveDefine(object param)
		{
			Defines.Remove(SelectedDefine);
			Save();
		}

		private async void AddIncludePath(object param)
		{
			var fbd = new OpenFolderDialog();

			fbd.InitialDirectory = Model.CurrentDirectory;

			var result = await fbd.ShowAsync();

			if (result != string.Empty)
			{
				var newInclude = Model.CurrentDirectory.MakeRelativePath(result).ToAvalonPath();

				if (newInclude == string.Empty)
				{
					newInclude = "\\";
				}

				IncludePaths.Add(new IncludeViewModel(Model, new Include {Value = newInclude}));

				Save();
			}
		}

		private bool AddIncludePathCanExecute(object param)
		{
			return true;
		}

		private void RemoveIncludePath(object param)
		{
			includePaths.Remove(SelectedInclude);
			Save();
		}
	}
}