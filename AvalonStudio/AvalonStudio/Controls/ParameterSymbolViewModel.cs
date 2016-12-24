namespace AvalonStudio.Controls
{
    using Avalonia.Media;
    using AvalonStudio.Languages;
    using AvalonStudio.MVVM;
    using System;
    using ReactiveUI;

    public class ParameterSymbolViewModel : ViewModel<ParameterSymbol>
	{
		public ParameterSymbolViewModel(ParameterSymbol model) : base(model)
		{
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