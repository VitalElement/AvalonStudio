using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Data;
using ReactiveUI;
using Splat;
using ViewLocator = AvalonStudio.MVVM.ViewLocator;

namespace AvalonStudio.Controls
{
	/// <summary>
	///     This content control will automatically load the View associated with
	///     the ViewModel property and display it. This control is very useful
	///     inside a DataTemplate to display the View associated with a ViewModel.
	/// </summary>
	public class ViewModelViewHost : ContentPresenter, IViewFor, IEnableLogger, IActivationForViewFetcher
	{
		public static readonly AvaloniaProperty ViewModelProperty =
			AvaloniaProperty.Register<ViewModelViewHost, object>(nameof(ViewModel), null, false, BindingMode.OneWay, null,
				somethingChanged);

		public static readonly AvaloniaProperty DefaultContentProperty =
			AvaloniaProperty.Register<ViewModelViewHost, object>(nameof(DefaultContent), null, false, BindingMode.OneWay, null,
				somethingChanged);

		public static readonly AvaloniaProperty ViewContractObservableProperty =
			AvaloniaProperty.Register<ViewModelViewHost, IObservable<string>>(nameof(ViewContractObservable),
				Observable.Return(default(string)));

		private readonly Subject<Unit> updateViewModel = new Subject<Unit>();

		private string viewContract;

		//public IViewLocator ViewLocator { get; set; }

		/// <summary>
		///     If no ViewModel is displayed, this content (i.e. a control) will be displayed.
		/// </summary>
		public object DefaultContent
		{
			get { return GetValue(DefaultContentProperty); }
			set { SetValue(DefaultContentProperty, value); }
		}


		public IObservable<string> ViewContractObservable
		{
			get { return (IObservable<string>) GetValue(ViewContractObservableProperty); }
			set { SetValue(ViewContractObservableProperty, value); }
		}

		public string ViewContract
		{
			get { return viewContract; }
			set { ViewContractObservable = Observable.Return(value); }
		}

		public int GetAffinityForView(Type view)
		{
			throw new NotImplementedException();
		}

		public IObservable<bool> GetActivationForView(IActivatable view)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     The ViewModel to display
		/// </summary>
		public object ViewModel
		{
			get { return GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		protected override void OnDataContextChanged()
		{
			Content = ViewLocator.Build(DataContext);
		}

		private static void somethingChanged(IAvaloniaObject dependencyObject, bool changed)
		{
			if (changed)
			{
				((ViewModelViewHost) dependencyObject).updateViewModel.OnNext(Unit.Default);
			}
		}
	}
}