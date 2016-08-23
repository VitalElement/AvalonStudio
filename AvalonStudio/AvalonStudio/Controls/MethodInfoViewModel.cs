namespace AvalonStudio.Controls
{
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.MVVM;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;

    public class MethodInfoViewModel : ViewModel<MethodInfo>
    {
        public MethodInfoViewModel(MethodInfo model) : base(model)
        {
            overloads = new List<SymbolViewModel>();

            foreach (var overload in model.Overloads)
            {
                overloads.Add(new SymbolViewModel(overload));
            }
                        
            SelectedIndex = 0;
            selectedOverload.ArgumentIndex = 0;
        }

        public int OverloadCount
        {
            get
            {
                return Overloads.Count;
            }
        }        

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if(value < 0 || value >= Overloads.Count)
                {
                    value = selectedIndex;
                }

                if(value < Overloads.Count)
                {
                    SelectedOverload = Overloads[value];
                }

                this.RaiseAndSetIfChanged(ref selectedIndex, value);
            }
        }

        private List<SymbolViewModel> overloads;
        public List<SymbolViewModel> Overloads
        {
            get { return overloads; }
            set { this.RaiseAndSetIfChanged(ref overloads, value); }
        }


        private SymbolViewModel selectedOverload;
        public SymbolViewModel SelectedOverload
        {
            get { return selectedOverload; }
            set { this.RaiseAndSetIfChanged(ref selectedOverload, value); }
        }
    }
}
