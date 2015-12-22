namespace AvalonStudio
{
    using Controls;
    using Perspex.Controls;
    using Perspex.Input;
    using Perspex.Markup.Xaml;

    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            App.AttachDevTools(this);

            var names = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceNames();

            foreach(var name in names)
            {
                System.Console.WriteLine(name);
            }
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }        
    }
}
