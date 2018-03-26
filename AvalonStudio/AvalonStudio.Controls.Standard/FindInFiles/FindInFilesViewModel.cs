using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    class FindInFilesViewModel : ToolViewModel, IExtension
    {
        private string _searchTerm;
        private ObservableCollection<FindResultViewModel> _results;
        private bool _caseSensitive;
        private bool _wholeWords;
        private bool _regex;

        public FindInFilesViewModel()
        {
            Title = "Find Results";

            FindCommand = ReactiveCommand.Create(async () =>
            {
                Results = null;

                var service = IoC.Get<IFindInFilesService>();

                var results = service.Find(_searchTerm, CaseSensitive, WholeWords, Regex);

                Results = await Task.Run(()=>new ObservableCollection<FindResultViewModel>(results.Select(r=>new FindResultViewModel(r))));
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
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }        

        public bool CaseSensitive
        {
            get { return _caseSensitive; }
            set { this.RaiseAndSetIfChanged(ref _caseSensitive, value); }
        }        

        public bool WholeWords
        {
            get { return _wholeWords; }
            set { this.RaiseAndSetIfChanged(ref _wholeWords, value); }
        }        

        public bool Regex
        {
            get { return _regex; }
            set { this.RaiseAndSetIfChanged(ref _regex, value); }
        }

        public ReactiveCommand FindCommand { get; }
    }
}
