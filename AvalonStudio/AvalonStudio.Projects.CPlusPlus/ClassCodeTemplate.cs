using AvalonStudio.Extensibility.Templating;
using System.Linq;
using System.Threading.Tasks;

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

                if (_settings.GenerateHeader)
                {
                    var rendered = await Template.Engine.CompileRenderAsync("CppClassHeader.template", new { ClassName = _settings.ClassName });
                    await SourceFile.Create(folder, $"{(name.Contains('.') ? name : name + ".h")}", rendered);
                }

                if (_settings.GenerateClass)
                {
                    var rendered = await Template.Engine.CompileRenderAsync("CppClass.template", new { ClassName = _settings.ClassName });
                    await SourceFile.Create(folder, $"{name}.cpp", rendered);
                }
            });
        }

        public bool IsCompatible(IProject project)
        {
            return project is CPlusPlusProject;
        }
    }
}