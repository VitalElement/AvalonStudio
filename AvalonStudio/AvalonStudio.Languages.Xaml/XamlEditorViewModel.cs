using Avalonia.Controls;
using AvalonStudio.Controls;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages.Xaml
{
    class XamlEditorViewModel : IFileDocumentTabViewModel
    {
        public ISourceFile File { get; set; }

        public bool IsDirty { get; set; }

        public string Title { get; set; }

        public ReactiveCommand CloseCommand => null;

        public bool IsTemporary { get; set; }

        public bool IsVisible { get; set; }

        public bool IsSelected { get; set; }

        public Dock Dock { get; set; }

        public void OnClose()
        {
        }
    }
}
