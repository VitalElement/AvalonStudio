using Avalonia.Ide.CompletionEngine;
using Avalonia.Ide.CompletionEngine.AssemblyMetadata;
using Avalonia.Ide.CompletionEngine.SrmMetadataProvider;
using AvaloniaEdit.Document;
using AvalonStudio.Projects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.Xaml
{
    class XamlLanguageService : XmlLanguageService
    {
        public override string Title => "XAML";

        public override string LanguageId => "xaml";

        public override bool CanHandle(ISourceFile file)
        {
            var result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".xaml":
                case ".paml":
                    result = true;
                    break;
            }

            return result;
        }

        public override bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            bool result = false;

            if (currentChar == '<' || currentChar == ' ' || currentChar == '.')
            {
                return true;
            }

            return result;
        }

        public override Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
        {
            var results = new CodeCompletionResults();

            if (unsavedFiles.Count > 0)
            {
                var currentUnsavedFile = unsavedFiles.FirstOrDefault(f => f.FileName == sourceFile.FilePath);
                var currentFileConts = currentUnsavedFile.Contents;

                var completionSet = engine.GetCompletions(metaData, currentFileConts, index);

                if (completionSet != null)
                {
                    foreach (var completion in completionSet.Completions)
                    {
                        results.Completions.Add(new CodeCompletionData(completion.DisplayText, completion.InsertText, completion.RecommendedCursorOffset)
                        {
                            BriefComment = completion.Description,
                            Kind = CodeCompletionKind.PropertyPublic,
                            RecommendImmediateSuggestions = completion.InsertText.Contains("=") || completion.InsertText.EndsWith('.')
                        });
                    }
                }
            }

            results.Contexts = CompletionContext.AnyType;

            return Task.FromResult(results);
        }

        private static CompletionEngine engine = null;
        private static Metadata metaData = null;

        public override void RegisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file, TextDocument textDocument)
        {
            if (engine == null)
            {
                engine = new CompletionEngine();
            }

            if (metaData == null)
            {
                metaData = new MetadataReader(new SrmMetadataProvider()).GetForTargetAssembly(file.Project.Solution.StartupProject.Executable);
            }
        }

        public override void UnregisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file)
        {

        }
    }
}
