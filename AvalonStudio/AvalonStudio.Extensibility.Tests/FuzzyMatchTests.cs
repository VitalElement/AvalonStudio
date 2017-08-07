using Xunit;
using AvalonStudio.Extensibility.Utils;
using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Tests
{
    public class FuzzyMatchTests
    {
        void runtest(string query, string str, bool match, int expected_score, string expected_matches)
        {
            int score = 0;
            string format;
            /*bool retmatch = FuzzyMatch.fuzzy_match(pattern, str, out score, out format);

            Assert.Equal(match, retmatch);
            if (retmatch)
            {
                Assert.Equal(expected_score, score);
            }
            Assert.Equal(expected_matches, format);*/

            var res = FuzzyMatch.StringMatch(str, query, null);

            // order of what? matchQuality

           // 2 things ... i need to reverse the order of results, the lower the number the better.
           // Also the range of chars matched is now a list of indexes into the string...
        }

        void CheckFSC(string test, List<int> specials, int lastSegmentSpecialIndex)
        {
            var res = FuzzyMatch.FindSpecialCharacters(test);

            Assert.Equal(specials.Count, res.specials.Count);

            int i;
            for(i = 0; i < specials.Count; i++)
            {
                Assert.Equal(specials[i], res.specials[i]);
            }

            Assert.Equal(lastSegmentSpecialIndex, res.lastSegmentSpecialsIndex);
        }

        [Fact]
        private void find_special_chars_0()
        {
            string test = "src/document/DocumentCommandHandler.js";
            List<int> specials = new List<int> { 0, 3, 4, 12, 13, 21, 28, 35, 36};
            CheckFSC(test, specials, 4);
        }

        [Fact]
        private void find_special_chars_1()
        {
            string test = "foobar.js";
            List<int> specials = new List<int> { 0, 6, 7};
            CheckFSC(test, specials, 0);
        }

        [Fact]
        private void find_special_chars_2()
        {
            string test = "foo";
            List<int> specials = new List<int> { 0 };
            CheckFSC(test, specials, 0);
        }

        [Fact]
        private void generateMatchList_0()
        {
            string path = "src/document/DocumentCommandHandler.js";
            var specials = FuzzyMatch.FindSpecialCharacters(path);
            var result = FuzzyMatch.generateMatchList("foo", path, specials.specials, specials.lastSegmentSpecialsIndex);
            Assert.Equal(null, result);
        }

        [Fact]
        private void generateMatchList_1()
        {
            string path = "src/document/DocumentCommandHandler.js";
            var specials = FuzzyMatch.FindSpecialCharacters(path);
            path = path.ToLower();
            var result = FuzzyMatch.generateMatchList("d", path, specials.specials, specials.lastSegmentSpecialsIndex);
            Assert.Equal(13, result[0].index);
        }

        [Fact]
        private void test_bug_0()
        {
            runtest("ma", "Startup\\startup_stm32f410tx.c", false, 0, "");
        }

        [Fact]
        private void test_bug_1()
        {
            runtest("ma", "Lib\\GCC\\libarm_cortexM4l_math.a", true, 0, "");
        }

        [Fact]        
        private void test_test_match_25()
        {
            runtest("test", "test", true, 25, "{test}");
        }

        [Fact]
        private void test_tes_match()
        {
            runtest("tes", "test", true, 19, "{tes}t");
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

        [Fact]
        private void stringmatch_0()
        {
            Assert.Equal(null, FuzzyMatch.StringMatch("foo/bar/baz.js", "bingo"));
            var res = FuzzyMatch.StringMatch("foo/bar/baz.js", "fbb.js");
            res = FuzzyMatch.StringMatch("src/search/QuickOpen.js", "qo");

        }
    }
}
