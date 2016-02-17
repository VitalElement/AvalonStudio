﻿using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ComponentSettingsForm : TabItem
    {
        public ComponentSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
