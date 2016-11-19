using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using AvalonStudio.Languages;
using AvalonStudio.Languages.ViewModels;
using AvalonStudio.MVVM;
using AvalonStudio.TextEditor.Document;
using ReactiveUI;

namespace AvalonStudio.Controls {
    public class FindInFileViewModel : ViewModel, IDisposable {
        private EditorModel _editor;
        private EditorViewModel _editorViewModel;

        private Thickness _position;

        private string _searchField;
        private bool _caseSensitive;
        private bool _matchWholeWord;

        public FindInFileViewModel(EditorModel editor, EditorViewModel viewModel) {
            _editor = editor;
            _editorViewModel = viewModel;

            Position = new Thickness(300, 50);
            Find = ReactiveCommand.Create();

            CaseSensitive = true;
            MatchWholeWord = false;

            Find.Subscribe(_ => {
                var lines = viewModel.TextDocument.Text.Split('\n');

                var caretIndex = 0;

                foreach (var line in lines) {
                    if (MatchLine(line)) {
                        viewModel.CaretIndex = caretIndex;
                    }

                    if (line == "\r")
                        // this is for new empty lines
                        caretIndex += 2;
                    else
                        // standard text line
                        caretIndex += line.Length + 1;
                }
            });
        }

        public bool MatchLine(string line) {
            if (!_caseSensitive) {
                if (line.ToLower().Contains(SearchField.ToLower()))
                    return true;
            }
            else {
                if (line.Contains(SearchField))
                    return true;
            }

            return false;
        }

        public Thickness Position
        {
            get { return _position; }
            set { this.RaiseAndSetIfChanged(ref _position, value); }
        }

        public void Dispose() {
            _editor = null;
            _editorViewModel = null;
        }

        public ReactiveCommand<object> Find { get; }

        public string SearchField
        {
            get { return _searchField; }
            set { this.RaiseAndSetIfChanged(ref _searchField, value); }
        }

        public bool CaseSensitive
        {
            get { return _caseSensitive; }
            set { this.RaiseAndSetIfChanged(ref _caseSensitive, value); }
        }

        public bool MatchWholeWord
        {
            get { return _matchWholeWord; }
            set { this.RaiseAndSetIfChanged(ref _matchWholeWord, value); }
        }
    }
}