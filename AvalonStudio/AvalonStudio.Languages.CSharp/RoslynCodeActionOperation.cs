using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;

namespace AvalonStudio.Languages.CSharp
{
    public class RoslynCodeActionOperation : ICodeActionOperation
    {
        private CodeActionOperation _inner;

        public RoslynCodeActionOperation(CodeActionOperation inner)
        {
            _inner = inner;
        }

        public string Title => _inner.Title;

        public void Apply(object workspace, CancellationToken cancellationToken)
        {
            _inner.Apply(workspace as Microsoft.CodeAnalysis.Workspace, cancellationToken);
        }
    }

}