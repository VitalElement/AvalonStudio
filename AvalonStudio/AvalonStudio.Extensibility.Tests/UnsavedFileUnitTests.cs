namespace AvalonStudio.Extensibility.Tests
{
    using AvalonStudio.Languages;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class UnsavedFileUnitTests
    {
        [Fact]
        public void UnsavedFiles_Are_Sorted_Alphabetically()
        {
            List<UnsavedFile> UnsavedFiles = new List<UnsavedFile>();

            UnsavedFiles.InsertSorted(new UnsavedFile("hij", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("bcd", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("abc", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("ghi", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("cde", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("fgh", "my contents"));

            List<UnsavedFile> expected = new List<UnsavedFile>();
            expected.Add(new UnsavedFile("abc", "my contents"));
            expected.Add(new UnsavedFile("bcd", "my contents"));
            expected.Add(new UnsavedFile("cde", "my contents"));
            expected.Add(new UnsavedFile("fgh", "my contents"));
            expected.Add(new UnsavedFile("ghi", "my contents"));
            expected.Add(new UnsavedFile("hij", "my contents"));

            Assert.True(expected.Select(u => u.FileName).SequenceEqual(UnsavedFiles.Select(u => u.FileName)));
        }

        [Fact]
        public void UnsavedFiles_Are_Sorted_Alphabetically_with_Complex_Paths()
        {
            List<UnsavedFile> UnsavedFiles = new List<UnsavedFile>();

            UnsavedFiles.InsertSorted(new UnsavedFile("G:\\development\\repos\\KHGD\\Modules\\CommonHal\\IFileSystem.h", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("G:\\development\\repos\\KHGD\\Gateway\\Gateway.h", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("G:\\development\\repos\\KHGD\\Gateway\\IPlatform.h", "my contents"));
            

            List<UnsavedFile> expected = new List<UnsavedFile>();
            expected.Add(new UnsavedFile("G:\\development\\repos\\KHGD\\Gateway\\Gateway.h", "my contents"));
            expected.Add(new UnsavedFile("G:\\development\\repos\\KHGD\\Gateway\\IPlatform.h", "my contents"));
            expected.Add(new UnsavedFile("G:\\development\\repos\\KHGD\\Modules\\CommonHal\\IFileSystem.h", "my contents"));

            Assert.True(expected.Select(u => u.FileName).SequenceEqual(UnsavedFiles.Select(u => u.FileName)));
        }


        [Fact]
        public void UnsavedFiles_Are_Sorted_Alphabetically_When_Paths_Are_Equivalent_But_Not_Equal()
        {
            List<UnsavedFile> UnsavedFiles = new List<UnsavedFile>();

            UnsavedFiles.InsertSorted(new UnsavedFile("C:\\hij/", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("c:\\bcd", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("c:\\abc/", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("c:/ghi/", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("c:\\cde\\", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("C:\\fgh", "my contents"));

            List<UnsavedFile> expected = new List<UnsavedFile>();
            expected.Add(new UnsavedFile("c:\\abc/", "my contents"));
            expected.Add(new UnsavedFile("c:\\bcd", "my contents"));
            expected.Add(new UnsavedFile("c:\\cde\\", "my contents"));
            expected.Add(new UnsavedFile("C:\\fgh", "my contents"));
            expected.Add(new UnsavedFile("c:/ghi/", "my contents"));
            expected.Add(new UnsavedFile("C:\\hij/", "my contents"));

            Assert.True(expected.Select(u => u.FileName).SequenceEqual(UnsavedFiles.Select(u => u.FileName)));
        }

        [Fact]
        public void UnsavedFiles_Can_Be_Binary_Searched()
        {
            List<UnsavedFile> UnsavedFiles = new List<UnsavedFile>();

            var expected1 = new UnsavedFile("c:\\abc/", "my contents");
            var expected2 = new UnsavedFile("c:/ghi/", "my contents");
            UnsavedFiles.InsertSorted(new UnsavedFile("C:\\hij/", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("c:\\bcd", "my contents"));
            UnsavedFiles.InsertSorted(expected1);
            UnsavedFiles.InsertSorted(expected2);
            UnsavedFiles.InsertSorted(new UnsavedFile("c:\\cde\\", "my contents"));
            UnsavedFiles.InsertSorted(new UnsavedFile("C:\\fgh", "my contents"));

            var found = UnsavedFiles.BinarySearch("c:\\abc");
            Assert.Equal(expected1, found);

            found = UnsavedFiles.BinarySearch("c:\\ghi");
            Assert.Equal(expected2, found);
        }
    }
}
