﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class SignatureView : UserControl
    {
        public SignatureView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
