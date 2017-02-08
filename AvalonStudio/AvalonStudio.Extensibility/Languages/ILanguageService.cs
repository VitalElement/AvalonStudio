using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;
using AvalonStudio.Projects;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Indentation;
using AvalonStudio.TextEditor.Rendering;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Languages
{
	public interface ILanguageService : IExtension
	{
		IIndentationStrategy IndentationStrategy { get; }

		/// <summary>
		///     A description of the language supported by the service, i.e. C/C++
		/// </summary>
		string Title { get; }

		/// <summary>
		///     The base type that all Project templates for this language must inherit. This base class must implement
		///     IProjectTemplate.
		/// </summary>
		Type BaseTemplateType { get; }

		Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line, int column, List<UnsavedFile> unsavedFiles, string filter = "");

        IEnumerable<char> IntellisenseTriggerCharacters { get; }
        IEnumerable<char> IntellisenseSearchCharacters { get; }
        IEnumerable<char> IntellisenseCompleteCharacters { get; }

		Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

		IList<IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file);

		IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file);

		void RegisterSourceFile(IIntellisenseControl intellisenseControl, ICompletionAssistant completionAssistant, TextEditor.TextEditor editor, ISourceFile file, TextDocument textDocument);

		void UnregisterSourceFile(TextEditor.TextEditor editor, ISourceFile file);

		bool CanHandle(ISourceFile file);

        bool CanHandle(IProject project);

		int Format(TextDocument textDocument, uint offset, uint length, int cursor);

		int Comment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true);

		int UnComment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true);

        Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName);

		Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset);

		Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name);

        Task AnalyseProjectAsync(IProject project);
	}
}