using Avalonia.Media;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public class ParameterViewModel : ViewModel<Parameter>
    {
        public ParameterViewModel(Parameter model) : base(model)
        {
            ResetFontWeight();
        }

        public void ResetFontWeight()
        {
            FontWeight = FontWeight.Light;
        }

        private FontWeight fontWeight;

        public FontWeight FontWeight
        {
            get { return fontWeight; }
            set { this.RaiseAndSetIfChanged(ref fontWeight, value); }
        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
        }

        public string Label
        {
            get
            {
                return Model.Label;
            }
        }

        public string Documentation
        {
            get
            {
                return Model.Documentation;
            }
        }

        public string Type
        {
            get
            {
                return Model.Type;
            }
        }

        public string BuiltInType
        {
            get
            {
                return Model.BuiltInType;
            }
        }

        public bool HasDocumentation
        {
            get
            {
                return !string.IsNullOrEmpty(Documentation);
            }
        }
    }
}