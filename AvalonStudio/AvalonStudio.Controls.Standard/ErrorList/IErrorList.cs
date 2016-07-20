using System.Collections.ObjectModel;

namespace AvalonStudio.Controls.Standard.ErrorList
{
	public interface IErrorList
	{
		ObservableCollection<ErrorViewModel> Errors { get; }
	}
}