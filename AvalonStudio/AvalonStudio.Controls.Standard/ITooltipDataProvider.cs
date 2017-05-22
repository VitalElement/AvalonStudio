using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard
{
    interface ITooltipDataProvider
    {
        Task<bool> GetTooltipDataAsync();
    }
}
