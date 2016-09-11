using D_Parser.Dom;
using D_Parser.Dom.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.D
{
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

        public override void Visit(DTokenDeclaration td)
        {
            Highlights.Add(new LineColumnSyntaxHighlightingData() { StartLine = td.Location.Line, StartColumn = td.Location.Column, EndLine = td.EndLocation.Line, EndColumn = td.EndLocation.Column, Type = HighlightType.Keyword });
            base.Visit(td);
        }


        public override void Visit(IdentifierExpression x)
        {
            
            // gets locaitons of literals i.e. "string"
            switch (x.Format)
            {
                case D_Parser.Parser.LiteralFormat.None:
                    break;

                default:
                    Highlights.Add(new LineColumnSyntaxHighlightingData() { StartLine = x.Location.Line, StartColumn = x.Location.Column, EndLine = x.EndLocation.Line, EndColumn = x.EndLocation.Column, Type = HighlightType.Literal });
                    break;
            }

            base.Visit(x);
        }


        public override void Visit(DClassLike n)
        {
            // Gets locations of class definition name
            base.Visit(n);
        }
    }
}
