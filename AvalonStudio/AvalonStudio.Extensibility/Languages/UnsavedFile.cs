namespace AvalonStudio.Languages
{
	public class UnsavedFile
	{
		public readonly string FileName;
		public string Contents;

		public UnsavedFile(string filename, string contents)
		{
			FileName = filename;
			Contents = contents;
		}
	}
}