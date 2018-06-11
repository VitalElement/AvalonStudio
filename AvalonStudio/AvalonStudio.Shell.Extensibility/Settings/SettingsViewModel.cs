using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvalonStudio.Extensibility.Settings
{
    public abstract class SettingsViewModel : ReactiveObject
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

        public virtual void OnDialogLoaded()
        {

        }
    }

    public class SettingsCategoryViewModel : ReactiveObject
    {
        private bool _isExpanded;

        public string Title { get; set; }

        public ObservableCollection<SettingsViewModel> Dialogs { get; set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { this.RaiseAndSetIfChanged(ref _isExpanded, value); }
        }
    }
}
