﻿namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Styling;
    using System;
    using Perspex.Input;

    public class MetroWindow : Window, IStyleable
    {
        public MetroWindow()
        {
        }

        Type IStyleable.StyleKey => typeof(MetroWindow);

        private Grid titleBar;
        private Border windowBorder;
        private Button restoreButton;
        private Button closeButton;

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            if(titleBar.IsPointerOver)
            {
                BeginMoveDrag();
            }

            base.OnPointerPressed(e);
        }

        protected override void OnTemplateApplied(INameScope nameScope)
        {
            titleBar = nameScope.Find<Grid>("titlebar");
            windowBorder = nameScope.Find<Border>("windowBorder");
            restoreButton = nameScope.Find<Button>("restoreButton");
            closeButton = nameScope.Find<Button>("closeButton");

            restoreButton.Click += (sender, e) =>
            {
                
            };

            closeButton.Click += (sender, e) =>
            {
                Close();
            };

            windowBorder.Cursor = new Cursor(StandardCursorType.SizeAll);
            base.OnTemplateApplied(nameScope);
        }
    }
}
