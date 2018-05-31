using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AvaloniaDemo.ViewModels.Documents;
using AvaloniaDemo.ViewModels.Tools;
using AvaloniaDemo.ViewModels.Views;
using AvalonStudio.Controls.Standard.SolutionExplorer;
using AvalonStudio.Extensibility;
using Dock.Avalonia.Controls;
using Dock.Model;
using Dock.Model.Controls;

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
            // Documents

            var document1 = new Document1
            {
                Id = "Document1",
                Width = double.NaN,
                Height = double.NaN,
                Title = "Document1"
            };

            var document2 = new Document2
            {
                Id = "Document2",
                Width = double.NaN,
                Height = double.NaN,
                Title = "Document2"
            };

            var document3 = new Document3
            {
                Id = "Document3",
                Width = double.NaN,
                Height = double.NaN,
                Title = "Document3"
            };

            // Left / Top

            var leftTopTool1 = new LeftTopTool1
            {
                Id = "LeftTop1",
                Width = double.NaN,
                Height = double.NaN,
                Title = "LeftTop1"
            };

            var leftTopTool2 = new LeftTopTool2
            {
                Id = "LeftTop2",
                Width = 200,
                Height = 200,
                Title = "LeftTop2"
            };

            var leftTopTool3 = new LeftTopTool3
            {
                Id = "LeftTop3",
                Width = double.NaN,
                Height = double.NaN,
                Title = "LeftTop3"
            };

            // Left / Bottom

            var leftBottomTool1 = new LeftBottomTool1
            {
                Id = "LeftBottom1",
                Width = double.NaN,
                Height = double.NaN,
                Title = "LeftBottom1"
            };

            var leftBottomTool2 = new LeftBottomTool2
            {
                Id = "LeftBottom2",
                Width = double.NaN,
                Height = double.NaN,
                Title = "LeftBottom2"
            };

            var leftBottomTool3 = new LeftBottomTool3
            {
                Id = "LeftBottom3",
                Width = double.NaN,
                Height = double.NaN,
                Title = "LeftBottom3"
            };

            // Right / Top            

            var rightTopTool2 = new RightTopTool2
            {
                Id = "RightTop2",
                Width = double.NaN,
                Height = double.NaN,
                Title = "RightTop2"
            };

            var rightTopTool3 = new RightTopTool3
            {
                Id = "RightTop3",
                Width = double.NaN,
                Height = double.NaN,
                Title = "RightTop3"
            };

            // Right / Bottom

            var rightBottomTool2 = new RightBottomTool1
            {
                Id = "RightBottom1",
                Width = double.NaN,
                Height = double.NaN,
                Title = "RightBottom1"
            };

            var rightBottomTool1 = IoC.Get<ISolutionExplorer>();

            var rightBottomTool3 = new RightBottomTool3
            {
                Id = "RightBottom3",
                Width = double.NaN,
                Height = double.NaN,
                Title = "RightBottom3"
            };

            // Left Pane

            LeftDock = new ToolDock
            {
                Id = "LeftPaneTop",
                Dock = "Left",
                Width = 340,
                Height = double.NaN,
                Title = "LeftPaneTop",
                CurrentView = leftTopTool1,
                Views = new ObservableCollection<IView>
                        {
                            leftTopTool1,
                            leftTopTool2,
                            leftTopTool3
                        }
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
                Views = new ObservableCollection<IView>
                        {                            
                            rightTopTool2,
                            rightTopTool3
                        }
            };

            BottomDock = new ToolDock
            {
                Id = "BottomDock",
                Dock = "Bottom",
                Width = double.NaN,
                Height = 300,
                Title = "BottomDock",
                CurrentView = null,
                Views = new ObservableCollection<IView>
                        {
                            rightTopTool2,
                            rightTopTool3
                        }
            };

            // Documents

            DocumentDock = new DocumentDock
            {
                Id = "DocumentsPane",
                Dock = "",
                Width = double.NaN,
                Height = double.NaN,
                Title = "DocumentsPane",
                CurrentView = document1,
                Views = new ObservableCollection<IView>
                {
                    document1,
                    document2,
                    document3
                }
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
                    new SplitterDock()
                    {
                        Id = "BottomSplitter",
                        Dock = "Bottom",
                        Title = "BottomSplitter"
                    },
                    BottomDock,
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

            // Home

            var homeView = new HomeView
            {
                Id = "Home",
                Width = double.NaN,
                Height = double.NaN,
                Title = "Home"
            };

            // Root

            var root = new RootDock
            {
                Id = "Root",
                Width = double.NaN,
                Height = double.NaN,
                Title = "Root",
                CurrentView = homeView,
                DefaultView = homeView,
                Views = new ObservableCollection<IView>
                {
                    homeView,
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
