﻿namespace AvalonStudio.Debugging
{
    using Perspex;
    using Perspex.Controls;

    public class RegistersView : UserControl
    {
        public RegistersView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
