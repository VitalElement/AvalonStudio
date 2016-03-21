using Perspex;
using Perspex.Controls;
using Perspex.Controls.Primitives;
using Perspex.Media;
using Perspex.Xaml.Interactivity;

namespace AvalonStudio.Actions
{
    public class PopupAction : PerspexObject, IAction
    {
        public object Execute(object sender, object parameter)
        {            
            WorkspaceViewModel.Instance.Editor.OnPointerHover();

            return null;
        }
    }
}

