using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SMEAppHouse.Core.CodeKits.Extensions
{
    public static class StringExt
    {
        public static string SplitQuotedLine(this string value, char separator, bool quotes)
        {
            // Use the "quotes" bool if you need to keep/strip the quotes or something...
            var s = new StringBuilder();
            var regex = new Regex("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");
            foreach (Match m in regex.Matches(value))
            {
                s.Append(m.Value);
            }
            return s.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(string param, int length)
        {
            var result = string.Empty;
            if (param.Length > 0) result = param.Substring(0, length);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(string param, int length)
        {
            var result = string.Empty;
            if (param.Length > 0) result = param.Substring((param.Length - length), length);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Mid(string param, int startIndex, int length)
        {
            var result = string.Empty;
            if (param.Length > 0) result = param.Substring(startIndex, length);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string Mid(string param, int startIndex)
        {
            var result = string.Empty;
            if (param.Length > 0) result = param.Substring(startIndex);
            return result;
        }

        /// <summary>
        /// Tries to create a phrase string from CamelCase text.
        /// Will place spaces before capitalized letters.
        /// 
        /// Note that this method may not work for round tripping 
        /// ToCamelCase calls, since ToCamelCase strips more characters
        /// than just spaces.
        /// </summary>
        /// <param name="camelCase"></param>
        /// <returns></returns>
        public static string FromCamelCase(string camelCase)
        {
            if (camelCase == null)
                throw new ArgumentException("Null is not allowed for StringUtils.FromCamelCase");

            var sb = new StringBuilder(camelCase.Length + 10);
            var first = true;
            var lastChar = '\0';

            foreach (var ch in camelCase)
            {
                if (!first &&
                    (char.IsUpper(ch) ||
                     char.IsDigit(ch) && !char.IsDigit(lastChar)))
                    sb.Append(' ');

                sb.Append(ch);
                first = false;
                lastChar = ch;
            }

            return sb.ToString();
            ;
        }

        /// <summary>
        /// Takes a phrase and turns it into CamelCase text.
        /// White Space, punctuation and separators are stripped
        /// </summary>
        /// <param name="?"></param>
        public static string ToCamelCase(string phrase)
        {
            if (phrase == null)
                return string.Empty;

            var sb = new StringBuilder(phrase.Length);

            // First letter is always upper case
            var nextUpper = true;

            foreach (var ch in phrase)
            {
                if (char.IsWhiteSpace(ch) || char.IsPunctuation(ch) || char.IsSeparator(ch))
                {
                    nextUpper = true;
                    continue;
                }

                if (nextUpper)
                    sb.Append(char.ToUpper(ch));
                else
                    sb.Append(char.ToLower(ch));

                nextUpper = false;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sData"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string sData)
        {
            Exception x = null;
            return StringToByteArray(sData, ref x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sData"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string sData, ref Exception x)
        {
            try
            {
                var text = (sData + " ").Trim();
                var arrText = text.Split(' ');
                var data = new byte[arrText.Length];
                for (var i = 0; i < arrText.Length; i++)
                {
                    if (arrText[i] != "")
                    {
                        var value = int.Parse(arrText[i], NumberStyles.Number);
                        data[i] = (byte)Convert.ToByte(value);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                x = ex;
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FindDigitInString(string str)
        {
            var strResult = string.Empty;
            for (var i = 0; i < str.Length; i++)
                if (Convert.ToInt32(str[i]) >= 0x32 && Convert.ToInt32(str[i]) <= 0x39)
                    strResult += str[i];
            return strResult;
        }

        /// <summary>
        /// Ex: s = RemoveBetween(s, '(', ')');
        /// </summary>
        /// <param name="s"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string RemoveBetween(string s, char begin, char end)
        {
            var regex = new Regex($"\\{begin}.*?\\{end}");
            //return regex.Replace(s, string.Empty);
            return new Regex(" +").Replace(regex.Replace(s, string.Empty), " ");
        }

        /// <summary>
        /// Split people's name whose pattern is First, Last Middle
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="middleName"></param>
        public static void SplitPeopleName1(string targetName, ref string firstName, ref string lastName,
            ref string middleName)
        {
            var match = Regex.Match(targetName, @"^(?<first>\w+), (?<last>\w+)(?: (?<middle>\w+))?$");
            if (!match.Success) return;

            firstName = match.Groups["first"].Value;
            lastName = match.Groups["last"].Value;
            if (match.Groups["middle"].Success)
            {
                middleName = match.Groups["middle"].Value;
            }
        }

        /// <summary>
        /// Split people's name whose pattern is First, Last Middle
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="middleName"></param>
        public static void SplitPeopleName2(string targetName, ref string firstName, ref string lastName,
            ref string middleName)
        {
            var match = Regex.Match(targetName, @"^(?<first>\w+), (?<last>\w+)(?: (?<middle>\w+))?$");
            if (!match.Success) return;

            firstName = match.Groups["first"].Value;
            lastName = match.Groups["last"].Value;
            if (match.Groups["middle"].Success)
            {
                middleName = match.Groups["middle"].Value;
            }
        }

        public static string TrimOrReplaceSpaces(this string target, int numSpace = 1)
        {
            target = target.Trim();
            var regex = new Regex(@"[ ]{2,}", RegexOptions.None);
            target = regex.Replace(target, new string(' ', numSpace));
            return target;
        }

        public static bool CompareContent(this string target, string compareWith)
        {
            // ReSharper disable InconsistentNaming
            var _target = target.TrimOrReplaceSpaces();
            var _compareWith = compareWith.TrimOrReplaceSpaces();
            // ReSharper restore InconsistentNaming
            return _target.ToLower().Equals(_compareWith.ToLower());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + " ..";
        }


        /// <summary>
        /// How can I find a string after a specific string/character using regex
        /// http://stackoverflow.com/questions/454414/how-can-i-find-a-string-after-a-specific-string-character-using-regex
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<string, string> FindKeyValues(IEnumerable<string> keywords, string source)
        {
            var found = new Dictionary<string, string>();

            var keys = string.Join("|", keywords.ToArray());
            var matches = Regex.Matches(source, @"(?<key>" + keys + "):",
                                  RegexOptions.IgnoreCase);

            foreach (Match m in matches)
            {
                var key = m.Groups["key"].ToString();
                if (found.ContainsKey(key)) continue;
                var start = m.Index + m.Length;
                var nx = m.NextMatch();
                var end = (nx.Success ? nx.Index : source.Length);

                var members = source.Substring(start, end - start)
                    .Replace("\n", "").Trim()
                    .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            }

            return found;

            /*
            foreach (var n in found)
            {
                Console.WriteLine("Key={0}, Value={1}", n.Key, n.Value);
            }*/
        }

        /// <summary>
        /// By Joebet
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CreateCommaDelimitedString(string[] value)
        {
            return value == null ? "" : string.Join(",", value);
        }

    }
}
