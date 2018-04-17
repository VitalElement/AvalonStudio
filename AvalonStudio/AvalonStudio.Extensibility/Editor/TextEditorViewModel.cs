using Avalonia.Input;
using AvalonStudio.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Editor
{
    public class TextEditorViewModel : EditorViewModel
    {
        private string _sourceText;
        private IEditor _documentAccessor;
        private CompositeDisposable _disposables;
        private bool _isReadOnly;

        public TextEditorViewModel(ISourceFile file) : base(file)
        {
            Title = file.Name;

            this.WhenAnyValue(x => x.DocumentAccessor).Subscribe(accessor =>
            {
                _disposables?.Dispose();
                _disposables = null;

                if (accessor != null)
                {
                    _disposables = new CompositeDisposable
                    {
                        Observable.FromEventPattern<TextInputEventArgs>(accessor, nameof(accessor.TextEntered)).Subscribe(args =>
                        {
                            IsDirty = true;
                        }),
                        Observable.FromEventPattern<TooltipDataRequestEventArgs>(accessor, nameof(accessor.RequestTooltipContent)).Subscribe(args =>
                        {

                        })
                    };
                }
            });
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { this.RaiseAndSetIfChanged(ref _isReadOnly, value); }
        }

        public override void Close()
        {
            base.Close();

            _disposables.Dispose();
        }

        ~TextEditorViewModel()
        {
        }

        public string SourceText
        {
            get { return _sourceText; }
            set { this.RaiseAndSetIfChanged(ref _sourceText, value); }
        }

        public string FontFamily
        {
            get
            {
                switch (Platform.PlatformIdentifier)
                {
                    /*case Platforms.PlatformID.Win32NT:
                        return "Consolas";*/

                    default:
                        return "Source Code Pro";
                }
            }
        }

        public override async Task WaitForEditorToLoadAsync()
        {
            if (_documentAccessor == null)
            {
                await Task.Run(() =>
                {
                    while (_documentAccessor == null)
                    {
                        Task.Delay(50);
                    }
                });
            }
        }

        public IEditor DocumentAccessor
        {
            get { return _documentAccessor; }
            set { this.RaiseAndSetIfChanged(ref _documentAccessor, value); }
        }

        public override IEditor Editor => _documentAccessor;
    }
}
