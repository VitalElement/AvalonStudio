namespace AvalonStudio.Controls.Editor
{
    using Avalonia.Media;
    using AvalonStudio.Languages;
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System.Linq;

    public class ParameterSymbolViewModel : ViewModel<ParameterSymbol>
    {
        private SymbolViewModel _symbol;

        public ParameterSymbolViewModel(SymbolViewModel symbol, ParameterSymbol model) : base(model)
        {
            _symbol = symbol;

            if (model.IsBuiltInType)
            {
                BuiltInTypeDescription = model.TypeDescription + " ";
            }
            else
            {
                TypeDescription = model.TypeDescription + " ";
            }

            ResetFontWeight();
        }

        public void ResetFontWeight()
        {
            FontWeight = FontWeight.Light;
        }

        public string DisplayName
        {
            get
            {
                if (_symbol.Arguments.LastOrDefault() == this)
                {
                    return Model.Name;
                }
                else
                {
                    return Model.Name + ",";
                }
            }
        }


        public string Name
        {
            get { return Model.Name; }
        }

        public string TypeDescription { get; set; }

        public string BuiltInTypeDescription { get; set; }

        public string Comment
        {
            get { return Model.Comment; }
        }

        private FontWeight fontWeight;

        public FontWeight FontWeight
        {
            get { return fontWeight; }
            set { this.RaiseAndSetIfChanged(ref fontWeight, value); }
        }
    }
}