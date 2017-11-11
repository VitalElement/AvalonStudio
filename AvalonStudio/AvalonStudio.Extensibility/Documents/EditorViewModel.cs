using Avalonia.Input;
using AvalonStudio.Extensibility.Documents;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace AvalonStudio.Controls
{
    public abstract class EditorViewModel : DocumentTabViewModel, IFileDocumentTabViewModel
    {
        private bool _isDirty;
        private ISourceFile _sourceFile;
        private ITextDocument _documentAccessor;
        private CompositeDisposable _disposables;

        public EditorViewModel(ISourceFile file)
        {            
            _sourceFile = file;

            this.WhenAnyValue(x => x.DocumentAccessor).Subscribe(accessor =>
            {
                _disposables?.Dispose();
                _disposables = null;

                if(accessor != null)
                {
                    _disposables = new CompositeDisposable();

                    _disposables.Add(Observable.FromEventPattern<TextInputEventArgs>(accessor, nameof(accessor.TextEntered)).Subscribe(args =>
                    {
                        IsDirty = true;
                    }));
                }
            });
        }

        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                this.RaiseAndSetIfChanged(ref _isDirty, value);

                if (value && IsTemporary)
                {
                    IsTemporary = false;
                }
            }
        }

        public override void Save()
        {
            _documentAccessor?.Save();

            IsDirty = false;
        }

        public ISourceFile SourceFile
        {
            get { return _sourceFile; }
            set { this.RaiseAndSetIfChanged(ref _sourceFile, value); }
        }

        public ITextDocument DocumentAccessor
        {
            get { return _documentAccessor; }
            set { this.RaiseAndSetIfChanged(ref _documentAccessor, value); }
        }
    }
}
