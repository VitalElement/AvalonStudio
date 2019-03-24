using AvalonStudio.Languages;
using System;
using System.Collections.Immutable;

namespace AvalonStudio.Controls.Editor.ContextActions
{
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
}
