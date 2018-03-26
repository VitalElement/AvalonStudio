using AvaloniaEdit.Document;
using AvaloniaEdit.Search;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    public class FindResult
    {
        public FindResult(ISourceFile file, int offset, int length, int lineNumber, int lineOffset, string lineText)
        {
            File = file;
            Offset = offset;
            Length = length;
            LineOffset = lineOffset;
            LineNumber = lineNumber;

            LineText = lineText.Trim();
        }

        public ISourceFile File { get; }
        public int Offset { get; }
        public int Length { get; }

        public int LineOffset { get; }
        public int LineNumber { get; }
        public string LineText { get; }
    }


    public class FindInFilesService : IFindInFilesService, IExtension
    {
        private IEnumerable<FindResult> GetResults(ISearchStrategy strategy, ISourceFile file)
        {
            using (var fileStream = file.OpenText())
            {
                using (var reader = new StreamReader(fileStream))
                {
                    var document = new TextDocument(reader.ReadToEnd());

                    var results = strategy.FindAll(document, 0, document.TextLength);

                    foreach (var result in results)
                    {
                        var line = document.GetLineByOffset(result.Offset);

                        yield return new FindResult(file, result.Offset, result.Length, line.LineNumber, line.Offset, document.GetText(line));
                    }
                }
            }
        }

        public IEnumerable<FindResult> Find(string searchString)
        {
            var shell = IoC.Get<IShell>();

            var searchStrategy = SearchStrategyFactory.Create(searchString, true, false, SearchMode.Normal);

            return shell.CurrentSolution.Projects.SelectMany(p => p.SourceFiles).SelectMany(f => GetResults(searchStrategy, f));
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<IFindInFilesService>(this);
        }

        public void Activation()
        {
        }
    }
}
