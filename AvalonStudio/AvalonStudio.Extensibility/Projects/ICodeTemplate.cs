using System.Composition;
using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
	//[InheritedExport(typeof (ICodeTemplate))]
	public interface ICodeTemplate
	{
		string Title { get; }

		string Description { get; }

		object TemplateForm { get; }

		Task Generate(IProjectFolder folder);

		bool IsCompatible(IProject project);
	}
}