﻿using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Projects.VEBuild
{
    public class DebuggerSettingsForm : TabItem
    {
        public DebuggerSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
