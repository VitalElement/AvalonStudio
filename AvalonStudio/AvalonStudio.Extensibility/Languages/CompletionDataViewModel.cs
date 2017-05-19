using AvalonStudio.MVVM;

namespace AvalonStudio.Languages.ViewModels
{
    public class CompletionDataViewModel : ViewModel<CodeCompletionData>
    {
        public CompletionDataViewModel(CodeCompletionData model) : base(model)
        {
            Overloads = 0;
        }

        public int Overloads { get; set; }

        public string OverloadText
        {
            get
            {
                if (Overloads == 0)
                {
                    return string.Empty;
                }
                else if (Overloads == 1)
                {
                    return $"(+ {Overloads} overload)";
                }
                else
                {
                    return $"(+ {Overloads} overloads)";
                }
            }
        }

        public string Text
        {
            get { return Model.Suggestion; }
        }

        public uint Priority
        {
            get { return Model.Priority; }
        }

        public string Kind
        {
            get { return Model.Kind.ToString(); }
        }

        public string Hint
        {
            get { return Model?.Hint; }
        }

        public string Comment
        {
            get { return Model?.BriefComment; }
        }

        public static CompletionDataViewModel Create(CodeCompletionData data)
        {
            CompletionDataViewModel result = null;

            switch (data.Kind)
            {
                case CodeCompletionKind.Method:
                    result = new MethodCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Macro:
                    result = new MacroCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Struct:
                    result = new StructCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Class:
                    result = new ClassCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Variable:
                    result = new VariableCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Property:
                    result = new PropertyCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Enum:
                    result = new EnumCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.EnumConstant:
                    result = new EnumConstantCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Keyword:
                    result = new KeywordCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Parameter:
                    result = new ParameterCompletionDataViewModel(data);
                    break;

                case CodeCompletionKind.Field:
                    result = new FieldCompletionDataViewModel(data);
                    break;

                default:
                    result = new DefaultCompletionDataViewModel(data);
                    break;
            }

            return result;
        }
    }
}