﻿using AvaloniaDemo.ViewModels.Views;
using Dock.Avalonia.Controls;
using Dock.Model;
using Dock.Model.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvalonStudio.Docking
{
    /// <inheritdoc/>
    public class DefaultLayoutFactory : DockFactory
    {

        public DocumentDock DocumentDock { get; private set; }
        public ToolDock LeftDock { get; private set; }
        public ToolDock RightDock { get; private set; }
        public ToolDock BottomDock { get; private set; }

        /// <inheritdoc/>
        public override IDock CreateLayout()
        {
            // Left Pane

            LeftDock = new ToolDock
            {
                Id = "LeftPaneTop",
                Dock = "Left",
                Width = 340,
                Height = double.NaN,
                Title = "LeftPaneTop",
                CurrentView = null,
                Views = new ObservableCollection<IView>()
            };

            // Right Pane

            RightDock = new ToolDock
            {
                Id = "RightDock",
                Dock = "Right",
                Width = 300,
                Height = double.NaN,
                Title = "RightDock",
                CurrentView = null,
                Views = new ObservableCollection<IView>()
            };

            BottomDock = new ToolDock
            {
                Id = "BottomDock",
                Dock = "Bottom",
                Width = double.NaN,
                Height = 300,
                Title = "BottomDock",
                CurrentView = null,
                Views = new ObservableCollection<IView>()
            };

            // Documents

            DocumentDock = new DocumentDock
            {
                Id = "DocumentsPane",
                Dock = "",
                Width = double.NaN,
                Height = double.NaN,
                Title = "DocumentsPane",
                CurrentView = null,
                Views = new ObservableCollection<IView>()
            };

            // Main

            var mainLayout = new LayoutDock
            {
                Id = "MainLayout",
                Dock = "",
                Width = double.NaN,
                Height = double.NaN,
                Title = "MainLayout",
                CurrentView = null,
                Views = new ObservableCollection<IView>
                {
                    LeftDock,
                    new SplitterDock()
                    {
                        Id = "LeftSplitter",
                        Dock = "Left",
                        Title = "LeftSplitter"
                    },
                    RightDock,
                    new SplitterDock()
                    {
                        Id = "RightSplitter",
                        Dock = "Right",
                        Title = "RightSplitter"
                    },
                    BottomDock,
                    new SplitterDock()
                    {
                        Id = "BottomSplitter",
                        Dock = "Bottom",
                        Title = "BottomSplitter"
                    },
                    DocumentDock
                }
            };

            var mainView = new MainView
            {
                Id = "Main",
                Width = double.NaN,
                Height = double.NaN,
                Title = "Main",
                CurrentView = mainLayout,
                Views = new ObservableCollection<IView>
                {
                   mainLayout
                }
            };

            // Root

            var root = new RootDock
            {
                Id = "Root",
                Width = double.NaN,
                Height = double.NaN,
                Title = "Root",
                CurrentView = mainView,
                DefaultView = mainView,
                Views = new ObservableCollection<IView>
                {
                    mainView,
                }
            };

            return root;
        }

        /// <inheritdoc/>
        public override void InitLayout(IView layout, object context)
        {
            this.ContextLocator = new Dictionary<string, Func<object>>
            {
                // Defaults
                [nameof(IRootDock)] = () => context,
                [nameof(ILayoutDock)] = () => context,
                [nameof(IDocumentDock)] = () => context,
                [nameof(IToolDock)] = () => context,
                [nameof(ISplitterDock)] = () => context,
                [nameof(IDockWindow)] = () => context,
                // Documents
                ["Document1"] = () => context,
                ["Document2"] = () => context,
                ["Document3"] = () => context,
                // Tools
                ["Editor"] = () => layout,
                ["LeftTop1"] = () => context,
                ["LeftTop2"] = () => context,
                ["LeftTop3"] = () => context,
                ["LeftBottom1"] = () => context,
                ["LeftBottom2"] = () => context,
                ["LeftBottom3"] = () => context,
                ["RightTop1"] = () => context,
                ["RightTop2"] = () => context,
                ["RightTop3"] = () => context,
                ["RightBottom1"] = () => context,
                ["RightBottom2"] = () => context,
                ["RightBottom3"] = () => context,
                ["LeftPane"] = () => context,
                ["LeftPaneTop"] = () => context,
                ["LeftPaneTopSplitter"] = () => context,
                ["LeftPaneBottom"] = () => context,
                ["RightPane"] = () => context,
                ["RightPaneTop"] = () => context,
                ["RightPaneTopSplitter"] = () => context,
                ["RightPaneBottom"] = () => context,
                ["DocumentsPane"] = () => context,
                ["MainLayout"] = () => context,
                ["LeftSplitter"] = () => context,
                ["RightSplitter"] = () => context,
                // Layouts
                ["MainLayout"] = () => context,
                // Views
                ["Home"] = () => layout,
                ["Main"] = () => context
            };

            this.HostLocator = new Dictionary<string, Func<IDockHost>>
            {
                [nameof(IDockWindow)] = () => new HostWindow()
            };

            this.Update(layout, context, null);

            if (layout is IWindowsHost layoutWindowsHost)
            {
                layoutWindowsHost.ShowWindows();
                if (layout is IViewsHost layoutViewsHost)
                {
                    layoutViewsHost.CurrentView = layoutViewsHost.DefaultView;
                    if (layoutViewsHost.CurrentView is IWindowsHost currentViewWindowsHost)
                    {
                        currentViewWindowsHost.ShowWindows();
                    }
                }
            }
        }
    }
}
