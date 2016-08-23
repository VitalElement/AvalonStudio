namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public interface ICompletionAssistant
    {
        void PushMethod(MethodInfo methodInfo);

        void PopMethod();
    }
}
