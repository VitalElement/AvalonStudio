namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public interface ICompletionAssistant
    {
        void PushMethod(SignatureHelp methodInfo);

        void PopMethod();

        SignatureHelp CurrentSignatureHelp { get; }

        void SetParameterIndex(int index);

        void IncrementSignatureIndex();
        void DecrementSignatureIndex();
        void Close();

        bool IsVisible { get; set; }
    }
}
