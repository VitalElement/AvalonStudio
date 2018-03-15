using Avalonia.Diagnostics.ViewModels;
using AvalonStudio.Extensibility.Templating;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    class TemplateViewModel : ViewModelBase
    {
        private ITemplate _inner;

        public TemplateViewModel(ITemplate template)
        {
            _inner = template;
        }

        public IEnumerable<TemplateParameterViewModel> TemplateParameters => _inner.Parameters.Select(p => new TemplateParameterViewModel(p));

        public string Name => _inner.Name;

        public ITemplate Template => _inner;
    }
}
