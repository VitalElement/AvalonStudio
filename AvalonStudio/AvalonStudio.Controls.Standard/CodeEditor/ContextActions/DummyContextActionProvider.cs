using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvalonStudio.Controls.Standard.CodeEditor.ContextActions
{
    public class DummyCodeActionOperation : ICodeActionOperation
    {
        public string Title => "Dummy Operation";

        public void Apply(object workspace, CancellationToken cancellationToken)
        {
            IoC.Get<IConsole>().WriteLine("Running dummy operation");
        }
    }

    public class DummyCodeAction : ICodeAction
    {
        public ImmutableArray<ICodeAction> NestedCodeActions => new ImmutableArray<ICodeAction>();

        //
        // Summary:
        //     The sequence of operations that define the code action.
        public async Task<ImmutableArray<ICodeActionOperation>> GetOperationsAsync(CancellationToken cancellationToken)
        {
            var result = new List<ICodeActionOperation>
            {
                new DummyCodeActionOperation(),
                new DummyCodeActionOperation()
            };

            return result.ToImmutableArray();
        }

        public Task PerformActionAsync()
        {
            IoC.Get<IConsole>().WriteLine("Running code action");
            return Task.CompletedTask;
        }
    }    

    public static class CodeActionExtensions
    {
        public static bool HasCodeActions(this ICodeAction codeAction)
        {
            if (codeAction == null) throw new ArgumentNullException(nameof(codeAction));

            return !codeAction.NestedCodeActions.IsDefaultOrEmpty;
        }

        public static ImmutableArray<ICodeAction> GetCodeActions(this ICodeAction codeAction)
        {
            if (codeAction == null) throw new ArgumentNullException(nameof(codeAction));

            return codeAction.NestedCodeActions;
        }        
    }

    class CodeActionCommand : ICommand
    {
        DummyContextActionProvider _provider;
        ICodeAction _codeAction;

        public CodeActionCommand(DummyContextActionProvider provider, ICodeAction codeAction)
        {
            _provider = provider;
            _codeAction = codeAction;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            await _provider.ExecuteCodeActionAsync(_codeAction).ConfigureAwait(true);
        }
    }

    class DummyContextActionProvider : IContextActionProvider
    {
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

        public async Task<IEnumerable<CodeFix>> GetActions(int offset, int length, CancellationToken cancellationToken)
        {
            return new List<CodeFix>
            {
                new CodeFix{ PrimaryDiagnostic = new Diagnostic{ Spelling = "Test Action 1" }, Action = new DummyCodeAction() },
                new CodeFix{ PrimaryDiagnostic = new Diagnostic { Spelling = "Code Action 2"}, Action = new DummyCodeAction()},
            };
        }

        public async Task ExecuteCodeActionAsync(ICodeAction codeAction)
        {
            var operations = await codeAction.GetOperationsAsync(CancellationToken.None).ConfigureAwait(true);
            foreach (var operation in operations)
            {
                operation.Apply(null,//_roslynHost.GetDocument(_documentId).Project.Solution.Workspace,
                    CancellationToken.None);
            }
        }
    }
}
