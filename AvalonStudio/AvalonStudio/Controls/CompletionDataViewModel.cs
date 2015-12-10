namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Models.LanguageServices;


    public class CompletionDataViewModel : ViewModel<CodeCompletionData>
    {
        public CompletionDataViewModel (CodeCompletionData model) : base (model)
        {

        }

        public string Title { get { return Model.Suggestion; } }
    }
}
