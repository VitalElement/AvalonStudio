namespace AvalonStudio.Controls
{
    using Perspex.Controls.Presenters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class ToolControl : ContentPresenter
    {
        public ToolControl()
        {
            Styles.Add(new ControlTheme());
        }
    }
}
