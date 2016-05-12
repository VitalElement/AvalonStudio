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
        
        Task<ISourceFile> Generate(IProjectFolder folder, string name);

        bool IsCompatible(IProject project);

        Control TemplateForm { get; }
    }
}
