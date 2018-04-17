using AvalonStudio.Controls;

namespace AvalonStudio.Languages
{
    public class QuickInfoResult
    {
        public QuickInfoResult(StyledText text)
        {
            Text = text;
        }

        public StyledText Text { get; }
    }
}
