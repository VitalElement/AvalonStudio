namespace AvalonStudio.Extensibility.Tests
{
    using Platforms;
    using Xunit;

    public class FilePathComparisonTests
    {
        [Fact]
        private void Comparison_Matches_When_Trailing_Seperator_On_One_Path()
        {
            string path1 = "c:\\avalonstudio\\test\\";
            string path2 = "c:\\avalonstudio\\test";

            Assert.Equal(0, path1.CompareFilePath(path2));

            Assert.Equal(0, path2.CompareFilePath(path1));
        }

        [Fact]
        private void Comparison_Matches_When_DriveSpecifier_has_different_capitalization()
        {
            string path1 = "C:\\avalonstudio\\test\\";
            string path2 = "c:\\avalonstudio\\test";

            if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
            {
                Assert.Equal(0, path1.CompareFilePath(path2));

                Assert.Equal(0, path2.CompareFilePath(path1));
            }
        }

        [Fact]
        private void Comparison_Matches_When_Path_Contains_UpperLevel_Symbols()
        {
            string path1 = "c:\\avalonstudio\\test\\..\\test\\";
            string path2 = "c:\\avalonstudio\\test";

            Assert.Equal(0, path1.CompareFilePath(path2));

            Assert.Equal(0, path2.CompareFilePath(path1));
        }

        [Fact]
        private void Comparison_Matches_When_unix_paths()
        {
            string path1 = "/c/avalonstudio/test/";
            string path2 = "/c/avalonstudio/test";

            Assert.Equal(0, path1.CompareFilePath(path2));

            Assert.Equal(0, path2.CompareFilePath(path1));
        }

        [Fact]
        private void Comparison_Against_Null_Works()
        {
            string path1 = null;
            string path2 = "/c/avalonstudio/test";

            Assert.NotEqual(0, path2.CompareFilePath(path1));

            Assert.NotEqual(0, Platform.CompareFilePath(null, path2));
        }
    }
}