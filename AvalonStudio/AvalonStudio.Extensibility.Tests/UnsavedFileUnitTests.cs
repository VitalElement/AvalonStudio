namespace AvalonStudio.Extensibility.Tests
{
    using AvalonStudio.Languages;
    using AvalonStudio.Utils;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class UnsavedFileUnitTests
    {
        [Fact]
        public void UnsavedFiles_Are_Sorted_Alphabetically()
        {
            List<UnsavedFile> unsavedFiles = new List<UnsavedFile>();

            unsavedFiles.InsertSorted(new UnsavedFile("hij", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("bcd", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("abc", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("ghi", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("cde", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("fgh", "my contents"));

            List<UnsavedFile> expected = new List<UnsavedFile>();
            expected.Add(new UnsavedFile("abc", "my contents"));
            expected.Add(new UnsavedFile("bcd", "my contents"));
            expected.Add(new UnsavedFile("cde", "my contents"));
            expected.Add(new UnsavedFile("fgh", "my contents"));
            expected.Add(new UnsavedFile("ghi", "my contents"));
            expected.Add(new UnsavedFile("hij", "my contents"));

            Assert.True(expected.Select(u => u.FileName).SequenceEqual(unsavedFiles.Select(u => u.FileName)));
        }

        [Fact]
        public void UnsavedFiles_Are_Sorted_Alphabetically_with_Complex_Paths()
        {
            List<UnsavedFile> unsavedFiles = new List<UnsavedFile>();

            unsavedFiles.InsertSorted(new UnsavedFile("G:\\development\\repos\\KHGD\\Modules\\CommonHal\\IFileSystem.h", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("G:\\development\\repos\\KHGD\\Gateway\\Gateway.h", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("G:\\development\\repos\\KHGD\\Gateway\\IPlatform.h", "my contents"));

            List<UnsavedFile> expected = new List<UnsavedFile>();
            expected.Add(new UnsavedFile("G:\\development\\repos\\KHGD\\Gateway\\Gateway.h", "my contents"));
            expected.Add(new UnsavedFile("G:\\development\\repos\\KHGD\\Gateway\\IPlatform.h", "my contents"));
            expected.Add(new UnsavedFile("G:\\development\\repos\\KHGD\\Modules\\CommonHal\\IFileSystem.h", "my contents"));

            Assert.True(expected.Select(u => u.FileName).SequenceEqual(unsavedFiles.Select(u => u.FileName)));
        }

        [Fact]
        public void UnsavedFiles_Are_Sorted_Alphabetically_When_Paths_Are_Equivalent_But_Not_Equal()
        {
            List<UnsavedFile> unsavedFiles = new List<UnsavedFile>();

            unsavedFiles.InsertSorted(new UnsavedFile("C:\\hij/", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("c:\\bcd", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("c:\\abc/", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("c:/ghi/", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("c:\\cde\\", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("C:\\fgh", "my contents"));

            List<UnsavedFile> expected = new List<UnsavedFile>();
            expected.Add(new UnsavedFile("c:\\abc/", "my contents"));
            expected.Add(new UnsavedFile("c:\\bcd", "my contents"));
            expected.Add(new UnsavedFile("c:\\cde\\", "my contents"));
            expected.Add(new UnsavedFile("C:\\fgh", "my contents"));
            expected.Add(new UnsavedFile("c:/ghi/", "my contents"));
            expected.Add(new UnsavedFile("C:\\hij/", "my contents"));

            Assert.True(expected.Select(u => u.FileName).SequenceEqual(unsavedFiles.Select(u => u.FileName)));
        }

        [Fact]
        public void UnsavedFiles_Can_Be_Binary_Searched()
        {
            List<UnsavedFile> unsavedFiles = new List<UnsavedFile>();

            var expected1 = new UnsavedFile("c:\\abc/", "my contents");
            var expected2 = new UnsavedFile("c:/ghi/", "my contents");
            unsavedFiles.InsertSorted(new UnsavedFile("C:\\hij/", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("c:\\bcd", "my contents"));
            unsavedFiles.InsertSorted(expected1);
            unsavedFiles.InsertSorted(expected2);
            unsavedFiles.InsertSorted(new UnsavedFile("c:\\cde\\", "my contents"));
            unsavedFiles.InsertSorted(new UnsavedFile("C:\\fgh", "my contents"));

            var found = unsavedFiles.BinarySearch("c:\\abc");
            Assert.Equal(expected1, found);

            found = unsavedFiles.BinarySearch("c:\\ghi");
            Assert.Equal(expected2, found);
        }
    }
}