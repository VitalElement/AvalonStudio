using Avalonia.Media;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.MVVM;
using ReactiveUI;
using System.Linq;

namespace AvalonStudio.Controls.Editor
{
    public class ParameterViewModel : ViewModel<Parameter>
    {
        private SignatureViewModel _signature;

        public ParameterViewModel(SignatureViewModel signature, Parameter model) : base(model)
        {
            ResetFontWeight();
            _signature = signature;
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

        public string Name => Model.Name;

        public string DisplayName
        {
            get
            {
                if (_signature.Parameters.LastOrDefault() == this)
                {
                    return Model.Name;
                }
                else
                {
                    return Model.Name + ",";
                }
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