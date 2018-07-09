using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Settings;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;

namespace AvalonStudio.Controls.Standard.SettingsDialog
{
    [Export(typeof(ISettingsManager))]
    [Export(typeof(SettingsDialogViewModel))]
    [Shared]
    public class SettingsDialogViewModel : DocumentTabViewModel, IActivatableExtension, ISettingsManager
    {
        private Dictionary<string, SettingsCategoryViewModel> _categories = new Dictionary<string, SettingsCategoryViewModel>();
        private ObservableCollection<SettingsCategoryViewModel> _categoryViewModels = new ObservableCollection<SettingsCategoryViewModel>();
        private object _selectedSetting;

        public SettingsDialogViewModel()
        {
            Title = "Options";
        }

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public void RegisterSettingsDialog(string category, SettingsViewModel viewModel)
        {
            if (!_categories.ContainsKey(category))
            {
                _categories[category] = new SettingsCategoryViewModel { Title = category, Dialogs = new ObservableCollection<SettingsViewModel>() };
                _categoryViewModels.Add(_categories[category]);
            }

            _categories[category].Dialogs.Add(viewModel);

            if (_categoryViewModels.Count == 1)
            {
                SelectedSetting = viewModel;

                _categories[category].IsExpanded = true;
            }
        }

        public ObservableCollection<SettingsCategoryViewModel> Categories => _categoryViewModels;

        public object SelectedSetting
        {
            get { return _selectedSetting; }
            set
            {
                if (value is SettingsViewModel setting)
                {
                    setting.OnDialogLoaded();

                    this.RaiseAndSetIfChanged(ref _selectedSetting, value);
                }
                else
                {
                    _selectedSetting = value;
                }
            }
        }
    }
}
