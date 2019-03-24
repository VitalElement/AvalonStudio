using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    [Export(typeof(FindInFilesViewModel))]
    [ExportToolControl]
    [Shared]
    class FindInFilesViewModel : ToolViewModel, IActivatableExtension
    {
        private string _searchTerm;
        private ObservableCollection<FindResultViewModel> _results;
        private bool _caseSensitive;
        private bool _wholeWords;
        private bool _regex;
        private string _fileMask;
        private string _searchStats;

        public FindInFilesViewModel() : base("Find Results")
        {
            Title = "Find Results";

            FindCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                Results = null;
                SearchStats = null;

                var service = IoC.Get<IFindInFilesService>();

                var results = service.Find(_searchTerm, CaseSensitive, WholeWords, Regex, FileMask?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

                Results = await Task.Run(() => new ObservableCollection<FindResultViewModel>(results.Select(r => new FindResultViewModel(r))));

                SearchStats = $"{Results.Count} matches in {Results.GroupBy(r => r.FilePath).Count()} files";
            });
        }

        public override Location DefaultLocation => Location.Bottom;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
            
        }

        public ObservableCollection<FindResultViewModel> Results
        {
            get { return _results; }
            set { this.RaiseAndSetIfChanged(ref _results, value); }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                this.RaiseAndSetIfChanged(ref _searchTerm, value);
                SearchStats = null;
            }
        }

        public bool CaseSensitive
        {
            get { return _caseSensitive; }
            set
            {
                this.RaiseAndSetIfChanged(ref _caseSensitive, value);
                SearchStats = null;
            }
        }

        public bool WholeWords
        {
            get { return _wholeWords; }
            set
            {
                this.RaiseAndSetIfChanged(ref _wholeWords, value);
                SearchStats = null;
            }
        }

        public bool Regex
        {
            get { return _regex; }
            set
            {
                this.RaiseAndSetIfChanged(ref _regex, value);
                SearchStats = null;
            }
        }

        public string FileMask
        {
            get { return _fileMask; }
            set
            {
                this.RaiseAndSetIfChanged(ref _fileMask, value);
                SearchStats = null;
            }
        }

        public string SearchStats
        {
            get { return _searchStats; }
            set { this.RaiseAndSetIfChanged(ref _searchStats, value); }
        }

        public ReactiveCommand<Unit, Unit> FindCommand { get; }
    }
}
