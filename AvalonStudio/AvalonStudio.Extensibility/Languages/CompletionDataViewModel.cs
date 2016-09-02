using AvalonStudio.MVVM;
using System;

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

        public string Title
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

    public class StructCompletionDataViewModel : CompletionDataViewModel
    {
        public StructCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class PropertyCompletionDataViewModel : CompletionDataViewModel
    {
        public PropertyCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class KeywordCompletionDataViewModel : CompletionDataViewModel
    {
        public KeywordCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class EnumCompletionDataViewModel : CompletionDataViewModel
    {
        public EnumCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class EnumConstantCompletionDataViewModel : CompletionDataViewModel
    {
        public EnumConstantCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class FieldCompletionDataViewModel : CompletionDataViewModel
    {
        public FieldCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class ParameterCompletionDataViewModel : CompletionDataViewModel
    {
        public ParameterCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

    public class DefaultCompletionDataViewModel : CompletionDataViewModel
    {
        public DefaultCompletionDataViewModel(CodeCompletionData model) : base(model)
        {
        }
    }

}