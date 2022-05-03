using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace IO
{
    public static class RichTextHandler
    {
        public static readonly Dictionary<string, string> Tags = new Dictionary<string, string>
        {
            { "<hl>", "<color=#03dbfc><b>" },
            { "</hl>", "</b></color>" }
        };

        /// <summary>
        /// Parse a string with one or multiple tags and return the result.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Parse(string input)
        {
            StringBuilder result = new StringBuilder(input.Length * 2);
            int i = 0;
            while (i < input.Length)
            {
                char next = input[i];

                if (next == '<')
                {
                    result.Append(ParseUpcomingTag(input, i, out i));
                    continue;
                }

                result.Append(next);
                i++;
            }

            return result.ToString();
        }

        /// <summary>
        /// Parses an upcoming tag character by character and returns the parsed tag.
        /// </summary>
        /// <returns></returns>
        public static string ParseUpcomingTag(string input, int startIndex, out int endIndex)
        {
            if (input[startIndex] != '<')
            {
                throw new System.ArgumentException($"Input string did not contain '<' at given index.\ninput: {input}\nstartIndex: {startIndex}\nActual Character: {input[startIndex]}");
            }
            StringBuilder inputTag = new StringBuilder();
            int currentIndex = startIndex;

            while(currentIndex < input.Length)
            {
                char next = input[currentIndex];

                inputTag.Append(next);
                currentIndex++;

                if (next == '>')
                {
                    endIndex = currentIndex;
                    return ParseSingle(inputTag.ToString());
                }
            }

            throw new System.ArgumentException($"Input string did not contain closing '>' character.\ninput: {input}\nstartIndex: {startIndex}");
        }

        /// <summary>
        /// Parse a string containing only a tag and return the result.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ParseSingle(string input)
        {
            if (Tags.TryGetValue(input.ToLower(), out string tag))
            {
                return tag;
            }
            else
            {
                return input;
            }
        }
    }

}