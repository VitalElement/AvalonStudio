using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
            var shell = IoC.Get<IShell>();
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

    public class SettingsCategoryViewModel
    {
        public string Title { get; set; }

        public ObservableCollection<SettingsViewModel> Dialogs { get; set; }
    }
}
