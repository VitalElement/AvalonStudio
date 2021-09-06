using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class SolutionExplorerView : UserControl
    {
        private CompositeDisposable _disposables;

        public SolutionExplorerView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _disposables?.Dispose();

            _disposables = new CompositeDisposable();

            Observable.FromEventPattern<Shell.SolutionChangedEventArgs>(IoC.Get<IStudio>(), nameof(IStudio.SolutionChanged))
                .Subscribe(x =>
            {
                Focus();
            })
            .DisposeWith(_disposables);

            base.OnAttachedToVisualTree(e);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _disposables?.Dispose();

            _disposables = null;

            base.OnDetachedFromVisualTree(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(DataContext is SolutionExplorerViewModel vm)
            {
                vm.OnKeyDown(e.Key, e.KeyModifiers);
            }
        }
    }
}