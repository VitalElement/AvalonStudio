namespace AvalonStudio.Controls
{
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System.Collections.Generic;
    using System;

    public class CompletionAssistantViewModel : ViewModel, ICompletionAssistant
    {
        private Stack<MethodInfoViewModel> methodStack;
        private IntellisenseViewModel intellisense;

        public CompletionAssistantViewModel(IntellisenseViewModel intellisense)
        {
            methodStack = new Stack<MethodInfoViewModel>();
            this.intellisense = intellisense;
        }

        public void PushMethod(MethodInfo methodInfo)
        {
            if (CurrentMethod != null)
            {
                methodStack.Push(CurrentMethod);
            }

            CurrentMethod = new MethodInfoViewModel(methodInfo);

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

        public MethodInfo CurrentMethodInfo
        {
            get
            {
                return CurrentMethod?.Model;
            }
        }

        public void SetArgumentIndex(int index)
        {
            CurrentMethod.SelectedOverload.ArgumentIndex = index;
        }

        public void IncrementOverloadIndex()
        {
            CurrentMethod.SelectedIndex++;
        }

        public void DecrementOverloadIndex()
        {
            CurrentMethod.SelectedIndex--;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { this.RaiseAndSetIfChanged(ref isVisible, value); intellisense.InvalidateIsOpen(); }
        }

        private MethodInfoViewModel currentMethod;
        public MethodInfoViewModel CurrentMethod
        {
            get { return currentMethod; }
            set { this.RaiseAndSetIfChanged(ref currentMethod, value); }
        }

    }
}
