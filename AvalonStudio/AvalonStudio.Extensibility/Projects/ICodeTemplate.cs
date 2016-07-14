namespace AvalonStudio.Projects
{
    using Avalonia.Controls;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    [InheritedExport(typeof(ICodeTemplate))]
    public interface ICodeTemplate
    {
        string Title { get; }        

        string Description { get; }
        
        Task Generate(IProjectFolder folder);

        bool IsCompatible(IProject project);

        object TemplateForm { get; }
    }
}
