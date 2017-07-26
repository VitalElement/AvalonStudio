using Xunit;
using AvalonStudio.Extensibility.Utils;

namespace AvalonStudio.Extensibility.Tests
{
    public class FuzzyMatchTests
    {
        void runtest(string pattern, string str, bool match, int expected_score, string expected_matches)
        {
            int score = 0;
            string format;
            bool retmatch = FuzzyMatch.fuzzy_match(pattern, str, out score, out format);

            Assert.Equal(match, retmatch);
            if (retmatch)
            {
                Assert.Equal(expected_score, score);
            }
            Assert.Equal(expected_matches, format);
        }

        [Fact]        
        private void test_test_match_25()
        {
            runtest("test", "test", true, 25, "{t}{e}{s}{t}");
        }

        [Fact]
        private void test_tes_match()
        {
            runtest("tes", "test", true, 19, "{t}{e}{s}t");
        }

        [Fact]
        private void test_sets_nomatch()
        {
            runtest("sets", "test", false, 0, "te{s}t");
        }

        [Fact]
        private void test_tt_match()
        {
            runtest("tt", "test", true, 8, "{t}es{t}");
        }
    }
}
