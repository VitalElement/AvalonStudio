namespace AvalonStudio.Languages
{
    using AvalonStudio.Languages;
    using System.Collections.Generic;

    public interface ICompletionAdviceControl
    {
        bool IsVisible { get; set; }

        Symbol Symbol { get; set; }

        int SelectedIndex { get; set; }

        int Count { get; set; }
    }
}