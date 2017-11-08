using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Indentation;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using System.IO;
using Avalonia.Ide.CompletionEngine;
using Avalonia.Ide.CompletionEngine.AssemblyMetadata;
using Avalonia.Ide.CompletionEngine.SrmMetadataProvider;
using System.Reflection;
using System.Linq;

namespace AvalonStudio.Languages.Xaml
{
    class XamlLanguageService : ILanguageService
    {
        public IIndentationStrategy IndentationStrategy => null;

        public string Title => "XAML";

        public string LanguageId => "xaml";

        public Type BaseTemplateType => null;

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators => new Dictionary<string, Func<string, string>>();

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables => new Dictionary<string, Func<int, int, int, string>>();

        public IEnumerable<char> IntellisenseSearchCharacters => new[]
        {
            '(', ')', '.', ':', '-', '<', '>', '[', ']', ';', '"', '#', ','
        };

        public IEnumerable<char> IntellisenseCompleteCharacters => new[]
        {
            ',', '.', ':', ';', '-', ' ', '(', ')', '[', ']', '<', '>', '=', '+', '*', '/', '%', '|', '&', '!', '^'
        };

        public IObservable<TextSegmentCollection<Diagnostic>> Diagnostics => null;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public bool CanHandle(ISourceFile file)
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

        public bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            bool result = false;

            if(currentChar == '<')
            {
                return true;
            }

            return result;
        }

        public Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
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
                        results.Completions.Add(new CodeCompletionData { Suggestion = completion.DisplayText, BriefComment = completion.Description, Kind = CodeCompletionKind.PropertyPublic });
                    }
                }
            }

            results.Contexts = CompletionContext.AnyType;

            return Task.FromResult(results);
        }

        public int Comment(ISourceFile file, TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            return caret;
        }

        public int Format(ISourceFile file, TextDocument textDocument, uint offset, uint length, int cursor)
        {
            return cursor;
        }

        public Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            return Task.FromResult<Symbol>(null);
        }

        public Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            return Task.FromResult<List<Symbol>>(null);
        }

        public bool IsValidIdentifierCharacter(char data)
        {
            return char.IsLetterOrDigit(data);
        }

        private static CompletionEngine engine = null;
        private static Metadata metaData = null;

        public void RegisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file, TextDocument textDocument)
        {
            engine = new CompletionEngine();
            
            metaData = new MetadataReader(new SrmMetadataProvider()).GetForTargetAssembly(file.Project.Solution.StartupProject.Executable);
        }
        
        public void UnregisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file)
        {
            
        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, TextDocument textDocument, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            return Task.FromResult(new CodeAnalysisResults());
        }

        public Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            return Task.FromResult<SignatureHelp>(null);
        }

        public int UnComment(ISourceFile file, TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            return caret;
        }
    }
}
