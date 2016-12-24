using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace AvalonStudio.Projects.CPlusPlus
{
	public class ClassCodeTemplate : ICodeTemplate
	{
		private readonly ClassTemplateSettingsViewModel _settings;

		public ClassCodeTemplate()
		{
			_settings = new ClassTemplateSettingsViewModel();
		}

		public string Description => string.Empty;

		public object TemplateForm => _settings;

		public string Title => "C/C++ Class";

		public async Task Generate(IProjectFolder folder)
		{
			await Task.Factory.StartNew(async () =>
			{
				var name = _settings.ClassName;

				var sourceTemplate = new CPlusPlusClassTemplate(name, _settings.GenerateHeader);
				var headerTemplate = new CPlusPlusClassHeaderTemplate(name);

				if (_settings.GenerateHeader)
				{
					await SourceFile.Create(folder, $"{(name.Contains('.') ? name : name + ".h")}", headerTemplate.TransformText());
				}

				if (_settings.GenerateClass)
				{
                    await SourceFile.Create(folder, $"{name}.cpp", sourceTemplate.TransformText());
				}
			});
		}

		public bool IsCompatible(IProject project)
		{
			return project is CPlusPlusProject;
		}
	}
}