using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    class FindInFilesViewModel : ToolViewModel, IExtension
    {
        private string _searchTerm;

        public FindInFilesViewModel()
        {
            Title = "Find Results";

            FindCommand = ReactiveCommand.Create(() =>
            {
                var service = IoC.Get<IFindInFilesService>();

                var results = service.Find(_searchTerm);

                Results = new ObservableCollection<FindResultViewModel>(results.Select(r=>new FindResultViewModel(r)));
            });
        }

        public override Location DefaultLocation => Location.Bottom;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        private ObservableCollection<FindResultViewModel> _results;

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

        public ReactiveCommand FindCommand { get; }
    }
}
