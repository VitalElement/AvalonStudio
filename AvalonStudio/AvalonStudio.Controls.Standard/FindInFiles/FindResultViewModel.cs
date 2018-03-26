using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System.IO;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    class FindResultViewModel : ViewModel<FindResult>
    {
        public FindResultViewModel(FindResult model) : base(model)
        {
            File = Path.GetFileName(FilePath);

            OpenCommand = ReactiveCommand.Create(() =>
            {
                var shell =IoC.Get<IShell>();

                shell.OpenDocumentAsync(Model.File, Model.LineNumber, focus: true, selectLine: true);
            });
        }

        public string File { get; private set; }

        public string FilePath => Model.File.Location;

        public int LineNumber => Model.LineNumber;

        public string LineText => Model.LineText;

        public ReactiveCommand OpenCommand { get; }
    }
}
