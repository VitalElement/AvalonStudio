using Avalonia.Diagnostics.ViewModels;
using AvalonStudio.Extensibility.Templating;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    class TemplateParameterViewModel : ViewModelBase
    {
        private ITemplateParameter _inner;

        public TemplateParameterViewModel(ITemplateParameter parameter)
        {
            _inner = parameter;
        }

        public string Name => _inner.Name;
    }
}
