﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Perspex.Controls;
using Perspex.Input;
using Perspex.Xaml.Interactivity;
using AvalonStudio.TextEditor;

namespace AvalonStudio.Behaviors
{
    public class PointerHoverBehaviour : Behavior<TextEditor.TextEditor>
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
            //WorkspaceViewModel.Instance.Editor.OnPointerHover();
        }
    }
}
