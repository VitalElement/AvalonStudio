using Perspex;
using Perspex.Controls;
using Perspex.Controls.Primitives;
using Perspex.Input;
using Perspex.Media;
using Perspex.Threading;
using Perspex.Xaml.Interactivity;
using System;

namespace AvalonStudio.Actions
{
    public class PopupAction : PerspexObject, IAction
    {
        private DispatcherTimer timer;

        public PopupAction()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            WorkspaceViewModel.Instance.Editor.OnPointerHover();
        }

        public object Execute(object sender, object parameter)
        {            
            if (parameter is PointerEventArgs)
            {
                timer.Stop();
                timer.Start();                
            }

            return null;
        }
    }
}

