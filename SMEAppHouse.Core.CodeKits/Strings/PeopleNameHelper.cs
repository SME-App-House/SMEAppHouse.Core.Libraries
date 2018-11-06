using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMEAppHouse.Core.CodeKits.Strings
{
    public sealed class PeopleNameHelper
    {
        private static readonly List<string> Suffixes;

        static PeopleNameHelper()
        {
            // Initialize suffixes
            Suffixes = new List<string> {"jr", "sr", "esq", "ii", "iii", "iv", "v", "2nd", "3rd", "4th", "5th"};
        }

        /// <summary>
        /// Splits a name string into the first and last name
        /// </summary>
        /// <param name="name">Name to be split</param>
        /// <param name="firstName">Returns the first name</param>
        /// <param name="lastName">Returns the last name</param>
        public static void Split(string name, out string firstName, out string lastName)
        {
            // Parse last name
            var pos = FindWordStart(name, name.Length - 1);

            // If last token is suffix, include next token
            // as part of last name also
            if (IsSuffix(name.Substring(pos)))
                pos = FindWordStart(name, pos);

            // Set results
            firstName = name.Substring(0, pos).Trim();
            lastName = name.Substring(pos).Trim();
        }

        /// <summary>
        /// Finds the start of the word that comes before startIndex.
        /// </summary>
        /// <param name="s">String to examine</param>
        /// <param name="startIndex">Position to begin search</param>
        private static int FindWordStart(string s, int startIndex)
        {
            // Find end of previous word
            while (startIndex > 0 && char.IsWhiteSpace(s[startIndex]))
                startIndex--;
            // Find start of previous word
            while (startIndex > 0 && !char.IsWhiteSpace(s[startIndex]))
                startIndex--;
            // Return final position
            return startIndex;
        }

        /// <summary>
        /// Returns true if the given string appears to be a name suffix
        /// </summary>
        /// <param name="s">String to test</param>
        private static bool IsSuffix(string s)
        {
            // Strip any punctuation from string
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
            }
            return Suffixes.Contains(sb.ToString(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
