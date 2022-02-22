using AvalonStudio.Projects;
using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    public interface IOpenableItem
    {
        string Description { get; }

        Task<ISolution> LoadAsync(string path);
    }
}