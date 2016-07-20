using System;

namespace AvalonStudio.Projects
{
	public enum Language
	{
		C,
		Cpp
	}

	public interface ISourceFile : IProjectItem, IComparable<ISourceFile>
	{
		string File { get; }
		string CurrentDirectory { get; }
		string Location { get; }
		Language Language { get; }
	}
}