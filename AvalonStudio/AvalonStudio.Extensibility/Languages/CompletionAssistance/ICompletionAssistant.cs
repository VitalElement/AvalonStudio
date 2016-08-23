namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public interface ICompletionAssistant
    {
        void PushMethod(MethodInfo methodInfo);

        void PopMethod();

        MethodInfo CurrentMethodInfo { get; }

        void SetArgumentIndex(int index);

        bool IsVisible { get; set; }
    }
}
