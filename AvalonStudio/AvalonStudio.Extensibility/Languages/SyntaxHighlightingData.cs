using AvalonStudio.Projects;
using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public enum HighlightType
    {
        None,
        CallExpression,
        Punctuation,
        Keyword,
        Identifier,
        Literal,
        NumericLiteral,
        Comment,
        ClassName,
        StructName,
        EnumConstant,
        EnumTypeName,
        InterfaceName,
        DelegateName,
        PreProcessor,
        PreProcessorText,
        Operator,
        Unnecessary
    }

    public class SyntaxHighlightDataList : List<OffsetSyntaxHighlightingData>
    {
        public SyntaxHighlightDataList(object tag, ISourceFile associatedFile)
        {
            AssociatedFile = associatedFile;
            Tag = tag;
        }

        public object Tag { get; }

        public ISourceFile AssociatedFile { get; }

        public new void Add(OffsetSyntaxHighlightingData item)
        {
            var index = BinarySearch(item);

            if (index < 0)
            {
                Insert(~index, item);
            }
            else
            {
                Insert(index + 1, item);
            }
        }
    }
}