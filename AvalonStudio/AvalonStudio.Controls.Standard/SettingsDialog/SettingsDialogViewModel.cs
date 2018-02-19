using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.Settings;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvalonStudio.Controls.Standard.SettingsDialog
{
    public class SettingsDialogViewModel : DocumentTabViewModel, IExtension, ISettingsManager
    {
        private Dictionary<string, SettingsCategoryViewModel> _categories = new Dictionary<string, SettingsCategoryViewModel>();
        private ObservableCollection<SettingsCategoryViewModel> _categoryViewModels = new ObservableCollection<SettingsCategoryViewModel>();
        private SettingsViewModel _selectedSetting;

        public SettingsDialogViewModel()
        {
            Title = "Options";
        }

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<ISettingsManager>(this);
            IoC.RegisterConstant(this);
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

        public SettingsViewModel SelectedSetting
        {
            get { return _selectedSetting; }
            set
            {
                if (value is SettingsViewModel)
                {
                    this.RaiseAndSetIfChanged(ref _selectedSetting, value);

                    _selectedSetting.OnDialogLoaded();
                }
            }
        }
    }
}
