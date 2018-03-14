using System;
using Microsoft.CodeAnalysis;

namespace AvalonStudio.Projects.OmniSharp.Roslyn
{
    public enum DiagnosticOptions
    {
        /// <summary>
        /// Include syntax errors
        /// </summary>
        Syntax = DiagnosticProvider.Options.Syntax,

        /// <summary>
        /// Include semantic errors
        /// </summary>
        Semantic = DiagnosticProvider.Options.Semantic,
    }


    /// <summary>
    /// Provide a way for users to turn on and off analyzing workspace for compiler diagnostics
    /// </summary>
    public static class WorkspaceExtensions
    {
        public static void EnableDiagnostics(this Workspace workspace, DiagnosticOptions options)
        {
            var diagnosticProviderOptions = (DiagnosticProvider.Options)0;
            if ((options & DiagnosticOptions.Syntax) != 0)
                diagnosticProviderOptions |= DiagnosticProvider.Options.Syntax;
            if ((options & DiagnosticOptions.Semantic) != 0)
                diagnosticProviderOptions |= DiagnosticProvider.Options.Semantic;

            DiagnosticProvider.Enable(workspace, diagnosticProviderOptions);
        }

        public static void DisableDiagnostics(this Workspace workspace)
        {
            DiagnosticProvider.Disable(workspace);
        }
    }
}