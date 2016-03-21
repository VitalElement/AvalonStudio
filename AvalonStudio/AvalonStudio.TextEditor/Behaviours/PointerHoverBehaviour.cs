using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Perspex.Controls;
using Perspex.Input;
using Perspex.Xaml.Interactivity;

namespace AvalonStudio.Behaviors
{
    public class FocusOnPointerMovedBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PointerMoved += PointerMoved;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PointerMoved -= PointerMoved;
        }

        private void PointerMoved(object sender, PointerEventArgs args)
        {
            AssociatedObject.Focus();
        }
    }
}
