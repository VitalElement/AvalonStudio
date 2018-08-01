using Avalonia.Media.Imaging;
using System;

namespace AvalonStudio.Languages
{
    public enum CompletionItemSelectionBehavior
    {
        Default = 0,
        //
        // Summary:
        //     If no text has been typed, the item should be soft selected. This is appropriate
        //     for completion providers that want to provide suggestions that shouldn't interfere
        //     with typing. For example a provider that comes up on space might offer items
        //     that are soft selected so that an additional space (or other puntuation character)
        //     will not then commit that item.
        SoftSelection = 1,
        //
        // Summary:
        //     If no text has been typed, the item should be hard selected. This is appropriate
        //     for completion providers that are providing suggestions the user is nearly certain
        //     to select. Because the item is hard selected, any commit characters typed after
        //     it will cause it to be committed.
        HardSelection = 2
    }

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
        public CodeCompletionData(string displayText, string filterText, string insertionText, int? recommendedCaretOffset = null, CompletionItemSelectionBehavior selectionBehavior = CompletionItemSelectionBehavior.Default, int priority = 0)
        {
            DisplayText = displayText;
            FilterText = filterText;
            InsertionText = insertionText;
            RecommendedCaretPosition = recommendedCaretOffset;
            SelectionBehavior = selectionBehavior;
            Priority = priority;
        }

        public bool RecommendImmediateSuggestions { get; set; }

        public int Priority { get; set; }

        public string DisplayText { get; set; }

        public string FilterText { get; set; }

        public string InsertionText { get; set; }

        public string BriefComment { get; set; }

        public int? RecommendedCaretPosition { get; set; }

        public CodeCompletionKind Kind { get; set; }

        public CompletionItemSelectionBehavior SelectionBehavior { get; }

        public int Overloads { get; set; }

        public IBitmap Image => null;

        public int CompareTo(CodeCompletionData other)
        {
            return DisplayText.CompareTo(other.DisplayText);
        }
    }
}