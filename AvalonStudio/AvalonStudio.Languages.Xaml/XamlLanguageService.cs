using Avalonia.Ide.CompletionEngine;
using Avalonia.Ide.CompletionEngine.AssemblyMetadata;
using Avalonia.Ide.CompletionEngine.SrmMetadataProvider;
using Avalonia.Threading;
using AvalonStudio.Documents;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.Xaml
{
    [ExportLanguageService(ContentCapabilities.Xaml)]
    internal class XamlLanguageService : XmlLanguageService
    {
        public override string LanguageId => "xaml";

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

        private static CodeCompletionKind FromAvaloniaCompletionKind(CompletionKind kind)
        {
            CodeCompletionKind result = CodeCompletionKind.None;

            switch (kind)
            {
                case CompletionKind.Class:
                    return CodeCompletionKind.ClassPublic;

                case CompletionKind.Enum:
                    return CodeCompletionKind.EnumMemberPublic;

                case CompletionKind.Property:
                    return CodeCompletionKind.PropertyPublic;

                case CompletionKind.Namespace:
                    return CodeCompletionKind.NamespacePublic;
            }

            return result;
        }

        public override async Task<CodeCompletionResults> CodeCompleteAtAsync(IEditor editor, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
        {
            var results = new CodeCompletionResults();

            string text = string.Empty;

            CreateMetaDataIfRequired(editor.SourceFile.Project.Solution.StartupProject.Executable);

            if (metaData != null)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    text = editor.Document.Text;
                });

                var completionSet = engine.GetCompletions(metaData, text, index);

                if (completionSet != null)
                {
                    foreach (var completion in completionSet.Completions)
                    {
                        results.Completions.Add(new CodeCompletionData(completion.DisplayText, completion.DisplayText, completion.InsertText, completion.RecommendedCursorOffset)
                        {
                            BriefComment = completion.Description,
                            Kind = FromAvaloniaCompletionKind(completion.Kind),
                            RecommendImmediateSuggestions = completion.InsertText.Contains("=") || completion.InsertText.EndsWith('.')
                        });
                    }
                }

                results.Contexts = CompletionContext.AnyType;
            }

            return await Task.FromResult(results);
        }

        private static CompletionEngine engine = null;
        private static Metadata metaData = null;

        private void CreateMetaDataIfRequired(string executable)
        {
            if (metaData == null && File.Exists(executable))
            {
                metaData = new MetadataReader(new SrmMetadataProvider()).GetForTargetAssembly(executable);
            }
        }

        public override void RegisterSourceFile(ITextEditor editor)
        {
            if (engine == null)
            {
                engine = new CompletionEngine();
            }

            CreateMetaDataIfRequired(editor.SourceFile.Project.Solution.StartupProject.Executable);
        }

        public override void UnregisterSourceFile(IEditor editor)
        {

        }
    }
}
