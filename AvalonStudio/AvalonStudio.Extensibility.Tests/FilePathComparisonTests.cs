namespace AvalonStudio.Extensibility.Tests
{
    using Platforms;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class FilePathComparisonTests
    {
        [Fact]
        void Comparison_Matches_When_Trailing_Seperator_On_One_Path ()
        {
            string path1 = "c:\\avalonstudio\\test\\";
            string path2 = "c:\\avalonstudio\\test";

            Assert.Equal(0, path1.CompareFilePath(path2));

            Assert.Equal(0, path2.CompareFilePath(path1));
        }

        [Fact]
        void Comparison_Matches_When_DriveSpecifier_has_different_capitalization()
        {
            string path1 = "C:\\avalonstudio\\test\\";
            string path2 = "c:\\avalonstudio\\test";

            Assert.Equal(0, path1.CompareFilePath(path2));

            Assert.Equal(0, path2.CompareFilePath(path1));
        }

        [Fact]
        void Comparison_Matches_When_unix_paths()
        {
            string path1 = "/c/avalonstudio/test/";
            string path2 = "/c/avalonstudio/test";

            Assert.Equal(0, path1.CompareFilePath(path2));

            Assert.Equal(0, path2.CompareFilePath(path1));
        }
    }
}
