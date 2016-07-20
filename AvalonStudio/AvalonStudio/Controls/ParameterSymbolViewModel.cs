using AvalonStudio.Languages;
using AvalonStudio.MVVM;

namespace AvalonStudio.Controls
{
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
	}
}