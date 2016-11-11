namespace AvalonStudio.Languages.D
{
    using D_Parser.Dom;
    using D_Parser.Dom.Expressions;
    using D_Parser.Parser;

    static class DParserExtensions
    {
        public static void AddHighlight(this ISyntaxRegion region, HighlightType type, SyntaxHighlightDataList list)
        {
            if (region.EndLocation.Line != 0 && region.EndLocation.Column != 0)
            {
                list.Add(new LineColumnSyntaxHighlightingData(region.Location.Line, region.Location.Column, region.EndLocation.Line, region.EndLocation.Column, type));
            }
        }

        //public static void AddHighlgiht(this IExpression expression, HighlightType type, SyntaxHighlightDataList list)
        //{
        //    if (expression.EndLocation.Line != 0 && expression.EndLocation.Column != 0)
        //    {
        //        list.Add(new LineColumnSyntaxHighlightingData(expression.Location.Line, expression.Location.Column, expression.EndLocation.Line, expression.EndLocation.Column, type));
        //    }
        //}

        public static LineColumnSyntaxHighlightingData ToHighlight(this Comment comment)
        {
            if (comment.CommentType == Comment.Type.SingleLine)
            {
                return new LineColumnSyntaxHighlightingData(comment.StartPosition.Line, comment.StartPosition.Column, comment.StartPosition.Line, comment.EndPosition.Column, HighlightType.Comment);
            }
            else
            {
                return new LineColumnSyntaxHighlightingData(comment.StartPosition.Line, comment.StartPosition.Column, comment.EndPosition.Line, comment.EndPosition.Column, HighlightType.Comment);
            }
        }
    }
    class HighlightVisitor : DefaultDepthFirstVisitor
    {
        public HighlightVisitor()
        {
            Highlights = new SyntaxHighlightDataList();
        }

        public SyntaxHighlightDataList Highlights { get; set; }

        public override void Visit(DEnum dEnum)
        {
            dEnum.AddHighlight(HighlightType.StructName, Highlights);
        }

        public override void Visit(DMethod n)
        {
            Highlights.Add(new LineColumnSyntaxHighlightingData(n.NameLocation.Line, n.NameLocation.Column, n.EndLocation.Line, n.EndLocation.Column, HighlightType.Identifier));
            base.Visit(n);      
        }

        public override void Visit(DVariable n)
        {
            Highlights.Add(new LineColumnSyntaxHighlightingData(n.NameLocation.Line, n.NameLocation.Column, n.EndLocation.Line, n.EndLocation.Column, HighlightType.Identifier));
            base.Visit(n);
        }

        public override void Visit(IdentifierDeclaration td)
        {
            td.AddHighlight(HighlightType.ClassName, Highlights);
            base.Visit(td);
        }

        public override void Visit(IdentityExpression td)
        {
            td.AddHighlight(HighlightType.Debug, Highlights);
            base.Visit(td);
        }

        public override void VisitAttribute(Modifier attribute)
        {
            attribute.AddHighlight(HighlightType.Keyword, Highlights);
            base.VisitAttribute(attribute);
        }

        public override void Visit(DTokenDeclaration x)
        {
            x.AddHighlight(HighlightType.Keyword, Highlights);

            base.Visit(x);
        }

        public override void Visit(IdentifierExpression x)
        {
            switch (x.Format)
            {
                case LiteralFormat.None:
                    break;

                case LiteralFormat.Scalar:
                    x.AddHighlight(HighlightType.NumericLiteral, Highlights);
                    break;
                case LiteralFormat.FloatingPoint:
                    x.AddHighlight(HighlightType.NumericLiteral, Highlights);
                    break;
                case LiteralFormat.Scalar | LiteralFormat.FloatingPoint:
                    x.AddHighlight(HighlightType.NumericLiteral, Highlights);
                    break;

                default:
                    x.AddHighlight(HighlightType.Literal, Highlights);
                    break;
            }

            base.Visit(x);
        }

        public override void Visit(DModule n)
        {
            foreach (var comment in n.Comments)
            {
                Highlights.Add(comment.ToHighlight());
            }

            base.Visit(n);
        }

        public override void Visit(TemplateAliasParameter p)
        {
            p.AddHighlight(HighlightType.Debug, Highlights);

            base.Visit(p);
        }

        public override void Visit(ArrayInitializer x)
        {
            x.AddHighlight(HighlightType.Debug, Highlights);

            base.Visit(x);
        }

        public override void Visit(ArrayDecl td)
        {
            td.AddHighlight(HighlightType.White, Highlights);

            if (td.KeyExpression != null)
            {
                td.KeyExpression.AddHighlight(HighlightType.NumericLiteral, Highlights);
            }

            base.Visit(td);
        }
    }
}
