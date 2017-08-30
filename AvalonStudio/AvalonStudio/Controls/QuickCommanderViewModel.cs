﻿using Avalonia.Threading;
using AvalonStudio.Extensibility;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Linq;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvalonStudio.Extensibility.Utils;
using Avalonia.Media;

namespace AvalonStudio.Controls
{
    public class QuickCommanderViewModel : ReactiveObject
    {
        private bool _isVisible;
        private bool _resultsVisible;
        private QuickCommander _instance;
        private bool _isFocused;
        private string _commandQuery;
        private ShellViewModel _shell;
        private ObservableCollection<SearchResultViewModel> _results;
        private SearchResultViewModel _selectedResult;
        private int _selectedIndex;

        public QuickCommanderViewModel()
        {
            _isVisible = false;
            _results = new ObservableCollection<SearchResultViewModel>();

            this.WhenAnyValue(x => x.CommandQuery).Throttle(TimeSpan.FromMilliseconds(165)).Subscribe(async query => await ProcessQuery(query));

            this.WhenAnyValue(x => x.SelectedResult).OfType<SearchResultViewModel>().Subscribe(result =>
            {
                Dispatcher.UIThread.InvokeAsync(async () => { await _shell.OpenDocument(result.Model, 1, focus: false); });
            });

            this.WhenAnyValue(x => x.IsVisible).Subscribe(visible =>
            {
                if (!visible)
                {
                    CommandQuery = string.Empty;
                    _results.Clear();
                }
            }); 

            UpCommand = ReactiveCommand.Create();
            UpCommand.Subscribe(_ =>
            {
                if (Results?.Count > 0 && SelectedIndex > 0)
                {
                    SelectedIndex--;
                }
            });

            DownCommand = ReactiveCommand.Create();
            DownCommand.Subscribe(_ =>
            {
                if (Results?.Count > 0 && SelectedIndex < Results?.Count - 1)
                {
                    SelectedIndex++;
                }
            });

            EnterCommand = ReactiveCommand.Create();
            EnterCommand.Subscribe(_ =>
            {
                IsVisible = false;
            });

            EscapeCommand = ReactiveCommand.Create();
            EscapeCommand.Subscribe(_ =>
            {
                if (_shell.DocumentTabs.TemporaryDocument != null)
                {
                    if (_shell.DocumentTabs.TemporaryDocument.SourceFile == SelectedResult?.Model)
                    {
                        _shell.RemoveDocument(_shell.DocumentTabs.TemporaryDocument);
                    }
                }
                
                IsVisible = false;
            });

            _shell = IoC.Get<ShellViewModel>();
        }        

        private async Task ProcessQuery(string query)
        {
            if (_shell?.CurrentSolution == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(query))
            {
                Dispatcher.UIThread.InvokeAsync(() =>
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
                    foreach (var project in _shell.CurrentSolution.Projects)
                    {
                        project.SourceFiles.Select(sf =>
                        {
                            //var match = FuzzyMatch.fuzzy_match(query, sf.Project.Location.MakeRelativePath(sf.Location), out int score, out string format);
                            var match = FuzzyMatch.StringMatch(sf.Project.Location.MakeRelativePath(sf.Location), query, null);
                            if(match != null)
                            {
                                return new Tuple<bool, int, string, ISourceFile, List<FuzzyMatch.Range>>(true, match.matchQuality, match.label, sf, match.stringRanges);
                            }

                            return new Tuple<bool, int, string, ISourceFile, List<FuzzyMatch.Range>>(false, 0, "", sf, null);
                        }).Where(tp => tp.Item1).Select(tp =>
                        {
                            List<Avalonia.Media.FormattedTextStyleSpan> list_spans = new List<FormattedTextStyleSpan>();
                            int index_start = 0;
                            foreach(var range in tp.Item5)
                            {
                                if(range.matched)
                                {
                                    Avalonia.Media.FormattedTextStyleSpan span = new FormattedTextStyleSpan(index_start+tp.Item4.Project.Name.Length+1, range.text.Length+1, Brushes.Red);
                                    list_spans.Add(span);
                                }

                                index_start += range.text.Length;
                            }

                            list.InsertSorted(new SearchResultViewModel(tp.Item4) { Priority = tp.Item2, Spans = list_spans });
                            return tp;
                        }).ToList();
                    }
                });

                Dispatcher.UIThread.InvokeAsync(() =>
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

        public ReactiveCommand<object> UpCommand { get; }

        public ReactiveCommand<object> DownCommand { get; }

        public ReactiveCommand<object> EscapeCommand { get; }

        public ReactiveCommand<object> EnterCommand { get; }

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
