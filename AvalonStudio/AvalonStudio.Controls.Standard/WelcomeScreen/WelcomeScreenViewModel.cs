using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.WelcomeScreen {
    public class WelcomeScreenViewModel : DocumentTabViewModel, IExtension {

        private ObservableCollection<string> templates;

        public WelcomeScreenViewModel() {
            Title = "Welcome Screen";

            templates = new ObservableCollection<string>();

            for (int i = 0; i < 10; i++) {
                templates.Add("i = " + i);
            }

        }

        public ObservableCollection<string> Templates
        {
            get { return templates; }
            set { this.RaiseAndSetIfChanged(ref templates, value); }
        }

        public void Activation() {
            IoC.Get<IShell>().AddDocument(this);
        }

        public void BeforeActivation() {

        }
    }
}
