using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    public interface IProjectType
    {
        Task<IProject> LoadAsync(ISolution solution, string filePath);
    }
}