using AvalonStudio.Controls.Editor.ContextActions;
using AvalonStudio.Documents;
using AvalonStudio.Projects.OmniSharp.Roslyn;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvalonStudio.Languages.CSharp
{
    class RoslynContextActionProvider : IContextActionProvider
    {
        private static readonly ImmutableArray<string> ExcludedRefactoringProviders = ImmutableArray.Create("ExtractInterface", "GenerateOverrides");

        private Microsoft.CodeAnalysis.Workspace _workspace;
        private ICodeFixService _codeFixService;

        public RoslynContextActionProvider(Microsoft.CodeAnalysis.Workspace workspace, ICodeFixService codeFixService)
        {
            _codeFixService = codeFixService;
            _workspace = workspace;
        }

        public async Task<IEnumerable<CodeFix>> GetCodeFixes(ITextEditor editor, int offset, int length, CancellationToken cancellationToken)
        {
            var textSpan = new TextSpan(offset, length);

            var workspace = RoslynWorkspace.GetWorkspace(editor.SourceFile.Project.Solution);

            var document = workspace.GetDocument(editor.SourceFile);

            var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);

            if (textSpan.End >= text.Length) return Array.Empty<CodeFix>();

            var fixes = (await _codeFixService.GetFixesAsync(document, textSpan, true, cancellationToken))
                .SelectMany(f => f.Fixes)
                .GroupBy(f => f.Action.EquivalenceKey)
                .Select(group => group.First()).Select(fix => new CodeFix { Action = new RoslynCodeAction(fix.Action) });

            try
            {
                var codeRefactorings = (await workspace.GetService<ICodeRefactoringService>().GetRefactoringsAsync(
                    document,
                    textSpan, cancellationToken).ConfigureAwait(false))
                    .Where(x => ExcludedRefactoringProviders.All(p => !x.Provider.GetType().Name.Contains(p)))
                    .SelectMany(refactoring => refactoring.Actions)
                    .GroupBy(f => f.EquivalenceKey)
                    .Select(group => group.First())
                    .Select(refactoring => new CodeFix { Action = new RoslynCodeAction(refactoring) });

                return fixes.Concat(codeRefactorings);
            }
            catch (TaskCanceledException)
            {
                return fixes;
            }

            //var actions = new List<CodeAction>();

            //var refactoringContext = new CodeRefactoringContext(document, textSpan, action => actions.Add(action), cancellationToken);

            //foreach (var action in actions)
            //{
            //    result.Add(new CodeFix { Action = new RoslynCodeAction(action) });
            //}

            //return result;
        }

        public ICommand GetActionCommand(object action)
        {
            //if (action is CodeAction codeAction)
            //{
            //    return new CodeActionCommand(this, codeAction);
            //}
            var codeFix = action as CodeFix;
            if (codeFix == null || codeFix.Action.HasCodeActions()) return null;
            return new CodeActionCommand(this, codeFix.Action);
        }

        public async Task ExecuteCodeActionAsync(ICodeAction codeAction)
        {
            var operations = await codeAction.GetOperationsAsync(CancellationToken.None).ConfigureAwait(true);
            foreach (var operation in operations)
            {
                operation.Apply(_workspace,
                    CancellationToken.None);
            }
        }
    }

}