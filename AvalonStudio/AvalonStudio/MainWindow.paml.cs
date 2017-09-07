using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility;
using System;

namespace AvalonStudio
{
    public class MainWindow : MetroWindow
    {
        private bool set = false;

        public MainWindow()
        {
            IoC.RegisterConstant<Window>(this);

            InitializeComponent();

            DataContext = ShellViewModel.Instance;

            KeyBindings.AddRange(IoC.Get<ShellViewModel>().KeyBindings);

            this.AttachDevTools();

            var timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (sender, e) =>
            {
                if(set)
                {
                    Resources["AvalonBorderBrush"] = Brushes.Red;
                }
                else
                {
                    Resources["AvalonBorderBrush"] = Brushes.Blue;
                }

                set = !set;
            };

            timer.Start();

            this.Resources.Add("AvalonBorderBrush", Brushes.Green);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}