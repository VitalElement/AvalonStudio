using Avalonia;
using AvalonStudio.Languages;
using AvalonStudio.Languages.ViewModels;
using AvalonStudio.MVVM;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class IntellisenseViewModel : ViewModel, IDisposable
    {
        private IList<CompletionDataViewModel> completionData;
        private EditorViewModel editorViewModel;

        private bool isVisible;

        private Thickness position;

        private CompletionDataViewModel selectedCompletion;

        public IntellisenseViewModel(EditorViewModel viewModel)
        {
            completionData = new List<CompletionDataViewModel>();
            completionAssistant = new CompletionAssistantViewModel(this);
            editorViewModel = viewModel;
            isVisible = false;
        }

        private CompletionAssistantViewModel completionAssistant;

        public CompletionAssistantViewModel CompletionAssistant
        {
            get { return completionAssistant; }
            set { this.RaiseAndSetIfChanged(ref completionAssistant, value); }
        }

        public Thickness Position
        {
            get { return position; }
            set { this.RaiseAndSetIfChanged(ref position, value); }
        }

        public void Dispose()
        {
            editorViewModel = null;
        }

        public async Task<CodeCompletionResults> DoCompletionRequestAsync(int index, int line, int column)
        {
            return null;
            //return await Model.DoCompletionRequestAsync(index, line, column);
        }

        public CompletionDataViewModel SelectedCompletion
        {
            get { return selectedCompletion; }
            set { this.RaiseAndSetIfChanged(ref selectedCompletion, value); }
        }

        public void InvalidateIsOpen()
        {
            if (IsVisible || CompletionAssistant.IsVisible)
            {
                IsOpen = true;
            }
            else
            {
                IsOpen = false;
            }
        }

        private bool isOpen;

        public bool IsOpen
        {
            get { return isOpen; }
            set { this.RaiseAndSetIfChanged(ref isOpen, value); }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isVisible, value);
                InvalidateIsOpen();
            }
        }

        public IList<CompletionDataViewModel> CompletionData
        {
            get { return completionData; }
            set { this.RaiseAndSetIfChanged(ref completionData, value); }
        }
    }
}