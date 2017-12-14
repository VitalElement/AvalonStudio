using Avalonia.Ide.CompletionEngine;
using Avalonia.Ide.CompletionEngine.AssemblyMetadata;
using Avalonia.Ide.CompletionEngine.SrmMetadataProvider;
using AvalonStudio.Documents;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace AvalonStudio.Languages.Xaml
{
    class XamlLanguageService : XmlLanguageService
    {
        public override string Title => "XAML";

        public override string LanguageId => "xaml";

        public override string Identifier => "XAML";

        public override bool CanHandle(IEditor editor)
        {
            var result = false;

            switch (Path.GetExtension(editor.SourceFile.Location))
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

        public override async Task<CodeCompletionResults> CodeCompleteAtAsync(IEditor editor, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
        {
            var results = new CodeCompletionResults();

            string text = string.Empty;

            await Dispatcher.UIThread.InvokeTaskAsync(()=> 
            {
                text = editor.Document.Text;
            });

            var completionSet = engine.GetCompletions(metaData, text, index);

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

            results.Contexts = CompletionContext.AnyType;

            return await Task.FromResult(results);
        }

        private static CompletionEngine engine = null;
        private static Metadata metaData = null;

        public override void RegisterSourceFile(IEditor editor)
        {
            if (engine == null)
            {
                engine = new CompletionEngine();
            }

            if (metaData == null)
            {
                metaData = new MetadataReader(new SrmMetadataProvider()).GetForTargetAssembly(editor.SourceFile.Project.Solution.StartupProject.Executable);
            }
        }

        public override void UnregisterSourceFile(IEditor editor)
        {

        }
    }
}
