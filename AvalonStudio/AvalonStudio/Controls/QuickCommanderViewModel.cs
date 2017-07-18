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
        private IEnumerable<SearchResultViewModel> _results;
        private SearchResultViewModel _selectedResult;

        public QuickCommanderViewModel()
        {
            _isVisible = false;

            this.WhenAnyValue(x => x.CommandQuery).Throttle(TimeSpan.FromMilliseconds(200)).Subscribe(query => ProcessQuery(query));

            this.WhenAnyValue(x => x.SelectedResult).OfType<SearchResultViewModel>().Subscribe(result =>
            {
                Dispatcher.UIThread.InvokeAsync(async () => { await _shell.OpenDocument(result.Model, 1); });
            });

            _shell = IoC.Get<ShellViewModel>();
        }        

        private void ProcessQuery(string query)
        {
            if (_shell.CurrentSolution == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(query))
            {
                Results = null;
            }
            else
            {
                var results = new List<SearchResultViewModel>();

                foreach (var project in _shell.CurrentSolution.Projects)
                {
                    results.AddRange(project.SourceFiles.Where(f => f.Project.Location.MakeRelativePath(f.Location).Contains(query)).Select(r => new SearchResultViewModel(r)));
                }

                Results = results;
            }
        }

        public SearchResultViewModel SelectedResult
        {
            get { return _selectedResult; }
            set { this.RaiseAndSetIfChanged(ref _selectedResult, value); }
        }


        public IEnumerable<SearchResultViewModel> Results
        {
            get { return _results; }
            set
            {
                this.RaiseAndSetIfChanged(ref _results, value);
                ResultsVisible = (value?.Count() > 0);
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
            set
            {
                this.RaiseAndSetIfChanged(ref _isVisible, value);

                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    IsFocused = value;
                });
                if (value)
                {
                    //_instance?.Focus();
                }
            }
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

        public void AttachControl(QuickCommander instance)
        {
            _instance = instance;
        }
    }
}
