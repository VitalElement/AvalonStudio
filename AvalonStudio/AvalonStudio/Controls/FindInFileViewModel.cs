using System;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls {
    public class FindInFileViewModel : ViewModel, IDisposable {
        private EditorModel _editor;
        private EditorViewModel _editorViewModel;

        private string _searchField;
        private bool _caseSensitive;
        private bool _matchWholeWord;

        private int _caretIndex;
        private int _lastLine;

        public FindInFileViewModel(EditorModel editor, EditorViewModel viewModel) {
            _editor = editor;
            _editorViewModel = viewModel;

            Find = ReactiveCommand.Create();

            CaseSensitive = true;
            MatchWholeWord = false;

            Find.Subscribe(_ => {
                var lines = viewModel.TextDocument.Text.Split('\n');

                _caretIndex = 0;

                for (var i = 0; i < lines.Length; i++) {
                    var line = lines[i];

                    int matchPosition;
                    if (MatchLine(line, out matchPosition)) {
                        viewModel.CaretIndex = _caretIndex + matchPosition;
                        _lastLine++;
                        break;
                    }

                    MoveCaretToNextLine(line);
                }
            });
        }

        public void MoveCaretToNextLine(string line) {
            if (line == "\r")
                // this is for new empty lines
                _caretIndex += 2;
            else
                // standard text line
                _caretIndex += line.Length + 1;
        }

        public bool ContainsWord(string line) {
            var words = line.Split(' ');
                
            foreach (var word in words) {
                if (CaseSensitive) {
                    if (word == SearchField)
                        return true;
                } else {
                    if (string.Equals(word, SearchField, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        public bool MatchLine(string line, out int matchPosition) {
            if (MatchWholeWord) {
                if (ContainsWord(line)) {
                    matchPosition = 0;
                    return true;
                }
            } else {
                if (CaseSensitive) {
                    if (line.Contains(SearchField)) {
                        matchPosition = 0;
                        return true;
                    }
                }
                else {
                    if (line.ToLower().Contains(SearchField.ToLower())) {
                        matchPosition = 0;
                        return true;
                    }
                }
            }

            matchPosition = 0;
            return false;
        }

        public void Dispose() {
            _editor = null;
            _editorViewModel = null;
        }

        public ReactiveCommand<object> Find { get; }

        public string SearchField {
            get { return _searchField; }
            set { this.RaiseAndSetIfChanged(ref _searchField, value); }
        }

        public bool CaseSensitive {
            get { return _caseSensitive; }
            set { this.RaiseAndSetIfChanged(ref _caseSensitive, value); }
        }

        public bool MatchWholeWord {
            get { return _matchWholeWord; }
            set { this.RaiseAndSetIfChanged(ref _matchWholeWord, value); }
        }
    }
}