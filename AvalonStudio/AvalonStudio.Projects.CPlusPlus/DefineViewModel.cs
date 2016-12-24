using AvalonStudio.MVVM;
using AvalonStudio.Projects.Standard;
using ReactiveUI;

namespace AvalonStudio.Projects.CPlusPlus
{
	public class DefinitionViewModel : ViewModel<Definition>
	{
		private bool exported;

		private bool global;
		private readonly IProject project;

		public DefinitionViewModel(IProject project, Definition model) : base(model)
		{
			this.project = project;
			exported = model.Exported;
			global = model.Global;
		}

		public string Value
		{
			get { return Model.Value; }
		}

		public bool Exported
		{
			get { return exported; }
			set
			{
				this.RaiseAndSetIfChanged(ref exported, value);
				Model.Exported = value;
				project.Save();
			}
		}

		public bool Global
		{
			get { return global; }
			set
			{
				this.RaiseAndSetIfChanged(ref global, value);
				Model.Global = value;
				project.Save();
			}
		}
	}
}