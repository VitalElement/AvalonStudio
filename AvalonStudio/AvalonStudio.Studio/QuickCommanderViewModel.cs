using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Projects;
using AvalonStudio.Studio;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Studio
{
    public class QuickCommanderViewModel : ReactiveObject
    {
        private bool _isVisible;
        private bool _resultsVisible;
        private QuickCommander _instance;
        private bool _isFocused;
        private string _commandQuery;
        private ObservableCollection<SearchResultViewModel> _results;
        private SearchResultViewModel _selectedResult;
        private int _selectedIndex;

        public QuickCommanderViewModel()
        {
            _isVisible = false;
            _results = new ObservableCollection<SearchResultViewModel>();

            this.WhenAnyValue(x => x.CommandQuery).Throttle(TimeSpan.FromMilliseconds(165)).Subscribe(async query => await ProcessQuery(query));

            this.WhenAnyValue(x => x.IsVisible).Subscribe(visible =>
            {
                if (!visible)
                {
                    CommandQuery = string.Empty;
                    _results.Clear();
                }
            });

            UpCommand = ReactiveCommand.Create(() =>
            {
                if (Results?.Count > 0 && SelectedIndex > 0)
                {
                    SelectedIndex--;
                }
            });

            DownCommand = ReactiveCommand.Create(() =>
            {
                if (Results?.Count > 0 && SelectedIndex < Results?.Count - 1)
                {
                    SelectedIndex++;
                }
            });

            EnterCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var selectedResult = SelectedResult.Model;
                IsVisible = false;
                await IoC.Get<IStudio>().OpenDocumentAsync(selectedResult, 1);
            });

            EscapeCommand = ReactiveCommand.Create(() =>
            {
                IsVisible = false;
            });
        }

        private async Task ProcessQuery(string query)
        {
            if (IoC.Get<IStudio>().CurrentSolution == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(query))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    ResultsVisible = false;
                    _results.Clear();
                });
            }
            else
            {
                query = query.ToLower();

                var list = new List<SearchResultViewModel>();

                await Task.Run(() =>
                {
                    foreach (var project in IoC.Get<IStudio>().CurrentSolution.Projects.ToList())
                    {
                        project.SourceFiles?.Select(sf =>
                        {
                            var match = FuzzyMatch.StringMatch(sf.Project.Location.MakeRelativePath(sf.Location), query, null);

                            if (match != null)
                            {
                                return new Tuple<bool, int, string, ISourceFile, List<FuzzyMatch.Range>>(true, match.matchQuality, match.label, sf, match.stringRanges);
                            }

                            return new Tuple<bool, int, string, ISourceFile, List<FuzzyMatch.Range>>(false, 0, "", sf, null);
                        }).Where(tp => tp.Item1).Select(tp =>
                        {
                            var spans = new List<FormattedTextStyleSpan>();
                            int index = 0;

                            foreach (var range in tp.Item5)
                            {
                                if (range.matched)
                                {
                                    var span = new FormattedTextStyleSpan(index + tp.Item4.Project.Name.Length + 1, range.text.Length, ColorTheme.CurrentTheme.AccentLow);
                                    spans.Add(span);
                                }

                                index += range.text.Length;
                            }

                            list.InsertSorted(new SearchResultViewModel(tp.Item4) { Priority = tp.Item2, Spans = spans });

                            return tp;
                        }).ToList();
                    }
                });

                Dispatcher.UIThread.Post(() =>
                {
                    _results.Clear();

                    foreach (var result in list)
                    {
                        _results.Add(result);
                    }

                    ResultsVisible = _results.Count > 0;

                    SelectedIndex = -1;
                });
            }
        }

        public ReactiveCommand<Unit, Unit> UpCommand { get; }

        public ReactiveCommand<Unit, Unit> DownCommand { get; }

        public ReactiveCommand<Unit, Unit> EscapeCommand { get; }

        public ReactiveCommand<Unit, Unit> EnterCommand { get; }

        public SearchResultViewModel SelectedResult
        {
            get { return _selectedResult; }
            set { this.RaiseAndSetIfChanged(ref _selectedResult, value); }
        }

        public ObservableCollection<SearchResultViewModel> Results
        {
            get { return _results; }
            set
            {
                this.RaiseAndSetIfChanged(ref _results, value);
                ResultsVisible = (value.Count > 0);
            }
        }

        public bool ResultsVisible
        {
            get { return _resultsVisible; }
            set { this.RaiseAndSetIfChanged(ref _resultsVisible, value); }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { this.RaiseAndSetIfChanged(ref _isVisible, value); }
        }

        public bool IsFocused
        {
            get { return _isFocused; }
            set { this.RaiseAndSetIfChanged(ref _isFocused, value); }
        }

        public string CommandQuery
        {
            get { return _commandQuery; }
            set { this.RaiseAndSetIfChanged(ref _commandQuery, value); }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        public void AttachControl(QuickCommander instance)
        {
            _instance = instance;
        }
    }
}
