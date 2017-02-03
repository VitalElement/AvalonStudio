using System.ComponentModel;
using System.Diagnostics;

namespace AvalonStudio.MVVM.DataVirtualization
{
	public class DataWrapper<T> : INotifyPropertyChanged where T : class
	{
		private T data;

		public DataWrapper(int index)
		{
			Index = index;
		}

		public int Index { get; }

		public int ItemNumber
		{
			get { return Index + 1; }
		}

		public bool IsLoading
		{
			get { return Data == null; }
		}

		public T Data
		{
			get { return data; }
			internal set
			{
				data = value;
				OnPropertyChanged("Data");
				OnPropertyChanged("IsLoading");
			}
		}

		public bool IsInUse
		{
			get { return PropertyChanged != null; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}