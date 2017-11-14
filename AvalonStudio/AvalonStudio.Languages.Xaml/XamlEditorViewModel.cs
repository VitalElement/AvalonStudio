using Avalonia.Controls;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorViewModel : ReactiveObject, IFileDocumentTabViewModel
    {
        private TextEditorViewModel _textEditor;
        private bool _isDirty;

        public XamlEditorViewModel(ISourceFile file)
        {
            _textEditor = new TextEditorViewModel(file);

            _textEditor.ObservableForProperty(te => te.IsDirty).Subscribe(value => IsDirty = value.Value);
        }

        public string SouceText => _textEditor.SourceText;

        public TextEditorViewModel CodeEditor => _textEditor;

        public ISourceFile SourceFile => ((IFileDocumentTabViewModel)_textEditor).SourceFile;

        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                this.RaiseAndSetIfChanged(ref _isDirty, value);
            }
        }

        public string Title { get => ((IFileDocumentTabViewModel)_textEditor).Title; set => ((IFileDocumentTabViewModel)_textEditor).Title = value; }        

        public void Close()
        {
            IoC.Get<IShell>().RemoveDocument(this);
        }

        public void Save()
        {
            ((IFileDocumentTabViewModel)_textEditor).Save();
        }
    }
}
