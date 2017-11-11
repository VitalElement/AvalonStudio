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

        public XamlEditorViewModel(ISourceFile file)
        {
            _textEditor = new TextEditorViewModel(file);
        }

        public string SouceText => _textEditor.SourceText;

        public TextEditorViewModel CodeEditor => _textEditor;

        public ISourceFile SourceFile => ((IFileDocumentTabViewModel)_textEditor).SourceFile;

        public bool IsDirty
        {
            get => ((IFileDocumentTabViewModel)_textEditor).IsDirty;
            set
            {
                ((IFileDocumentTabViewModel)_textEditor).IsDirty = value;
                this.RaisePropertyChanged();
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
