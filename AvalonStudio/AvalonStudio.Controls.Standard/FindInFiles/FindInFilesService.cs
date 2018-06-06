using AvaloniaEdit.Document;
using AvaloniaEdit.Search;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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


    public class FindInFilesService : IFindInFilesService, IActivatableExtension
    {
        private IEnumerable<FindResult> GetResults(ISearchStrategy strategy, ISourceFile file)
        {
            using (var fileStream = file.OpenText())
            {
                using (var reader = new StreamReader(fileStream))
                {
                    var document = new TextDocument(reader.ReadToEnd());

                    var results = strategy.FindAll(document, 0, document.TextLength);

                    foreach (var result in results.GroupBy(sr => document.GetLineByOffset(sr.Offset).LineNumber).Select(group => group.First()))
                    {
                        var line = document.GetLineByOffset(result.Offset);

                        yield return new FindResult(file, result.Offset, result.Length, line.LineNumber, line.Offset, document.GetText(line));
                    }
                }
            }
        }

        public IEnumerable<FindResult> Find(string searchString, bool caseSensitive, bool wholeWords, bool regex, string[] fileMasks)
        {
            var shell = IoC.Get<IShell>();

            var searchStrategy = SearchStrategyFactory.Create(searchString, !caseSensitive, wholeWords, regex ? SearchMode.RegEx : SearchMode.Normal);

            if (shell.CurrentSolution == null)
            {
                return Enumerable.Empty<FindResult>();
            }

            var files = shell.CurrentSolution.Projects.SelectMany(p => p.SourceFiles);

            if (fileMasks != null && fileMasks.Count() > 0)
            {
                // Construct corresponding regular expression. Note Regex.Escape!
                var options = RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase;

                var patterns = fileMasks
                .Select(f => new Regex("^" + Regex.Escape(f).Replace("\\*", ".*") + "$", options))
                .ToArray();                

                files = files.Where(f => patterns.Any(p=>p.IsMatch(f.FilePath)));
            }

            return files.SelectMany(f => GetResults(searchStrategy, f));
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
