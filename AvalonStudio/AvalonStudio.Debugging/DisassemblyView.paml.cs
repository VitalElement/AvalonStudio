using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections;

namespace AvalonStudio.Debugging
{
    public class DisassemblyView : UserControl
    {
        private ListBox disassemblyList;

        public DisassemblyView()
        {
            InitializeComponent();

            disassemblyList = this.FindControl<ListBox>("disassemblyList");

            disassemblyList.SelectionChanged += (sender, e) =>
            {
                var list = disassemblyList.Items as IList;

                if (list.Count > 0)
                {
                    if (list.Count >= disassemblyList.SelectedIndex + 8)
                    {
                        disassemblyList.ScrollIntoView(list[disassemblyList.SelectedIndex + 8]);
                    }

                    if (disassemblyList.SelectedIndex >= 8)
                    {
                        disassemblyList.ScrollIntoView(list[disassemblyList.SelectedIndex - 8]);
                    }
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}