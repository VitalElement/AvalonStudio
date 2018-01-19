namespace AvalonStudio.Languages.CSharp
{
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class CheckForStaticExtension
    {
        public static (string name, bool inbuilt)? GetReturnType(ISymbol symbol)
        {
            var type = GetReturnTypeSymbol(symbol);

            if (type != null)
            {
                return (type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), type.IsSealed && type.IsValueType);
            }

            return null;
        }

        private static ITypeSymbol GetReturnTypeSymbol(ISymbol symbol)
        {
            var methodSymbol = symbol as IMethodSymbol;
            if (methodSymbol != null)
            {
                if (methodSymbol.MethodKind != MethodKind.Constructor)
                {
                    return methodSymbol.ReturnType;
                }
            }

            if (symbol is IPropertySymbol propertySymbol)
            {
                return propertySymbol.Type;
            }

            if (symbol is ILocalSymbol localSymbol)
            {
                return localSymbol.Type;
            }

            if (symbol is IParameterSymbol parameterSymbol)
            {
                return parameterSymbol.Type;
            }

            if (symbol is IFieldSymbol fieldSymbol)
            {
                return fieldSymbol.Type;
            }

            if (symbol is IEventSymbol eventSymbol)
            {
                return eventSymbol.Type;
            }

            return null;
        }

        public static bool IsInStaticContext(this SyntaxNode node)
        {
            // this/base calls are always static.
            if (node.FirstAncestorOrSelf<ConstructorInitializerSyntax>() != null)
            {
                return true;
            }

            var memberDeclaration = node.FirstAncestorOrSelf<MemberDeclarationSyntax>();
            if (memberDeclaration == null)
            {
                return false;
            }

            switch (memberDeclaration.Kind())
            {
                case SyntaxKind.MethodDeclaration:
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.EventDeclaration:
                case SyntaxKind.IndexerDeclaration:
                    return GetModifiers(memberDeclaration).Any(SyntaxKind.StaticKeyword);

                case SyntaxKind.PropertyDeclaration:
                    return GetModifiers(memberDeclaration).Any(SyntaxKind.StaticKeyword) ||
                        node.IsFoundUnder((PropertyDeclarationSyntax p) => p.Initializer);

                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                    // Inside a field one can only access static members of a type (unless it's top-level).
                    return !memberDeclaration.Parent.IsKind(SyntaxKind.CompilationUnit);

                case SyntaxKind.DestructorDeclaration:
                    return false;
            }

            // Global statements are not a static context.
            if (node.FirstAncestorOrSelf<GlobalStatementSyntax>() != null)
            {
                return false;
            }

            // any other location is considered static
            return true;
        }

        public static SyntaxTokenList GetModifiers(SyntaxNode member)
        {
            if (member != null)
            {
                switch (member.Kind())
                {
                    case SyntaxKind.EnumDeclaration:
                        return ((EnumDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.ClassDeclaration:
                    case SyntaxKind.InterfaceDeclaration:
                    case SyntaxKind.StructDeclaration:
                        return ((TypeDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.DelegateDeclaration:
                        return ((DelegateDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.FieldDeclaration:
                        return ((FieldDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.EventFieldDeclaration:
                        return ((EventFieldDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.ConstructorDeclaration:
                        return ((ConstructorDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.DestructorDeclaration:
                        return ((DestructorDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.PropertyDeclaration:
                        return ((PropertyDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.EventDeclaration:
                        return ((EventDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.IndexerDeclaration:
                        return ((IndexerDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.OperatorDeclaration:
                        return ((OperatorDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.ConversionOperatorDeclaration:
                        return ((ConversionOperatorDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.MethodDeclaration:
                        return ((MethodDeclarationSyntax)member).Modifiers;
                    case SyntaxKind.GetAccessorDeclaration:
                    case SyntaxKind.SetAccessorDeclaration:
                    case SyntaxKind.AddAccessorDeclaration:
                    case SyntaxKind.RemoveAccessorDeclaration:
                        return ((AccessorDeclarationSyntax)member).Modifiers;
                }
            }

            return default;
        }

        public static bool IsFoundUnder<TParent>(this SyntaxNode node, Func<TParent, SyntaxNode> childGetter)
           where TParent : SyntaxNode
        {
            var ancestor = node.GetAncestor<TParent>();
            if (ancestor == null)
            {
                return false;
            }

            var child = childGetter(ancestor);

            // See if node passes through child on the way up to ancestor.
            return node.GetAncestorsOrThis<SyntaxNode>().Contains(child);
        }

        public static TNode GetAncestor<TNode>(this SyntaxNode node)
           where TNode : SyntaxNode
        {
            var current = node.Parent;
            while (current != null)
            {
                if (current is TNode tNode)
                {
                    return tNode;
                }

                current = current.GetParent();
            }

            return null;
        }

        private static SyntaxNode GetParent(this SyntaxNode node)
        {
            return node is IStructuredTriviaSyntax trivia ? trivia.ParentTrivia.Token.Parent : node.Parent;
        }

        public static TNode FirstAncestorOrSelfUntil<TNode>(this SyntaxNode node, Func<SyntaxNode, bool> predicate)
            where TNode : SyntaxNode
        {
            for (var current = node; current != null; current = current.GetParent())
            {
                if (current is TNode tnode)
                {
                    return tnode;
                }

                if (predicate(current))
                {
                    break;
                }
            }

            return default;
        }

        public static TNode GetAncestorOrThis<TNode>(this SyntaxNode node)
                where TNode : SyntaxNode
        {
            return node?.GetAncestorsOrThis<TNode>().FirstOrDefault();
        }

        public static IEnumerable<TNode> GetAncestorsOrThis<TNode>(this SyntaxNode node)
            where TNode : SyntaxNode
        {
            var current = node;
            while (current != null)
            {
                if (current is TNode tNode)
                {
                    yield return tNode;
                }

                current = current.GetParent();
            }
        }
    }

    internal class InvocationContext
    {
        public SemanticModel SemanticModel { get; }
        public int Position { get; }
        public SyntaxNode Receiver { get; }
        public IEnumerable<TypeInfo> ArgumentTypes { get; }
        public IEnumerable<SyntaxToken> Separators { get; }
        public bool IsInStaticContext { get; }

        public InvocationContext(SemanticModel semModel, int position, SyntaxNode receiver, ArgumentListSyntax argList, bool isStatic)
        {
            SemanticModel = semModel;
            Position = position;
            Receiver = receiver;
            ArgumentTypes = argList.Arguments.Select(argument => semModel.GetTypeInfo(argument.Expression));
            Separators = argList.Arguments.GetSeparators();
            IsInStaticContext = isStatic;
        }

        public InvocationContext(SemanticModel semModel, int position, SyntaxNode receiver, AttributeArgumentListSyntax argList, bool isStatic)
        {
            SemanticModel = semModel;
            Position = position;
            Receiver = receiver;
            ArgumentTypes = argList.Arguments.Select(argument => semModel.GetTypeInfo(argument.Expression));
            Separators = argList.Arguments.GetSeparators();
            IsInStaticContext = isStatic;
        }

        public SignatureHelp BuildSignatureHelp()
        {
            var result = new SignatureHelp(Receiver.SpanStart);

            int activeParameter = 0;

            foreach (var comma in Separators)
            {
                if (comma.Span.Start > Position)
                {
                    break;
                }

                activeParameter += 1;
            }

            var types = ArgumentTypes;
            ISymbol throughSymbol = null;
            ISymbol throughType = null;

            var bestScore = int.MinValue;
            Signature bestScoredItem = null;

            var methodGroup = SemanticModel.GetMemberGroup(Receiver).OfType<IMethodSymbol>();
            if (Receiver is MemberAccessExpressionSyntax)
            {
                var throughExpression = ((MemberAccessExpressionSyntax)Receiver).Expression;
                throughSymbol = SemanticModel.GetSpeculativeSymbolInfo(Position, throughExpression, SpeculativeBindingOption.BindAsExpression).Symbol;
                throughType = SemanticModel.GetSpeculativeTypeInfo(Position, throughExpression, SpeculativeBindingOption.BindAsTypeOrNamespace).Type;
                var includeInstance = throughSymbol != null && !(throughSymbol is ITypeSymbol);
                var includeStatic = (throughSymbol is INamedTypeSymbol) || throughType != null;
                methodGroup = methodGroup.Where(m => (m.IsStatic && includeStatic) || (!m.IsStatic && includeInstance));
            }
            else if (Receiver is SimpleNameSyntax && IsInStaticContext)
            {
                methodGroup = methodGroup.Where(m => m.IsStatic);
            }

            foreach (var methodOverload in methodGroup)
            {
                var signature = BuildSignature(methodOverload);
                result.Signatures.Add(signature);

                var score = InvocationScore(methodOverload, types);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestScoredItem = signature;
                }
            }

            result.ActiveSignature = result.Signatures.IndexOf(bestScoredItem);

            result.ActiveParameter = activeParameter;

            return result;
        }

        private int InvocationScore(IMethodSymbol symbol, IEnumerable<TypeInfo> types)
        {
            var parameters = symbol.Parameters;
            if (parameters.Count() < types.Count())
            {
                return int.MinValue;
            }

            var score = 0;
            var invocationEnum = types.GetEnumerator();
            var definitionEnum = parameters.GetEnumerator();
            while (invocationEnum.MoveNext() && definitionEnum.MoveNext())
            {
                if (invocationEnum.Current.ConvertedType == null)
                {
                    // 1 point for having a parameter
                    score += 1;
                }
                else if (invocationEnum.Current.ConvertedType.Equals(definitionEnum.Current.Type))
                {
                    // 2 points for having a parameter and being
                    // the same type
                    score += 2;
                }
            }

            return score;
        }


        private static Signature BuildSignature(IMethodSymbol symbol)
        {
            var signature = new Signature();

            var docComment = DocumentationComment.From(symbol.GetDocumentationCommentXml(), Environment.NewLine);

            var parameterDocumentation = new Dictionary<string, string>();

            foreach (var param in docComment.ParamElements)
            {
                var parts = param.Split(':');

                parameterDocumentation.Add(parts[0].Trim(), parts[1].Trim());
            }

            signature.Description = docComment.SummaryText;

            signature.Name = symbol.MethodKind == MethodKind.Constructor ? symbol.ContainingType.Name : symbol.Name;
            signature.Label = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

            var returnTypeInfo = CheckForStaticExtension.GetReturnType(symbol);

            if(returnTypeInfo.HasValue)
            {
                if(returnTypeInfo.Value.inbuilt)
                {
                    signature.BuiltInReturnType = returnTypeInfo.Value.name;
                }
                else
                {
                    signature.ReturnType = returnTypeInfo.Value.name;
                }
            }

            signature.Parameters = symbol.Parameters.Select(parameter =>
            {
                var info = CheckForStaticExtension.GetReturnType(parameter);

                var result = new Parameter()
                {
                    Name = parameter.Name,
                    Label = parameter.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                };

                if(parameterDocumentation.ContainsKey(parameter.Name))
                {
                    result.Documentation = parameterDocumentation[parameter.Name];
                }

                if(info.HasValue)
                {
                    if(info.Value.inbuilt)
                    {
                        result.BuiltInType = info.Value.name;
                    }
                    else
                    {
                        result.BuiltInType = info.Value.name;
                    }
                }

                return result;
            }).ToList();

            return signature;
        }

    }
}