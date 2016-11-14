using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using AvalonStudio.Languages;
using AvalonStudio.Languages.ViewModels;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public class FindInFileViewModel : ViewModel, IDisposable
    {
        private EditorModel editor;
        private EditorViewModel editorViewModel;

        private Thickness position;

        public FindInFileViewModel(EditorModel editor, EditorViewModel viewModel)
        {
            editorViewModel = viewModel;
            this.editor = editor;
            IsOpen = true;
        }

        public Thickness Position
        {
            get { return position; }
            set { this.RaiseAndSetIfChanged(ref position, value); }
        }

        public void Dispose()
        {
            editor = null;
            editorViewModel = null;
        }

        private bool isOpen;
        public bool IsOpen
        {
            get { return isOpen; }
            set { this.RaiseAndSetIfChanged(ref isOpen, value); }
        }
    }
}