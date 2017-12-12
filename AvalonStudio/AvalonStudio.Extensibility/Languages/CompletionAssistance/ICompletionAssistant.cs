using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public interface ICompletionAssistant
    {
        void PushMethod(SignatureHelp methodInfo);

        void PopMethod();

        IReadOnlyList<SignatureHelp> Stack { get; }

        SignatureHelp CurrentSignatureHelp { get; }

        void SelectStack(SignatureHelp stack);

        void SetParameterIndex(int index);

        void IncrementSignatureIndex();

        void DecrementSignatureIndex();

        void Close();

        bool IsVisible { get; set; }
    }
}