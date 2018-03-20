using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class TextTemplateParameter : UserControl
    {
        public TextTemplateParameter()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if(DataContext is TemplateParameterViewModel pvm)
            {
                if(pvm.Name == "Name")
                {
                    var textBox = this.FindControl<TextBox>("PART_ValueTextBlock");

                    textBox?.Focus();                    
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.SelectionEnd = 0;
                    textBox.CaretIndex = textBox.Text.Length - 1;
                }
            }
            base.OnTemplateApplied(e);
        }
    }
}
