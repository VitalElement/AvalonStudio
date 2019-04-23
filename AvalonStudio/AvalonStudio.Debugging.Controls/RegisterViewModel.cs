using Avalonia.Media;
using AvalonStudio.MVVM;
using ReactiveUI;
using System;
using System.Reactive;

namespace AvalonStudio.Debugging
{
    public class RegisterViewModel : ViewModel
    {
        private IBrush background;

        private bool hasChanged;

        private string name;

        private string val;

        public RegisterViewModel(Register model)
        {
            name = model.Name;
            val = model.Value;
            Index = model.Index;

            ShowInMemoryCommand = ReactiveCommand.Create(() => { }); //WorkspaceViewModel.Instance.DebugManager.MemoryView.Address = Value;
        }

        public ReactiveCommand<Unit, Unit> ShowInMemoryCommand { get; }

        public IBrush Background
        {
            get { return background; }
            set { this.RaiseAndSetIfChanged(ref background, value); }
        }

        public bool HasChanged
        {
            get
            {
                return hasChanged;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref hasChanged, value);

                if (value)
                {
                    Background = Brush.Parse("#33008299");
                }
                else
                {
                    Background = null;
                }
            }
        }

        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }

        public string Value
        {
            get { return val; }
            set { this.RaiseAndSetIfChanged(ref val, value); }
        }

        public int Index { get; private set; }
    }
}