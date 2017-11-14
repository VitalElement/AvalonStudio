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
        NamespacePublic,
        NamespaceProtected,
        NamespacePrivate,
        NamespaceInternal,

        DelegatePublic,
        DelegateProtected,
        DelegatePrivate,
        DelegateInternal,

        InterfacePublic,
        InterfaceProtected,
        InterfacePrivate,
        InterfaceInternal,

        PropertyPublic,
        PropertyProtected,
        PropertyPrivate,
        PropertyInternal,

        EventPublic,
        EventProtected,
        EventPrivate,
        EventInternal,

        EnumPublic,
        EnumProtected,
        EnumPrivate,
        EnumInternal,

        EnumMemberPublic,
        EnumMemberProtected,
        EnumMemberPrivate,
        EnumMemberInternal,

        StructurePublic,
        StructureProtected,
        StructurePrivate,
        StructureInternal,

        ClassPublic,
        ClassProtected,
        ClassPrivate,
        ClassInternal,

        MethodPublic,
        MethodProtected,
        MethodPrivate,
        MethodInternal,

        FieldPublic,
        FieldProtected,
        FieldPrivate,
        FieldInternal,

        Parameter,
        None,
        Macro,
        Variable,
        OverloadCandidate,
        Snippet
    }

    public class CodeCompletionData : IComparable<CodeCompletionData>
    {
        public CodeCompletionData(string displayText, string insertionText, int? recommendedCaretOffset = null)
        {
            DisplayText = displayText;
            InsertionText = insertionText;
            RecommendedCaretPosition = recommendedCaretOffset;
        }

        public bool RecommendImmediateSuggestions { get; set; }

        public uint Priority { get; set; }

        public string DisplayText { get; set; }

        public string InsertionText { get; set; }

        public string BriefComment { get; set; }

        public int? RecommendedCaretPosition { get; set; }

        public CodeCompletionKind Kind { get; set; }

        public int Overloads { get; set; }

        public IBitmap Image => null;

        public int CompareTo(CodeCompletionData other)
        {
            return DisplayText.CompareTo(other.DisplayText);
        }
    }
}