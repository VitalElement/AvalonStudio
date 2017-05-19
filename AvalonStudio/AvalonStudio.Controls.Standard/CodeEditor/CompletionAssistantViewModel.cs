namespace AvalonStudio.Controls.Standard.CodeEditor
{
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Threading;

    public class CompletionAssistantViewModel : ViewModel, ICompletionAssistant
    {
        private Stack<SignatureHelpViewModel> methodStack;
        private IntellisenseViewModel intellisense;
        private Thread uiThread;

        public CompletionAssistantViewModel(IntellisenseViewModel intellisense)
        {
            uiThread = Thread.CurrentThread;
            methodStack = new Stack<SignatureHelpViewModel>();
            this.intellisense = intellisense;
        }

        public void PushMethod(SignatureHelp methodInfo)
        {
            if (CurrentMethod != null)
            {
                methodStack.Push(CurrentMethod);
            }

            CurrentMethod = new SignatureHelpViewModel(methodInfo);

            IsVisible = true;
        }

        public void PopMethod()
        {
            if (methodStack.Count > 0)
            {
                CurrentMethod = methodStack.Pop();
            }
            else
            {
                CurrentMethod = null;
                IsVisible = false;
            }
        }

        public SignatureHelp CurrentSignatureHelp
        {
            get
            {
                return CurrentMethod?.Model;
            }
        }

        public void SetParameterIndex(int index)
        {
            CurrentMethod.SelectedSignature.ParameterIndex = index;
        }

        public void IncrementSignatureIndex()
        {
            CurrentMethod.SelectedIndex++;
        }

        public void DecrementSignatureIndex()
        {
            CurrentMethod.SelectedIndex--;
        }

        public void Close()
        {
            IsVisible = false;

            CurrentMethod = null;
            methodStack.Clear();
        }

        private bool isVisible;

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isVisible, value);
                intellisense.InvalidateIsOpen();
            }
        }

        private SignatureHelpViewModel currentMethod;

        public SignatureHelpViewModel CurrentMethod
        {
            get { return currentMethod; }
            set { this.RaiseAndSetIfChanged(ref currentMethod, value); }
        }
    }
}