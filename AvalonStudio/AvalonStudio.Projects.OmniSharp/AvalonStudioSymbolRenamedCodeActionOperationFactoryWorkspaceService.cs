using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeActions.WorkspaceServices;
using Microsoft.CodeAnalysis.Host.Mef;
using System;
using System.Composition;

namespace Microsoft.VisualStudio.LanguageServices.Implementation
{
    [ExportWorkspaceService(typeof(ISymbolRenamedCodeActionOperationFactoryWorkspaceService), ServiceLayer.Host), Shared]
    internal sealed class AvalonStudioRenameSymbolCodeActionOperationFactoryWorkspaceService : ISymbolRenamedCodeActionOperationFactoryWorkspaceService
    {
        public AvalonStudioRenameSymbolCodeActionOperationFactoryWorkspaceService()
        {
        }

        public CodeActionOperation CreateSymbolRenamedOperation(ISymbol symbol, string newName, Solution startingSolution, Solution updatedSolution)
        {
            return new RenameSymbolOperation(                
                symbol ?? throw new ArgumentNullException(nameof(symbol)),
                newName ?? throw new ArgumentNullException(nameof(newName)),
                startingSolution ?? throw new ArgumentNullException(nameof(startingSolution)),
                updatedSolution ?? throw new ArgumentNullException(nameof(updatedSolution)));
        }

        private class RenameSymbolOperation : CodeActionOperation
        {            
            private readonly ISymbol _symbol;
            private readonly string _newName;
            private readonly Solution _startingSolution;
            private readonly Solution _updatedSolution;

            public RenameSymbolOperation(                
                ISymbol symbol,
                string newName,
                Solution startingSolution,
                Solution updatedSolution)
            {                
                _symbol = symbol;
                _newName = newName;
                _startingSolution = startingSolution;
                _updatedSolution = updatedSolution;
            }

            public override string Title => string.Format("Rename '{0}' to '{1}'", _symbol.Name, _newName);
        }
    }
}