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
        public static LineColumnSyntaxHighlightingData ToHighlight(this ISyntaxRegion region, HighlightType type)
        {
            return new LineColumnSyntaxHighlightingData(region.Location.Line, region.Location.Column, region.EndLocation.Line, region.EndLocation.Column, type);
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

        public override void Visit(DMethod n)
        {
            base.Visit(n);
        }

        public override void Visit(DVariable n)
        {
            base.Visit(n);
        }

        public override void Visit(IdentifierDeclaration td)
        {
            base.Visit(td);
        }

        public override void Visit(DTokenDeclaration x)
        {
            Highlights.Add(x.ToHighlight(HighlightType.Keyword));

            base.Visit(x);
        }
        

        public override void Visit(IdentifierExpression x)
        {
            switch (x.Format)
            {
                case D_Parser.Parser.LiteralFormat.None:
                    break;

                default:                    
                    Highlights.Add(x.ToHighlight(HighlightType.Literal));
                    break;
            }

            base.Visit(x);
        }

        public override void Visit(DModule n)
        {         
            foreach(var comment in n.Comments)
            {
                Highlights.Add(comment.ToHighlight());
            }
            
            base.Visit(n);
        }
        
        public override void Visit(DClassLike x)
        {
            Highlights.Add(x.ToHighlight(HighlightType.ClassName));
            
            base.Visit(x);
        }
    }
}
