using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    public interface ICodeTemplate
    {
        string Title { get; }

        string Description { get; }

        object TemplateForm { get; }

        Task Generate(IProjectFolder folder);

        bool IsCompatible(IProject project);
    }
}