using System;
using System.Collections.Generic;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls {
    public class FindInFileViewModel : ViewModel, IDisposable {
        private string _searchField;
        private bool _caseSensitive;
        private bool _matchWholeWord;

        private int _caretIndex;

        private List<int> _matches;
        private int _matchesListPosition;
        private bool _reachedEnd;

        public FindInFileViewModel(EditorModel editor, EditorViewModel viewModel) {
            Find = ReactiveCommand.Create();

            CaseSensitive = true;
            MatchWholeWord = false;

            Find.Subscribe(_ => {
                if (_matches == null || _reachedEnd) {
                    // create the search result list for this document

                    _matches = new List<int>();
                    var lines = viewModel.TextDocument.Text.Split('\n');

                    _caretIndex = 0;

                    foreach (var line in lines) {
                        int matchPosition;
                        if (MatchLine(line, out matchPosition)) {
                            _matches.Add(_caretIndex + matchPosition);
                        }

                        MoveCaretToNextLine(line);
                    }

                    viewModel.CaretIndex = _matches[0];
                    _matchesListPosition = 1;

                    _reachedEnd = _matches.Count == _matchesListPosition;
                } else {
                    if (_matches.Count == _matchesListPosition) {
                        _reachedEnd = true;
                        return;
                    }

                    viewModel.CaretIndex = _matches[_matchesListPosition];
                    _matchesListPosition++;
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
                } else {
                    if (line.ToLower().Contains(SearchField.ToLower())) {
                        matchPosition = 0;
                        return true;
                    }
                }
            }

            matchPosition = 0;
            return false;
        }

        public ReactiveCommand<object> Find { get; }

        public string SearchField {
            get { return _searchField; }
            set { this.RaiseAndSetIfChanged(ref _searchField, value); }
        }

        public bool CaseSensitive {
            get { return _caseSensitive; }
            set {
                this.RaiseAndSetIfChanged(ref _caseSensitive, value);
                // reset the search
                _reachedEnd = true;
            }
        }

        public bool MatchWholeWord {
            get { return _matchWholeWord; }
            set {
                this.RaiseAndSetIfChanged(ref _matchWholeWord, value);
                // reset the search
                _reachedEnd = true;
            }
        }
    }
}