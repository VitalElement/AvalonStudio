using AvalonStudio.Documents;
using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public static class ICompletionAssistantExtensions
    {
        public static void UpdateActiveParameterAndVisibility(this ICompletionAssistant completionAssistant, int caretIndex, IEditor editor)
        {
            if (completionAssistant.CurrentSignatureHelp != null)
            {
                var indexStack = new Stack<int>();

                int index = 0;
                int level = -1;
                int offset = completionAssistant.CurrentSignatureHelp.Offset;

                while (offset < caretIndex)
                {
                    var curChar = '\0';

                    curChar = editor.Document.GetCharAt(offset++);

                    switch (curChar)
                    {
                        case ',':
                            if (level == 0)
                            {
                                index++;
                            }
                            break;

                        case '(':
                            level++;

                            indexStack.Push(index);
                            index = 0;
                            break;

                        case ')':
                            if (level >= 0)
                            {
                                index = indexStack.Pop();
                            }

                            level--;
                            break;
                    }
                }

                if (index >= 0 && level >= 0)
                {
                    completionAssistant.SetParameterIndex(index);
                }
                else
                {
                    completionAssistant.Close();
                }
            }
        }
    }

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