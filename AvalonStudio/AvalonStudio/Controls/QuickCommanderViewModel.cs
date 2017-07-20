using Avalonia.Threading;
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

            this.WhenAnyValue(x => x.CommandQuery).Throttle(TimeSpan.FromMilliseconds(200)).Subscribe(async query => await ProcessQuery(query));

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
            if (_shell.CurrentSolution == null)
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
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _results.Clear();
                });

                query = query.ToLower();

                await Task.Run(() =>
                {
                    foreach (var project in _shell.CurrentSolution.Projects)
                    {
                        var newResults = project.SourceFiles.Where(f => f.Project.Location.MakeRelativePath(f.Location).ToLower().Contains(query)).Select(r => new SearchResultViewModel(r));

                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            foreach (var result in newResults)
                            {
                                _results.Add(result);
                            }

                            ResultsVisible = _results.Count > 0;
                        });
                    }
                });

                SelectedIndex = -1;
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
