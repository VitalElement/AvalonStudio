using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;

namespace AvalonStudio.Controls.Standard.CodeEditor.Completion
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

        public DrawingGroup Icon { get; private set; }

        public string Hint
        {
            get { return Model?.Hint; }
        }

        public string Comment
        {
            get { return Model?.BriefComment; }
        }
    }
}