using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class ParameterSymbolViewModel : ViewModel<ParameterSymbol>
    {
        public ParameterSymbolViewModel(ParameterSymbol model) : base(model)
        {
            if(model.IsBuiltInType)
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
