namespace AvalonStudio.MVVM
{
	using System.ComponentModel.Composition;
	using ReactiveUI;

	public interface IActivatable
	{
		void Activate();
	}

	public enum Location
	{
		Left,
		Right,
		Bottom,
		BottomRight,
		RightBottom,
		RightMiddle,
		RightTop,
	}


	[InheritedExport(typeof(ToolViewModel))]
	public abstract class ToolViewModel : ViewModel
	{
		protected ToolViewModel()
		{
			_isVisible = true;
		}

		private bool _isVisible;
		public bool IsVisible
		{
			get { return _isVisible; }
			set
			{
				this.RaiseAndSetIfChanged(ref _isVisible, value);
			}
		}

		public abstract Location DefaultLocation { get; }

		// TODO This should use ToolControl
		private string _title;
		public string Title
		{
			get { return _title; }
			set { this.RaiseAndSetIfChanged(ref _title, value); }
		}
	}

	public abstract class ToolViewModel<T> : ToolViewModel
	{
		protected ToolViewModel(T model)
		{
			this._model = model;
		}

		private T _model;
		public new T Model
		{
			get { return _model; }
			set { this.RaiseAndSetIfChanged(ref _model, value); }
		}
	}

	public abstract class ViewModel : ViewModel<object>
	{
		protected ViewModel() : base(null)
		{

		}
	}

	public abstract class HeaderedViewModel : HeaderedViewModel<object>
	{
		protected HeaderedViewModel(string header) : base(header, null)
		{
		}
	}

	public abstract class HeaderedViewModel<T> : ViewModel<T>
	{
		protected HeaderedViewModel(string header, T model) : base(model)
		{
			Title = header;
		}

		public string Title
		{
			get; private set;
		}
	}

	public abstract class ViewModel<T> : ReactiveObject
	{
		protected ViewModel(T model)
		{
			this._model = model;
		}

		private T _model;
		public T Model
		{
			get { return _model; }
			set { this.RaiseAndSetIfChanged(ref _model, value); Invalidate(); }
		}

		public void Invalidate()
		{
			this.RaisePropertyChanged("");
		}
	}
}
