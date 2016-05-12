namespace AvalonStudio.Projects.CPlusPlus
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Avalonia.Controls;

    public class ClassCodeTemplate : ICodeTemplate
    {
        public string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public Control TemplateForm
        {
            get
            {
                return null;
            }
        }

        public string Title
        {
            get
            {
                return "C/C++ Class";
            }
        }

        public Task<ISourceFile> Generate(IProjectFolder folder, string name)
        {
            return Task<ISourceFile>.Factory.StartNew(() =>
            {
                var result = SourceFile.Create(folder.Project, folder, folder.Location, name, "test text");

                return result;
            });
        }

        public bool IsCompatible(IProject project)
        {
            bool result = false;

            if(project is CPlusPlusProject)
            {
                result = true;
            }

            return result;
        }
    }
}
