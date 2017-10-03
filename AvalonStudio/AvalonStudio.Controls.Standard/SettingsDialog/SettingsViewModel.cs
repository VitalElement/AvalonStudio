using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SettingsDialog
{
    public class SettingsViewModel : ReactiveObject
    {
        private string _title;

        public SettingsViewModel(string title)
        {
            _title = title;
        }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

    }
}
