using Avalonia.Media;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;

namespace AvalonStudio.Controls.Editor.Completion
{
    public class CompletionDataViewModel : ViewModel<CodeCompletionData>
    {
        public CompletionDataViewModel(CodeCompletionData model) : base(model)
        {
            Overloads = 0;

            Icon = model.Kind.ToDrawingGroup();
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
            get { return Model.DisplayText; }
        }

        public string FilterText => Model.FilterText;

        public int Priority
        {
            get { return Model.Priority; }
        }

        public string Kind
        {
            get { return Model.Kind.ToString(); }
        }

        public DrawingGroup Icon { get; private set; }

        public string Hint
        {
            get { return Model?.DisplayText; }
        }

        public string Comment
        {
            get { return Model?.BriefComment; }
        }
    }
}