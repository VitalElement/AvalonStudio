using Microsoft.CodeAnalysis.CodeActions;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp
{
    public class RoslynCodeAction : ICodeAction
    {
        public ImmutableArray<ICodeAction> NestedCodeActions => new ImmutableArray<ICodeAction>();

        public bool IsInlinable => _inner.IsInlinable;

        public string EquivalenceKey => _inner.EquivalenceKey;

        public string Message => _inner.Message;

        public string Title => _inner.Title;

        private CodeAction _inner;

        public RoslynCodeAction(CodeAction inner)
        {
            _inner = inner;
        }

        //
        // Summary:
        //     The sequence of operations that define the code action.
        public async Task<ImmutableArray<ICodeActionOperation>> GetOperationsAsync(CancellationToken cancellationToken)
        {
            var operations = await _inner.GetOperationsAsync(cancellationToken);

            return operations.Select(op => new RoslynCodeActionOperation(op)).Cast<ICodeActionOperation>().ToImmutableArray();
        }

        public Task PerformActionAsync()
        {
            return Task.CompletedTask;
        }
    }

}