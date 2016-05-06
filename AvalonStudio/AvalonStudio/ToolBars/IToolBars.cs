﻿namespace AvalonStudio.ToolBars
{
    using AvalonStudio.Extensibility.MVVM;

    public interface IToolBars
    {
        IObservableCollection<IToolBar> Items {get;}
        bool Visible { get; set; }
    }
}