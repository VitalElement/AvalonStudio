using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.SettingsDialog
{
    public interface ISettingsManager
    {
        void RegisterSettingsDialog(string category, SettingsViewModel viewModel);
    }
}
