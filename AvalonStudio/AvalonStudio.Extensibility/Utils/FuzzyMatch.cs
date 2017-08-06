using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Extensibility.Utils
{
    public class FuzzyMatch
    {
        static public (List<int>, int) FindSpecialCharacters(string str)
        {
            const string special_chars = ".-_";
            int i;
            char c;
            int lastSegmentSpecialsIndex = 0;

            List<int> specials = new List<int>();

            bool lastWasLower = false;

            for(i=0; i < str.Length; i++)
            {
                c = str[i];

                if (c == '/' || c == '\\')
                {
                    specials.Add(i++);
                    specials.Add(i);
                    lastSegmentSpecialsIndex = specials.Count - 1;
                    lastWasLower = false;
                } else if (special_chars.IndexOf(c) >= 0)
                {
                    specials.Add(i);
                    specials.Add(i++);
                    lastWasLower = false;
                } else if (Char.ToUpper(c) == c)
                {
                    if(lastWasLower)
                    {
                        specials.Add(i);
                    }

                    lastWasLower = false;
                } else
                {
                    lastWasLower = true;
                }
            }

            return (specials, lastSegmentSpecialsIndex);
        }

        static bool StringMatch(string str, string query = "", object special_data=null)
        {
            bool result = false;

            if (query.Length == 0)
            {
                // Return a single result..
            }

            if(special_data != null)
            {
                special_data = FindSpecialCharacters(str);
            }



            return result;
        }

        static public bool fuzzy_match(string pattern, string str, out int pa_score, out string pa_formatStr)
        {
            const int adjacency_bonus = 5;
            const int separator_bonus = 10;
            const int camel_bonus = 10;
            const int leading_letter_penalty = -3;
            const int max_leading_letter_penalty = -9;
            const int unamtched_letter_penalty = -1;

            // loop vars
            int score = 0;
            int patternIdx = 0;
            int patternLength = pattern.Length;
            int strIdx = 0;
            int strLength = str.Length;
            bool prevMatched = false;
            bool prevLower = false;
            bool prevSeparator = true;   // True so first letter match gets separator bonus.

            char? bestLetter = null;
            char? bestLower = null;
            int? bestLetterIdx = null;
            int bestLetterScore = 0;

            string formattedStr = "";

            List<int> matchedIndices = new List<int>();

            while (strIdx != strLength)
            {
                char? patternChar = null;
                if (patternIdx != patternLength) {
                    patternChar = pattern[patternIdx];
                }

                char strChar = str[strIdx];

                char? patternLower = null;
                if (patternChar != null)
                {
                    patternLower = Char.ToLower(patternChar.Value);
                }

                char strLower = Char.ToLower(strChar);
                char strUpper = Char.ToUpper(strChar);

                bool nextMatch = (patternChar == strLower || patternLower == strLower);
                bool rematch = (bestLetter == strLower || bestLower == strLower);

                bool advanced = nextMatch && bestLetter.HasValue;
                bool patternRepeat = (bestLetter == strLower && bestLower == strLower);

                if(advanced || patternRepeat)
                {
                    score += bestLetterScore;
                    matchedIndices.Add(bestLetterIdx.Value);
                    bestLetter = null;
                    bestLower = null;
                    bestLetterIdx = null;
                    bestLetterScore = 0;
                }

                if(nextMatch || rematch)
                {
                    int newScore = 0;
                    if(patternIdx == 0)
                    {
                        int penalty = Math.Max(strIdx * max_leading_letter_penalty, max_leading_letter_penalty);
                        score += penalty;
                    }

                    if(prevMatched)
                    {
                        newScore += adjacency_bonus;
                    }

                    if(prevSeparator)
                    {
                        newScore += separator_bonus;
                    }

                    if(prevLower && strChar == strUpper && strLower != strUpper)
                    {
                        newScore += camel_bonus;
                    }

                    if(nextMatch)
                    {
                        patternIdx += 1;
                    }

                    if(newScore >= bestLetterScore)
                    {
                        if(bestLetter.HasValue)
                        {
                            score += unamtched_letter_penalty;
                        }

                        bestLetter = strChar;
                        bestLower = Char.ToLower(bestLetter.Value);
                        bestLetterIdx = strIdx;
                        bestLetterScore = newScore;
                    }

                    prevMatched = true;

                } else
                {
                    //formattedStr += strChar;
                    score += unamtched_letter_penalty;
                    prevMatched = false;
                }

                prevLower = (strChar == strLower || strLower != strUpper);
                prevSeparator = strChar == '_' || strChar == ' ';

                strIdx += 1;
            }

            if(bestLetter.HasValue)
            {
                score += bestLetterScore;
                matchedIndices.Add(bestLetterIdx.Value);
            }

            int lastIdx = 0;
            for (int i = 0; i < matchedIndices.Count; i++)
            {
                var idx = matchedIndices[i];
                formattedStr += str.Substring(lastIdx, idx - lastIdx) + "{" + str[idx] + "}";
                lastIdx = idx + 1;
            }

            formattedStr += str.Substring(lastIdx, str.Length - lastIdx);

            bool matched = patternIdx == patternLength;

            pa_score = score;
            pa_formatStr = formattedStr;

            return matched;
        }
    }
}
