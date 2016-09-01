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
                if(Overloads == 0)
                {
                    return string.Empty;
                }
                else if(Overloads == 1)
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
                case CodeCompletionKind.Class:
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