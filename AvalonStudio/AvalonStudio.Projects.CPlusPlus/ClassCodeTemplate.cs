namespace AvalonStudio.Projects.CPlusPlus
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Avalonia.Controls;
    using Avalonia.Threading;

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

        public Task Generate(IProjectFolder folder)
        {
            return Task.Factory.StartNew(() =>
            {
                var name = _settings.ClassName;

                var sourceTemplate = new CPlusPlusClassTemplate(name, _settings.GenerateHeader);
                var headerTemplate = new CPlusPlusClassHeaderTemplate(name);

                if (_settings.GenerateHeader)
                {
                    Dispatcher.UIThread.InvokeAsync(() => folder.AddFile(SourceFile.Create(folder.Project, folder, folder.LocationDirectory, $"{(name.Contains('.') ? name : name + ".h")}", headerTemplate.TransformText())));
                }

                if (_settings.GenerateClass)
                {
                    Dispatcher.UIThread.InvokeAsync(() => folder.AddFile(SourceFile.Create(folder.Project, folder, folder.LocationDirectory, $"{name}.cpp", sourceTemplate.TransformText())));
                }
            });
        }

        public bool IsCompatible(IProject project)
        {
            return project is CPlusPlusProject;
        }
    }
}
