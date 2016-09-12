using D_Parser.Dom;
using D_Parser.Dom.Expressions;
using D_Parser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.D
{
    static class DParserExtensions
    {
        public static void AddHighlight(this ISyntaxRegion region, HighlightType type, SyntaxHighlightDataList list)
        {
            if (region.EndLocation.Line != 0 && region.EndLocation.Column != 0)
            {
                list.Add(new LineColumnSyntaxHighlightingData(region.Location.Line, region.Location.Column, region.EndLocation.Line, region.EndLocation.Column, type));
            }
        }

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

        public override void Visit(DEnumValue dEnumValue)
        {
            //
        }


        public override void Visit(DEnum dEnum)
        {
            dEnum.AddHighlight(HighlightType.StructName, Highlights);
        }




        public override void Visit(DMethod n)
        {
            if (n.ContainsPropertyAttribute(BuiltInAtAttribute.BuiltInAttributes.Property))


                n.AddHighlight(HighlightType.Identifier, Highlights);

            base.Visit(n);
        }

        public override void Visit(DVariable n)
        {
            n.AddHighlight(HighlightType.Identifier, Highlights);
            base.Visit(n);
        }

        public override void Visit(IdentifierDeclaration td)
        {
            td.AddHighlight(HighlightType.ClassName, Highlights);
            base.Visit(td);
        }

        public override void Visit(ImportStatement s)
        {
            s.AddHighlight(HighlightType.Keyword, Highlights);
            base.Visit(s);
        }

        public override void Visit(ModuleStatement s)
        {
            base.Visit(s);
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
                case D_Parser.Parser.LiteralFormat.None:
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

        public override void Visit(DClassLike x)
        {
            switch (x.ClassType)
            {
                case DTokens.Struct:
                    Highlights.Add(new LineColumnSyntaxHighlightingData(x.NameLocation.Line, x.NameLocation.Column, x.NameLocation.Line, x.Location.Column + 5, HighlightType.StructName));
                    break;

                default:
                    Highlights.Add(new LineColumnSyntaxHighlightingData(x.NameLocation.Line, x.NameLocation.Column, x.NameLocation.Line, x.Location.Column + 5, HighlightType.ClassName));
                    break;
                case DTokens.Interface:
                    Highlights.Add(new LineColumnSyntaxHighlightingData(x.NameLocation.Line, x.NameLocation.Column, x.NameLocation.Line, x.Location.Column + 5, HighlightType.StructName));
                    break;

                case DTokens.Template:
                    Highlights.Add(new LineColumnSyntaxHighlightingData(x.NameLocation.Line, x.NameLocation.Column, x.NameLocation.Line, x.Location.Column + 5, HighlightType.StructName));
                    break;

                case DTokens.Union:
                    Highlights.Add(new LineColumnSyntaxHighlightingData(x.NameLocation.Line, x.NameLocation.Column, x.NameLocation.Line, x.Location.Column + 5, HighlightType.StructName));
                    break;
            }

            Highlights.Add(new LineColumnSyntaxHighlightingData(x.Location.Line, x.Location.Column, x.Location.Line, x.Location.Column + 5, HighlightType.Keyword));
            //Highlights.Add(x.ToHighlight(HighlightType.ClassName));

            base.Visit(x);
        }
    }
}
