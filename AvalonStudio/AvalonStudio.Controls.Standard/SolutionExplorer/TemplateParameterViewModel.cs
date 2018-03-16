using Avalonia.Controls;
using Avalonia.Diagnostics.ViewModels;
using AvalonStudio.Extensibility.Templating;
using System.Globalization;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    class TemplateParameterViewModel : ViewModelBase
    {
        private ITemplateParameter _inner;
        private string _value;

        public TemplateParameterViewModel(ITemplate parent, ITemplateParameter parameter)
        {
            _inner = parameter;

            if (parameter.Name == "name" && string.IsNullOrEmpty(parameter.DefaultValue))
            {
                _value = parent.DefaultName;
            }
            else
            {
                _value = parameter.DefaultValue;
            }
        }

        public string Name => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_inner.Name);

        public string Value
        {
            get { return _value; }
            set { this.RaiseAndSetIfChanged(ref _value, value); }
        }
    }
}
