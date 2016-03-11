namespace AvalonStudio.Controls
{
    using AvalonStudio.Debugging;
    using AvalonStudio.MVVM;
    using Perspex.Media;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RegisterViewModel : ViewModel
    {
        public RegisterViewModel(Register model)
        {
            this.name = model.Name;
            this.val = model.Value;
            this.Index = model.Index;

            this.ShowInMemoryCommand = ReactiveCommand.Create();
            ShowInMemoryCommand.Subscribe( _=>
            {
                //WorkspaceViewModel.Instance.DebugManager.MemoryView.Address = Value;
            });
        }

        public ReactiveCommand<object> ShowInMemoryCommand { get; }

        private Brush background;
        public Brush Background
        {
            get { return background; }
            set { this.RaiseAndSetIfChanged(ref background, value); }
        }

        private bool hasChanged;
        public bool HasChanged
        {
            get { return hasChanged; }
            set
            {
                this.RaiseAndSetIfChanged(ref hasChanged, value);

                if(value)
                {
                    Background = Brush.Parse("#33008299");
                }
                else
                {
                    Background = null;
                }
            }
        }


        private string name;
        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }

        private string val;
        public string Value
        {
            get { return val; }
            set { this.RaiseAndSetIfChanged(ref val, value); }
        }

        public int Index { get; private set; }
    }
}
