using AvalonStudio.Documents;
using AvalonStudio.Projects;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvalonStudio.Languages
{
    public interface IContextActionProvider
    {   
        ICommand GetActionCommand(object action);

        Task<IEnumerable<CodeFix>> GetCodeFixes(ITextEditor editor, int offset, int length, CancellationToken cancellationToken);
    }

    public interface ICodeAction
    {
        ImmutableArray<ICodeAction> NestedCodeActions { get; }

        bool IsInlinable { get; }
        string EquivalenceKey { get; }
        string Message { get; }
        string Title { get; }

        Task<ImmutableArray<ICodeActionOperation>> GetOperationsAsync(CancellationToken cancellationToken);
    }

    public interface ICodeActionOperation
    {
        //
        // Summary:
        //     A short title describing of the effect of the operation.
        string Title { get; }

        //
        // Summary:
        //     Called by the host environment to apply the effect of the operation. This method
        //     is guaranteed to be called on the UI thread.
        void Apply(object workspace, CancellationToken cancellationToken);
    }

    public sealed class CodeFix
    {
        //private readonly Microsoft.CodeAnalysis.CodeFixes.CodeFix _inner;

        public IProject Project => null;

        public ICodeAction Action { get; set; }        

        //public ImmutableArray<Diagnostic> Diagnostics => _inner.Diagnostics;

        public Diagnostic PrimaryDiagnostic { get; set; }// => _inner.PrimaryDiagnostic;

        public CodeFix()
        {

        }
    }
}
