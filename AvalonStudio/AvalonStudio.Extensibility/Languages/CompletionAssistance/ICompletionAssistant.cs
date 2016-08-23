namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public interface ICompletionAssistant
    {
        void PushMethod(MethodInfo methodInfo);

        void PopMethod();

        MethodInfo CurrentMethodInfo { get; }

        void SetArgumentIndex(int index);

        void IncrementOverloadIndex();
        void DecrementOverloadIndex();
        void Close();

        bool IsVisible { get; set; }
    }
}
