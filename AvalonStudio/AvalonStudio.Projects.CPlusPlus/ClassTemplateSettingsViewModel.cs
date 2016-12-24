using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Projects.CPlusPlus
{
	public class ClassTemplateSettingsViewModel : ViewModel
	{
		private string _className;

		private bool _generateClass;

		private bool _generateHeader;

		public ClassTemplateSettingsViewModel()
		{
			_generateClass = true;
			_generateHeader = true;
		}

		public bool GenerateHeader
		{
			get { return _generateHeader; }
			set { this.RaiseAndSetIfChanged(ref _generateHeader, value); }
		}

		public bool GenerateClass
		{
			get { return _generateClass; }
			set { this.RaiseAndSetIfChanged(ref _generateClass, value); }
		}

		public string ClassName
		{
			get { return _className; }
			set { this.RaiseAndSetIfChanged(ref _className, value); }
		}
	}
}