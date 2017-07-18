using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public class SearchResultViewModel : ViewModel<ISourceFile>
    {
        public SearchResultViewModel(ISourceFile model) : base(model)
        {
        }

        public string Title => Model.Name;

        public string Location => $"{Model.Project.Name}\\{Model.Project.Location.MakeRelativePath(Model.Location)}";
    }
}