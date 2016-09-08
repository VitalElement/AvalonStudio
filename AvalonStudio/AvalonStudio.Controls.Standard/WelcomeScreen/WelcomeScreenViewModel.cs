using System.Collections.ObjectModel;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Shell;

namespace AvalonStudio.Controls.Standard.WelcomeScreen {
    public class WelcomeScreenViewModel : ModalDialogViewModelBase {

        private ObservableCollection<string> solutionPaths;

        public WelcomeScreenViewModel() : base("Welcome Screen") {
            var shell = IoC.Get<IShell>();

            solutionPaths = new ObservableCollection<string>();

            for (int i = 0; i < 5; i++) {
                solutionPaths.Add("I = " + i);
            }

        }
    }
}
