﻿namespace AvalonStudio.Debugging
{
    using Perspex.Controls;
    using Perspex;

    public class WatchListView : UserControl
    {
        public WatchListView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
