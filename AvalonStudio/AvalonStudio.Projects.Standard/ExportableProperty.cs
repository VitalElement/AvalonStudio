namespace AvalonStudio.Projects.Standard
{
	public class ExportableProperty<T>
	{
		public T Value { get; set; }
		public bool Exported { get; set; }
		public bool Global { get; set; }
	}
}