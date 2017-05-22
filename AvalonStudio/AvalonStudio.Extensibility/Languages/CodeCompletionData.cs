using System;
using Avalonia.Media.Imaging;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;

namespace AvalonStudio.Languages
{
    public enum CodeCompletionKind
    {
        Keyword,
        Namespace,
        Delegate,
        Interface,
        Property,
        Event,
        Enum,
        EnumConstant,
        Struct,
        Class,
        Method,
        Field,
        Parameter,
        None,
        Macro,
        Variable,
        OverloadCandidate
    }

    public class CodeCompletionData : ICompletionData
    {
        public uint Priority { get; set; }
        public string Suggestion { get; set; }
        public CodeCompletionKind Kind { get; set; }
        public string Hint { get; set; }
        public string BriefComment { get; set; }
        public int Overloads { get; set; }

        public IBitmap Image => null;

        public string Text => Suggestion;

        public object Content => Text; // Could show a ui element here instead! Future use! 

        public object Description => BriefComment;

        double ICompletionData.Priority => (double)Priority;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, Text);
        }
    }
}