using Avalonia.Input;
using AvalonStudio.Commands;
using AvalonStudio.Controls;
using AvalonStudio.Docking;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Languages;
using AvalonStudio.MainMenu;
using AvalonStudio.Menus.ViewModels;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.Toolbars;
using AvalonStudio.Toolbars.ViewModels;
using Dock.Model;
using Dock.Model.Controls;
using Dock.Serializer;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace AvalonStudio
{
    [Export(typeof(ShellViewModel))]
    [Export(typeof(IShell))]
    [Shared]
    public class ShellViewModel : ViewModel, IShell
    {
        public static ShellViewModel Instance { get; set; }
        private double _globalZoomLevel;
        private List<KeyBinding> _keyBindings;

        private IEnumerable<ToolbarViewModel> _toolbars;
        private IEnumerable<Lazy<IExtension>> _extensions;
        private IEnumerable<Lazy<ToolViewModel>> _toolControls;
        private CommandService _commandService;
        private List<IDocumentTabViewModel> _documents;

        private Lazy<StatusBarViewModel> _statusBar;

        private ModalDialogViewModelBase modalDialog;

        private QuickCommanderViewModel _quickCommander;

        private DocumentDock _documentDock;
        private ToolDock _leftPane;
        private ToolDock _rightPane;
        private ToolDock _bottomPane;

        private IDockFactory _factory;
        private IDock _layout;

        [ImportingConstructor]
        public ShellViewModel(
            CommandService commandService,
            Lazy<StatusBarViewModel> statusBar,
            IContentTypeService contentTypeService,
            MainMenuService mainMenuService,
            ToolbarService toolbarService,
            [ImportMany] IEnumerable<Lazy<IExtension>> extensions,
            [ImportMany] IEnumerable<Lazy<ToolViewModel>> toolControls)
        {
            _extensions = extensions;
            _toolControls = toolControls;

            _commandService = commandService;

            MainMenu = mainMenuService.GetMainMenu();

            var toolbars = toolbarService.GetToolbars();
            StandardToolbar = toolbars.Single(t => t.Key == "Standard").Value;

            _statusBar = statusBar;
            //IoC.RegisterConstant<IStatusBar>(_statusBar.Value);            

            _keyBindings = new List<KeyBinding>();

            //IoC.RegisterConstant<IShell>(this);
            //IoC.RegisterConstant(this);

            var factory = new DefaultLayoutFactory();
            Factory = factory;

            ModalDialog = new ModalDialogViewModelBase("Dialog");

            QuickCommander = new QuickCommanderViewModel();

            ProcessCancellationToken = new CancellationTokenSource();

            EnableDebugModeCommand = ReactiveCommand.Create(() =>
            {
                DebugMode = !DebugMode;
            });

            _documents = new List<IDocumentTabViewModel>();
        }

        public void Initialise()
        {
            foreach (var extension in _extensions)
            {
                if (extension.Value is IActivatableExtension activatable)
                {
                    activatable.BeforeActivation();
                }
            }

            LoadLayout();

            Layout.WhenAnyValue(l => l.FocusedView).Subscribe(focused =>
            {
                if (focused is IDocumentTabViewModel doc)
                {
                    SelectedDocument = doc;
                }
                else
                {
                    SelectedDocument = null;
                }
            });

            _leftPane = (Factory as DefaultLayoutFactory).LeftDock;
            _documentDock = (Factory as DefaultLayoutFactory).DocumentDock;
            _rightPane = (Factory as DefaultLayoutFactory).RightDock;
            _bottomPane = (Factory as DefaultLayoutFactory).BottomDock;

            foreach (var extension in _extensions)
            {
                if (extension.Value is IActivatableExtension activatable)
                {
                    activatable.Activation();
                }
            }

            foreach (var command in _commandService.GetKeyGestures())
            {
                foreach (var keyGesture in command.Value)
                {
                    _keyBindings.Add(new KeyBinding { Command = command.Key.Command, Gesture = KeyGesture.Parse(keyGesture) });
                }
            }

            foreach (var tool in _toolControls)
            {
                switch (tool.Value.DefaultLocation)
                {
                    case Location.Bottom:
                        DockView(_bottomPane, tool.Value);
                        break;

                    //case Location.BottomRight:
                    //    BottomRightTabs.Tools.Add(tool);
                    //    break;

                    //case Location.RightBottom:
                    //    RightBottomTabs.Tools.Add(tool);
                    //    break;

                    //case Location.RightMiddle:
                    //    RightMiddleTabs.Tools.Add(tool);
                    //    break;

                    //case Location.RightTop:
                    //    RightTopTabs.Tools.Add(tool);
                    //    break;

                    //case Location.MiddleTop:
                    //    MiddleTopTabs.Tools.Add(tool);
                    //    break;

                    case Location.Left:
                        DockView(_leftPane, tool.Value);
                        break;

                    case Location.Right:
                        DockView(_rightPane, tool.Value);
                        break;
                }
            }

            IoC.Get<IStatusBar>().ClearText();

            var editorSettings = Settings.GetSettings<EditorSettings>();

            _globalZoomLevel = editorSettings.GlobalZoomLevel;

            this.WhenAnyValue(x => x.GlobalZoomLevel).Subscribe(zoomLevel =>
            {
                foreach (var document in Documents.OfType<EditorViewModel>())
                {
                    document.ZoomLevel = zoomLevel;
                }
            });

            this.WhenAnyValue(x => x.GlobalZoomLevel).Throttle(TimeSpan.FromSeconds(2)).Subscribe(zoomLevel =>
            {
                var settings = Settings.GetSettings<EditorSettings>();

                settings.GlobalZoomLevel = zoomLevel;

                Settings.SetSettings(settings);
            });
        }

        private void DockView(IDock dock, IView view, bool add = true)
        {
            if (add)
            {
                dock.Views.Add(view);
                Factory.Update(view, view, dock);
            }
            else
            {
                Factory.Update(view, view, view.Parent);
            }

            Factory.Select(view);
        }

        public IReadOnlyList<IDocumentTabViewModel> Documents => _documents.AsReadOnly();

        public ReactiveCommand EnableDebugModeCommand { get; }



        public IDockFactory Factory
        {
            get => _factory;
            set => this.RaiseAndSetIfChanged(ref _factory, value);
        }

        public IDock Layout
        {
            get => _layout;
            set => this.RaiseAndSetIfChanged(ref _layout, value);
        }

        public void LoadLayout()
        {
            string path = System.IO.Path.Combine(Platform.SettingsDirectory, "Layout.json");

            if (DockSerializer.Exists(path))
            {
                //Layout = DockSerializer.Load<RootDock>(path);
            }

            if (Layout == null)
            {
                Layout = Factory.CreateLayout();
                Factory.InitLayout(Layout, this);
            }
        }

        public void CloseLayout()
        {
            Factory.CloseLayout(Layout);
        }

        public void SaveLayout()
        {
            string path = System.IO.Path.Combine(Platform.SettingsDirectory, "Layout.json");
            DockSerializer.Save(path, Layout);
        }

        public MenuViewModel MainMenu { get; }

        public StatusBarViewModel StatusBar => _statusBar.Value;

        public IEnumerable<ToolbarViewModel> Toolbars => _toolbars;

        private ToolbarViewModel StandardToolbar { get; }

        public IEnumerable<KeyBinding> KeyBindings => _keyBindings;

        public CancellationTokenSource ProcessCancellationToken { get; private set; }


        public void AddDocument(IDocumentTabViewModel document, bool temporary = false)
        {
            DockView(_documentDock, document, !Documents.Contains(document));

            _documents.Add(document);
        }

        public void RemoveDocument(IDocumentTabViewModel document)
        {
            if (document == null)
            {
                return;
            }

            if (document is EditorViewModel doc)
            {
                doc.Editor?.Save();
            }

            if (document.Parent is IDock dock)
            {
                dock.Views.Remove(document);
                Factory.Update(document, document, dock);
            }

            _documents.Remove(document);
        }



        public void ShowQuickCommander()
        {
            this._quickCommander.IsVisible = true;
        }

        public ModalDialogViewModelBase ModalDialog
        {
            get { return modalDialog; }
            set { this.RaiseAndSetIfChanged(ref modalDialog, value); }
        }

        public QuickCommanderViewModel QuickCommander
        {
            get { return _quickCommander; }
            set { this.RaiseAndSetIfChanged(ref _quickCommander, value); }
        }

        private ColorScheme _currentColorScheme;

        public ColorScheme CurrentColorScheme
        {
            get { return _currentColorScheme; }

            set
            {
                this.RaiseAndSetIfChanged(ref _currentColorScheme, value);

                foreach (var document in Documents.OfType<EditorViewModel>())
                {
                    document.ColorScheme = value;
                }
            }
        }

        public IDocumentTabViewModel SelectedDocument
        {
            get => Layout.FocusedView as IDocumentTabViewModel;
            set
            {
                if (value != null)
                {
                    Factory.Select(value);
                }

                this.RaisePropertyChanged(nameof(SelectedDocument));
            }
        }

        public double GlobalZoomLevel
        {
            get { return _globalZoomLevel; }
            set { this.RaiseAndSetIfChanged(ref _globalZoomLevel, value); }
        }

        public bool DebugMode { get; set; }
    }
}