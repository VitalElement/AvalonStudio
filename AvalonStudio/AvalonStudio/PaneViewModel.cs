using System;
using ReactiveUI;

namespace AvalonStudio
{
	public abstract class PaneViewModel : ReactiveObject
	{
		//private Visibility visibility;
		//public Visibility Visibility
		//{
		//    get { return visibility; }
		//    set { visibility = value; this.RaisePropertyChanged(); }
		//}


		private bool isCollapsed;

		public PaneViewModel(string contentId)
		{
			ContentId = contentId;
		}

		public virtual Uri IconSource { get; protected set; }

		public bool IsCollapsed
		{
			get { return isCollapsed; }
			set
			{
				isCollapsed = value;
				this.RaisePropertyChanged();
			}
		}

		#region Title

		private string title;

		public string ToolTitle
		{
			get { return title; }
			set
			{
				title = value;
				this.RaisePropertyChanged();
			}
		}

		#endregion

		#region ContentId

		private string contentId;

		public string ContentId
		{
			get { return contentId; }

			set
			{
				contentId = value;
				this.RaisePropertyChanged();
			}
		}

		#endregion

		#region IsSelected

		private bool isSelected;

		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				isSelected = value;
				this.RaisePropertyChanged();
			}
		}

		#endregion

		#region IsActive

		private bool isActive;

		public bool IsActive
		{
			get { return isActive; }

			set
			{
				isActive = value;
				this.RaisePropertyChanged();
			}
		}

		#endregion
	}
}