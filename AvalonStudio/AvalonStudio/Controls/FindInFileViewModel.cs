using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using AvalonStudio.Languages;
using AvalonStudio.Languages.ViewModels;
using AvalonStudio.MVVM;
using AvalonStudio.TextEditor.Document;
using ReactiveUI;

namespace AvalonStudio.Controls {
    public class FindInFileViewModel : ViewModel, IDisposable {
        private EditorModel editor;
        private EditorViewModel editorViewModel;

        private Thickness position;

        public FindInFileViewModel(EditorModel editor, EditorViewModel viewModel) {
            editorViewModel = viewModel;

            Find = ReactiveCommand.Create();

            Find.Subscribe(_ => {
                var lines = viewModel.TextDocument.Text.Split('\n');

                var caretIndex = 0;

                foreach (var line in lines) {
                    if (line.Contains(StringValue)) {
                        viewModel.CaretIndex = caretIndex;
                    }

                    if (line == "\r")
                        caretIndex += 2;
                    else
                        caretIndex += line.Length + 1;
                }
            });

            this.editor = editor;

            IsOpen = true;
            Position = new Thickness(300, 50);
        }

        public Thickness Position
        {
            get { return position; }
            set { this.RaiseAndSetIfChanged(ref position, value); }
        }

        public void Dispose() {
            editor = null;
            editorViewModel = null;
        }

        private bool isOpen;
        private string _stringValue;

        public bool IsOpen
        {
            get { return isOpen; }
            set { this.RaiseAndSetIfChanged(ref isOpen, value); }
        }

        public ReactiveCommand<object> Find { get; }

        public string StringValue
        {
            get { return _stringValue; }
            set { this.RaiseAndSetIfChanged(ref _stringValue, value); }
        }
    }
}