using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using ReactiveUI;
using System.IO;
using System.Reactive;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    class FindResultViewModel : ViewModel<FindResult>
    {
        public FindResultViewModel(FindResult model) : base(model)
        {
            File = Path.GetFileName(FilePath);

            OpenCommand = ReactiveCommand.Create(() =>
            {
                var studio = IoC.Get<IStudio>();

                studio.OpenDocumentAsync(Model.File, Model.LineNumber, focus: true, selectLine: true);
            });
        }

        public string File { get; private set; }

        public string FilePath => Model.File.Location;

        public int LineNumber => Model.LineNumber;

        public string LineText => Model.LineText;

        public ReactiveCommand<Unit, Unit> OpenCommand { get; }
    }
}
