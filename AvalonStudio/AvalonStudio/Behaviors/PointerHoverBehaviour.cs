using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

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