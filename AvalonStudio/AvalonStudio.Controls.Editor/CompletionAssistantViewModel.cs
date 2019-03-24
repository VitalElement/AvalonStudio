namespace AvalonStudio.Controls.Editor
{
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.Extensibility.Studio;
    using AvalonStudio.MVVM;
    using AvalonStudio.Shell;
    using AvalonStudio.Utils;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class CompletionAssistantViewModel : ViewModel, ICompletionAssistant
    {
        private List<SignatureHelpViewModel> methodVmStack;
        private List<SignatureHelp> methodStack;
        private IntellisenseViewModel intellisense;
        private Thread uiThread;

        public CompletionAssistantViewModel(IntellisenseViewModel intellisense)
        {
            uiThread = Thread.CurrentThread;
            methodStack = new List<SignatureHelp>();
            methodVmStack = new List<SignatureHelpViewModel>();
            this.intellisense = intellisense;
        }

        public IReadOnlyList<SignatureHelp> Stack => methodStack.AsReadOnly();

        public void PushMethod(SignatureHelp methodInfo)
        {
            CurrentMethod = new SignatureHelpViewModel(methodInfo);

            methodStack.Insert(0, CurrentMethod.Model);
            methodVmStack.Insert(0, CurrentMethod);

            IsVisible = true;

            if (IoC.Get<IStudio>().DebugMode)
            {
                IoC.Get<IConsole>().WriteLine($"[Signature Help] - PushMethod - {CurrentMethod.SelectedSignature.Name}");
            }
        }

        public void SelectStack(SignatureHelp stack)
        {
            CurrentMethod = methodVmStack.First(s => s.Model == stack);
        }

        public void PopMethod()
        {
            if (methodStack.Count > 0)
            {
                CurrentMethod = methodVmStack[0];

                methodVmStack.RemoveAt(0);
                methodStack.RemoveAt(0);
            }
            else
            {
                CurrentMethod = null;
                IsVisible = false;
            }

            if (IoC.Get<IStudio>().DebugMode)
            {
                IoC.Get<IConsole>().WriteLine($"[Signature Help] - PopMethod - {CurrentMethod?.SelectedSignature.Name ?? "null"}");
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
            if (CurrentMethod.SelectedSignature != null)
            {
                CurrentMethod.SelectedSignature.ParameterIndex = index;
            }
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
            methodVmStack.Clear();
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