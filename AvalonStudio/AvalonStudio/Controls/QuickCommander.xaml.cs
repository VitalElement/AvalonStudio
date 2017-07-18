using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Reactive.Disposables;
using System;
using Avalonia;
using System.Reactive.Linq;
using Avalonia.Controls.Primitives;

namespace AvalonStudio.Controls
{
    public class QuickCommander : UserControl
    {
        private CompositeDisposable disposables;        

        public QuickCommander()
        {
            InitializeComponent();

            disposables = new CompositeDisposable
            {
                this.GetObservable(DataContextProperty).OfType<QuickCommanderViewModel>().Subscribe(vm => vm.AttachControl(this))
            };

            GotFocus += (sender, e) =>
            {
                // _commandBox?.Focus();
            };
        }        

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}