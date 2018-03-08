using AvalonStudio.Languages;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvalonStudio.Controls.Standard.CodeEditor.ContextActions
{
    public sealed class CodeFix
    {
        //private readonly Microsoft.CodeAnalysis.CodeFixes.CodeFix _inner;

        public IProject Project => null;

        //public CodeAction Action => _inner.Action;

        //public ImmutableArray<Diagnostic> Diagnostics => _inner.Diagnostics;

        public Diagnostic PrimaryDiagnostic { get; set; }// => _inner.PrimaryDiagnostic;

        internal CodeFix()
        {
            
        }
    }

    class DummyContextActionProvider : IContextActionProvider
    {
        public ICommand GetActionCommand(object action)
        {
            return null;
        }

        public async Task<IEnumerable<object>> GetActions(int offset, int length, CancellationToken cancellationToken)
        {
            return new List<CodeFix>
            {
                new CodeFix{ PrimaryDiagnostic = new Diagnostic{ Spelling = "Test Action 1" } },
                new CodeFix{ PrimaryDiagnostic = new Diagnostic { Spelling = "Code Action 2"}},
            };
        }
    }
}
