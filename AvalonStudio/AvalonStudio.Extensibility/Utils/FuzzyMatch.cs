using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonStudio.Extensibility.Utils
{

    public class FuzzyMatch
    {
        static public (List<int> specials, int lastSegmentSpecialsIndex) FindSpecialCharacters(string str)
        {
            const string special_chars = ".-_";
            int i;
            char c;
            int lastSegmentSpecialsIndex = 0;

            List<int> specials = new List<int>();
            specials.Add(0);    // The beginning of a string is always special!

            bool lastWasLower = false;

            for (i = 0; i < str.Length; i++)
            {
                c = str[i];

                if (c == '/' || c == '\\')
                {
                    specials.Add(i++);
                    specials.Add(i);
                    lastSegmentSpecialsIndex = specials.Count - 1;
                    lastWasLower = false;
                }
                else if (special_chars.IndexOf(c) >= 0)
                {
                    specials.Add(i++);
                    specials.Add(i);
                    lastWasLower = false;
                }
                else if (Char.ToUpper(c) == c)
                {
                    if (lastWasLower)
                    {
                        specials.Add(i);
                    }

                    lastWasLower = false;
                }
                else
                {
                    lastWasLower = true;
                }
            }

            return (specials, lastSegmentSpecialsIndex);
        }

        public enum States
        {
            SPECIALS_MATCH,
            ANY_MATCH,
        };

        public class Match
        {
            public Match(int index, bool isSpecial)
            {
                this.index = index;
                this.isSpecial = isSpecial;
            }

            public int index;
            public bool isSpecial;
        };

        public static List<Match> generateMatchList(string query, string str, List<int> specials, int startingSpecial)
        {
            var matches = new Stack<Match>();

            int specialsCounter = startingSpecial;

            int strCounter = specials[startingSpecial];
            int queryCounter;

            int[] deadBranches = Enumerable.Repeat(int.MaxValue, query.Length).ToArray();

            queryCounter = 0;

            States state = States.SPECIALS_MATCH;

            bool findMatchingSpecial()
            {
                int i;

                for (i = specialsCounter; i < specials.Count; i++)
                {
                    if (specials[i] >= deadBranches[queryCounter])
                    {
                        break;
                    }

                    if (specials[i] < strCounter)
                    {
                        specialsCounter = i;
                    }
                    else if (query[queryCounter] == str[specials[i]])
                    {
                        // we have a match -- track.
                        specialsCounter = i;
                        queryCounter++;
                        strCounter = specials[i];
                        matches.Push(new Match(strCounter++, true));
                        return true;
                    }
                }

                return false;
            }

            bool backtrack()
            {
                while (matches.Count > 0)
                {
                    var item = matches.Pop();

                    // pulled off a match, put a char back into the query.
                    queryCounter--;

                    if (item.isSpecial)
                    {
                        specialsCounter--;

                        if (item.index < deadBranches[queryCounter])
                        {
                            deadBranches[queryCounter] = item.index - 1;
                            state = States.ANY_MATCH;

                            item = matches.ElementAtOrDefault(matches.Count - 1);
                            if (item == null)
                            {
                                strCounter = specials[startingSpecial] + 1;
                                return true;
                            }

                            strCounter = item.index + 1;

                            return true;
                        }
                    }
                }
                return false;
            }

            while (true)
            {
                while (queryCounter < query.Length && strCounter < str.Length && strCounter <= deadBranches[queryCounter])
                {
                    if (state == States.SPECIALS_MATCH)
                    {
                        if (!findMatchingSpecial())
                        {
                            state = States.ANY_MATCH;
                        }
                    }

                    if (state == States.ANY_MATCH)
                    {
                        if (query[queryCounter] == str[strCounter])
                        {
                            queryCounter++;
                            matches.Push(new Match(strCounter++, false));
                            state = States.SPECIALS_MATCH;
                        }
                        else
                        {
                            strCounter++;
                        }
                    }
                }

                if (queryCounter >= query.Length || (queryCounter < query.Length && !backtrack()))
                {
                    break;
                }
            }

            if (queryCounter < query.Length || matches.Count == 0)
            {
                return null;
            }

            List<Match> result = matches.ToList();
            result.Reverse();

            return result;
        }

        static (string remainder, List<Match> matchList)? _lastSegmentSearch(string query, string str, List<int> specials, int startingSpecial, int lastSegmentStart)
        {
            List<Match> matchList = null;
            var remainder = "";
            var extraCharacters = specials[startingSpecial] + query.Length - str.Length;

            if (extraCharacters > 0)
            {
                remainder = query.Substring(0, extraCharacters);
                query = query.Substring(extraCharacters);
            }

            int queryCounter;
            for (queryCounter = 0; queryCounter < query.Length; queryCounter++)
            {
                matchList = generateMatchList(query.Substring(queryCounter), str, specials, startingSpecial);

                if(matchList != null || startingSpecial == 0)
                {
                    break;
                }
            }

            if(queryCounter == query.Length || matchList == null)
            {
                return null;
            }

            return (remainder + query.Substring(0, queryCounter), matchList);
        }


        static List<Match> _wholeStringSearch(string query, string str, List<int> specials, int lastSegmentSpecialsIndex)
        {
            query = query.ToLower();
            var compareStr = str.ToLower();

            var lastSegmentStart = specials[lastSegmentSpecialsIndex];

            List<Match> matchList = new List<Match>();

            var result = _lastSegmentSearch(query, compareStr, specials, lastSegmentSpecialsIndex, lastSegmentStart);

            if(result.HasValue)
            {
                matchList = result.Value.matchList;

                if(result.Value.remainder != "")
                {
                    var remainderMatchList = generateMatchList(result.Value.remainder, compareStr.Substring(0, lastSegmentStart), specials.GetRange(0, lastSegmentSpecialsIndex), 0);
                    if (remainderMatchList != null)
                    {
                        // add the new matched ranges to the beginning of the set of ranges we had
                        matchList.InsertRange(0, remainderMatchList);
                    }
                    else
                    {
                        // No match
                        return null;
                    }
                } 
            } else
            {
                // no match in last segment, start over searching whole string.
                matchList = generateMatchList(query, compareStr, specials, 0); // lastSegmentStart as 5th param??
            }

            return matchList;
        }

        public class Range
        {
            public Range(string text, bool matched, bool includesLastSegment)
            {
                this.text = text;
                this.matched = matched;
                this.includesLastSegment = includesLastSegment;
            }

            public string text;
            public bool matched;
            public bool includesLastSegment;     
        };

        const int SPECIAL_POINTS = 35;
        const int MATCH_POINTS = 10;
        const int LAST_SEGMENT_BOOST = 1;
        const int BEGINNING_OF_NAME_POINTS = 25;
        const double DEDUCTION_FOR_LENGTH = 0.2;
        const int CONSECUTIVE_MATCHES_POINTS = 10;
        const int NOT_STARTING_ON_SPECIAL_PENALTY = 25;

        static (List<Range> ranges, int score) _computeRangesAndScore(List<Match> matchList, string str, int lastSegmentStart)
        {
            List<Range> ranges = new List<Range>();

            int matchCounter;
            int lastMatchIndex = -1;
            int lastSegmentScore = 0;
            bool currentRangeStartedOnSpecial = false;

            int score = 0;
            //int scoreDebug;

            Range currentRange = null;

            void closeRangeGap(int c)
            {
                if(currentRange != null)
                {
                    currentRange.includesLastSegment = lastMatchIndex >= lastSegmentStart;
                    if(currentRange.matched && currentRange.includesLastSegment)
                    {
                        score += lastSegmentScore * LAST_SEGMENT_BOOST;
                    }

                    if(currentRange.matched && !currentRangeStartedOnSpecial)
                    {
                        score -= NOT_STARTING_ON_SPECIAL_PENALTY;
                    }

                    ranges.Add(currentRange);
                }

                if(lastMatchIndex + 1 < c)
                {
                    ranges.Add(new Range(str.Substring(lastMatchIndex + 1, c-(lastMatchIndex + 1)), false, c > lastSegmentStart));
                }

                currentRange = null;
                lastSegmentScore = 0;
            }

            int numConsecutive = 0;

            void addMatch(Match match)
            {
                int c = match.index;
                int newPoints = 0;

                newPoints += MATCH_POINTS;

                if(c == lastSegmentStart)
                {
                    newPoints += BEGINNING_OF_NAME_POINTS;
                }

                if(score > 0 && lastMatchIndex + 1 == c)
                {
                    if(currentRangeStartedOnSpecial)
                    {
                        numConsecutive++;
                    }

                    newPoints += CONSECUTIVE_MATCHES_POINTS * numConsecutive;
                } else
                {
                    numConsecutive = 1;
                }

                if(match.isSpecial)
                {
                    newPoints += SPECIAL_POINTS;
                }

                score += newPoints;

                if(c >= lastSegmentStart)
                {
                    lastSegmentScore += newPoints;
                }

                if((currentRange != null && !currentRange.matched) || c > lastMatchIndex + 1)
                {
                    closeRangeGap(c);
                }

                lastMatchIndex = c;

                if(currentRange == null)
                {
                    currentRange = new Range(str[c].ToString(), true, false);
                    currentRangeStartedOnSpecial = (match.isSpecial);
                } else
                {
                    currentRange.text += str[c];
                }
            }

            for(matchCounter = 0; matchCounter < matchList.Count; matchCounter++)
            {
                Match match = matchList[matchCounter];
                addMatch(match);
            }

            closeRangeGap(str.Length);

            int lengthPenalty = (int) (-1 * Math.Round(str.Length * DEDUCTION_FOR_LENGTH));

            score = score + lengthPenalty;

            return (ranges, score);
        }

        public class SearchResult
        {
            public SearchResult(string label)
            {
                this.label = label;
            }

            public string label;
            public List<Range> stringRanges;
            public int matchQuality;
        }

        public static SearchResult StringMatch(string str, string query, (List<int> specials, int lastSegmentSpecialsIndex)? special_data)
        {
            SearchResult result = null;

            if (query.Length == 0)
            {
                // Return a single result..
            }

            if (special_data == null)
            {
                special_data = FindSpecialCharacters(str);
            }

            var lastSegmentStart = special_data.Value.specials[special_data.Value.lastSegmentSpecialsIndex];
            var matchList = _wholeStringSearch(query, str, special_data.Value.specials, special_data.Value.lastSegmentSpecialsIndex);

            // If a matchList is resulted then return a fully formed result.
            if(matchList != null)
            {
                var compareData = _computeRangesAndScore(matchList, str, lastSegmentStart);
                result = new SearchResult(str);
                result.stringRanges = compareData.ranges;
                result.matchQuality = -1 * (compareData.score);
            }

            return result;
        }

        public static SearchResult StringMatch(string str, string query)
        {
            return StringMatch(str, query, null);
        }
    }
}
