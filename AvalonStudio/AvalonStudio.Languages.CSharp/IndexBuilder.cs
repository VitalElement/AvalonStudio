using AvalonStudio.Extensibility.Languages;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp
{
    public class IndexBuilder : CSharpSyntaxWalker
    {
        private readonly List<IndexEntry> _entries = new List<IndexEntry>();

        public IndexBuilder(List<IndexEntry> entries)
        {
            _entries = entries;
        }

        public static async Task<List<IndexEntry>> Compute(
            Document document)
        {
            var root = new List<IndexEntry>();
            var visitor = new IndexBuilder(root);

            await visitor.Process(document);

            return root;
        }

        private async Task Process(Document document)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync();
            (syntaxRoot as CSharpSyntaxNode)?.Accept(this);
        }

        private void AddEntry(string text, Location location)
        {
            _entries.Add(new IndexEntry(text, location.SourceSpan.Start, location.SourceSpan.End, CursorKind.ClassDeclaration));
        }

        public override void VisitBlock(BlockSyntax node)
        {
            AddEntry("", node.GetLocation());

            base.VisitBlock(node);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            AddEntry(node.Identifier.Text, node.GetLocation());

            base.VisitClassDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            AddEntry(node.Identifier.Text, node.GetLocation());

            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            AddEntry(node.Identifier.Text, node.GetLocation());

            base.VisitEnumDeclaration(node); ;
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            AddEntry(node.Identifier.Text, node.GetLocation());
            base.VisitStructDeclaration(node);
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            AddEntry("", node.GetLocation());
            base.VisitNamespaceDeclaration(node);
        }
    }

}
