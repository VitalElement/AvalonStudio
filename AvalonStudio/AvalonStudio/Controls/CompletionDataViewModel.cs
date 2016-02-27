namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Languages;
    using System;

    public class CompletionDataViewModel : ViewModel<CodeCompletionData>
    {
        public CompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }

        public string Title { get { return Model.Suggestion; } }

        public uint Priority { get { return Model.Priority; } }

        public string Kind
        {
            get
            {
                return Model.Kind.ToString();
            }
        }

        

        public string Hint { get { return Model?.Hint; } }
        public string Comment { get { return Model?.BriefComment; } }

        public static CompletionDataViewModel Create(CodeCompletionData data)
        {
            CompletionDataViewModel result = null;

            switch (data.Kind)
            {
                case CursorKind.FunctionDeclaration:
                case CursorKind.CXXMethod:
                case CursorKind.Constructor:
                case CursorKind.Destructor:
                    result = new MethodCompletionDataViewModel(data);
                    break;

                case CursorKind.MacroDefinition:
                    result = new MacroCompletionDataViewModel(data);
                    break;

                case CursorKind.StructDeclaration:
                case CursorKind.ClassDeclaration:
                    result = new ClassCompletionDataViewModel(data);
                    break;

                default:
                    result = new VariableCompletionDataViewModel(data);
                    break;
            }

            return result;
        }
    }

    public class MethodCompletionDataViewModel : CompletionDataViewModel
    {
        public MethodCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class MacroCompletionDataViewModel : CompletionDataViewModel
    {
        public MacroCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class VariableCompletionDataViewModel : CompletionDataViewModel
    {
        public VariableCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class ClassCompletionDataViewModel : CompletionDataViewModel
    {
        public ClassCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }
}
