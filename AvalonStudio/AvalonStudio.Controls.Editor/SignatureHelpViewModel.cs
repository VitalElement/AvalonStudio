namespace AvalonStudio.Controls.Editor
{
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System.Collections.Generic;

    public class SignatureHelpViewModel : ViewModel<SignatureHelp>
    {
        public SignatureHelpViewModel(SignatureHelp model) : base(model)
        {
            signatures = new List<SignatureViewModel>();

            foreach (var signature in model.Signatures)
            {
                signatures.Add(new SignatureViewModel(signature));
            }

            SelectedIndex = model.ActiveSignature;

            if (SelectedSignature != null)
            {
                SelectedSignature.ParameterIndex = model.ActiveParameter;
            }
        }

        public int SignatureCount
        {
            get
            {
                return Signatures.Count;
            }
        }

        public int SelectedSignatureIndex
        {
            get
            {
                return SelectedIndex + 1;
            }
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (value < 0 || value >= Signatures.Count)
                {
                    value = selectedIndex;
                }

                if (value < Signatures.Count)
                {
                    SelectedSignature = Signatures[value];
                }

                this.RaiseAndSetIfChanged(ref selectedIndex, value);
                this.RaisePropertyChanged(nameof(SelectedSignatureIndex));
            }
        }

        private List<SignatureViewModel> signatures;

        public List<SignatureViewModel> Signatures
        {
            get { return signatures; }
            set { this.RaiseAndSetIfChanged(ref signatures, value); }
        }

        private SignatureViewModel selectedSignature;

        public SignatureViewModel SelectedSignature
        {
            get { return selectedSignature; }
            set { this.RaiseAndSetIfChanged(ref selectedSignature, value); }
        }
    }
}