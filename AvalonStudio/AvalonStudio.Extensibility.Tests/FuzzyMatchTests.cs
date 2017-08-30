using Xunit;
using AvalonStudio.Extensibility.Utils;
using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Tests
{
    public class FuzzyMatchTests
    {
        private void RunTest(string query, string str, bool match, int expected_score, string expected_matches)
        {
            var res = FuzzyMatch.StringMatch(str, query, null);
            if (res != null)
            {
                Assert.Equal(expected_score, res.matchQuality);
            }

            if(res == null)
            {
                Assert.Equal(0, expected_score);
                return;
            }

            List<int> matches = new List<int>();

            bool inMatch = false;
            for (int actual_index = 0, i = 0; i < expected_matches.Length; i++)
            {
                if ("{}".Contains(expected_matches[i].ToString()))
                {
                    switch (expected_matches[i])
                    {
                        case '{':
                            inMatch = true;
                            continue;

                        case '}':
                            inMatch = false;
                            continue;

                        default:
                            break;
                    }
                }

                if (inMatch)
                {
                    matches.Add(actual_index);
                }

                actual_index++;
            }

            int section_index = 0;
            foreach (var range in res.stringRanges)
            {

                for (int i = 0; i < range.text.Length; i++)
                {
                    if (range.matched)
                    {
                        Assert.True(matches.Contains(section_index + i));
                    }
                    else
                    {
                        Assert.False(matches.Contains(section_index + i));
                    }
                }


                section_index += range.text.Length;
            }

        }

        void CheckFSC(string test, List<int> specials, int lastSegmentSpecialIndex)
        {
            var res = FuzzyMatch.FindSpecialCharacters(test);

            Assert.Equal(specials.Count, res.specials.Count);

            int i;
            for (i = 0; i < specials.Count; i++)
            {
                Assert.Equal(specials[i], res.specials[i]);
            }

            Assert.Equal(lastSegmentSpecialIndex, res.lastSegmentSpecialsIndex);
        }

        [Fact]
        private void find_special_chars_0()
        {
            string test = "src/document/DocumentCommandHandler.js";
            List<int> specials = new List<int> { 0, 3, 4, 12, 13, 21, 28, 35, 36 };
            CheckFSC(test, specials, 4);
        }

        [Fact]
        private void find_special_chars_1()
        {
            string test = "foobar.js";
            List<int> specials = new List<int> { 0, 6, 7 };
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
            RunTest("ma", "Startup\\startup_stm32f410tx.c", false, 0, "");
        }

        [Fact]
        private void test_bug_1()
        {
            RunTest("ma", "Lib\\GCC\\libarm_cortexM4l_math.a", true, -129, "Lib\\GCC\\libarm_cortex{M}4l_math.{a}");
        }

        [Fact]
        private void test_test_match_25()
        {
            RunTest("test", "test", true, -379, "{test}");
        }

        [Fact]
        private void test_tes_match()
        {
            RunTest("tes", "test", true, -279, "{tes}t");
        }

        [Fact]
        private void test_sets_nomatch()
        {
            RunTest("sets", "test", false, 0, "");
        }

        [Fact]
        private void test_tt_match()
        {
            RunTest("tt", "test", true, -134, "{t}es{t}");
        }

        [Fact]
        private void stringmatch_0()
        {
            Assert.Equal(null, FuzzyMatch.StringMatch("foo/bar/baz.js", "bingo"));
            var res = FuzzyMatch.StringMatch("foo/bar/baz.js", "fbb.js");
            res = FuzzyMatch.StringMatch("src/search/QuickOpen.js", "qo");

        }

        static bool goodRelativeOrdering(string query, List<string> testStrings)
        {
            var lastScore = int.MinValue;
            var goodOrdering = true;

            foreach (var str in testStrings)
            {
                var result = FuzzyMatch.StringMatch(str, query);

                if (result.matchQuality < lastScore)
                {
                    goodOrdering = false;
                }

                lastScore = result.matchQuality;
            }

            return goodOrdering;
        }

        [Fact]
        private void good_ordering_0()
        {
            Assert.Equal(true, goodRelativeOrdering("quick", new List<string>
            {
                "src/search/QuickOpen.js",
                "test/spec/QuickOpen-test.js",
                "samples/root/Getting Started/screenshots/brackets-quick-edit.png",
                "src/extensions/default/QuickOpenCSS/main.js",
            }));

            Assert.Equal(true, goodRelativeOrdering("spec/live", new List<string>
            {
                    "test/spec/LiveDevelopment-test.js",
                    "test/spec/LiveDevelopment-chrome-user-data/Default/VisitedLinks"
            }));


            Assert.Equal(true, goodRelativeOrdering("samples/index", new List<string>
            {
                    "samples/de/Erste Schritte/index.html",
                    "src/thirdparty/CodeMirror2/mode/ntriples/index.html"
             }));

            Assert.Equal(true, goodRelativeOrdering("Commands", new List<string>
            {        "src/command/Commands.js",
                    "src/command/CommandManager.js"
            }));



            Assert.Equal(true, goodRelativeOrdering("extensions", new List<string>
            {
                    "src/utils/ExtensionLoader.js",
                    "src/extensions/default/RecentProjects/styles.css"
            }));


            Assert.Equal(true, goodRelativeOrdering("EUtil", new List<string>
            {
                    "src/editor/EditorUtils.js",
                    "src/utils/ExtensionUtils.js",
                    "src/file/FileUtils.js"
            }));

            Assert.Equal(true, goodRelativeOrdering("ECH", new List<string>
            {
                "EditorCommandHandlers",
                    "EditorCommandHandlers-test",
                    "SpecHelper"
            }));

            Assert.Equal(true, goodRelativeOrdering("DMan", new List<string>
            {
                "DocumentManager",
                    "CommandManager"
            }));

            Assert.Equal(true, goodRelativeOrdering("sru", new List<string>
            {
                    "test/spec/SpecRunnerUtils.js",
                    "test/SpecRunner.html"
            }));

            Assert.Equal(true, goodRelativeOrdering("jsutil", new List<string>
            {
                    "src/language/JSUtil.js",
                    "src/language/JSLintUtils.js"
            }));

            Assert.Equal(true, goodRelativeOrdering("jsu", new List<string>
            {
                    "src/language/JSLintUtils.js",
                    "src/language/JSUtil.js"
            }));
        }


    }
}
