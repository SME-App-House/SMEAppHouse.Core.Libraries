using System;
using System.Text.RegularExpressions;

namespace SMEAppHouse.Core.CodeKits.Strings
{
    public static class ParsingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringToTest"></param>
        /// <returns></returns>
        public static bool IsNumeric(string stringToTest)
        {
            int result;
            return int.TryParse(stringToTest, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int ParseToInt32(string data)
        {
            //return Int32.Parse(data, System.Globalization.NumberStyles.Number);
            var m = Regex.Match(data, @"-?\d+");
            return Convert.ToInt32(m.Value);
        }

        /// <summary>
        /// Takes a substring between two anchor strings (or the end of the string if that anchor is null)
        /// Source: http://stackoverflow.com/questions/17252615/get-string-between-two-strings-in-a-string
        /// </summary>
        /// <param name="this">a string</param>
        /// <param name="from">an optional string to search after</param>
        /// <param name="until">an optional string to search before</param>
        /// <param name="comparison">an optional comparison for the search</param>
        /// <returns>a substring based on the search</returns>
        public static string Substring(this string @this, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from)
                ? @this.IndexOf(from, comparison) + fromLength
                : 0;

            if (startIndex < fromLength) { throw new ArgumentException("from: Failed to find an instance of the first anchor"); }

            var endIndex = !string.IsNullOrEmpty(until)
            ? @this.IndexOf(until, startIndex, comparison)
            : @this.Length;

            if (endIndex < 0) { throw new ArgumentException("until: Failed to find an instance of the last anchor"); }

            var subString = @this.Substring(startIndex, endIndex - startIndex);
            return subString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullAddress"></param>
        /// <param name="streetBlk"></param>
        /// <param name="locality"></param>
        /// <param name="region"></param>
        /// <param name="postalCode"></param>
        public static void SplitAddresses(string fullAddress, ref string streetBlk, ref string locality, ref string region, ref string postalCode)
        {
            var addressArray = fullAddress.Split(',');
            var num = 2;
            for(var i = addressArray.Length - 1; i >= 0; i--)
            {
                switch(num)
                {
                    case 0:
                        {
                            streetBlk = addressArray[i] + " " + streetBlk;
                            break;
                        }

                    case 1:
                        {
                            streetBlk = addressArray[i];
                            break;
                        }
                    case 2:
                        {
                            var pc_region = addressArray[i].Split(' ');
                            for(var j = 0; j < pc_region.Length; j++)
                            {
                                var temp = pc_region[j].ToUpper();
                                var postal = false;
                                try
                                {
                                    var a = Convert.ToInt32(temp);
                                    postal = true;
                                }
                                catch
                                {

                                }
                                if(pc_region[j] != temp && pc_region[j].Contains("[0-9]+") == false)
                                {
                                    locality += " " + pc_region[j];
                                }
                                else if(postal)
                                {
                                    postalCode = pc_region[j];
                                }
                                else
                                {
                                    region = pc_region[j];
                                }
                            }
                            break;
                        }

                }
                num--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullAddress"></param>
        /// <param name="streetBlk"></param>
        /// <param name="region"></param>
        /// <param name="postalCode"></param>
        public static void SplitAddresses(string fullAddress, ref string streetBlk, ref string region, ref string postalCode)
        {
            fullAddress = fullAddress.Replace(",", " ")
                            .Replace("Servicing: ", "")
                            .Replace("Services this area", "")
                            .Replace("Call Free", "")
                            .Trim();

            if(((((((fullAddress.Contains(" St ") | fullAddress.Contains(" Dve "))
                | fullAddress.Contains(" Rd "))
                | fullAddress.Contains(" Ave "))
                | fullAddress.Contains(" Road "))
                | fullAddress.Contains(" Rds "))
                | fullAddress.Contains(" Street "))
                | fullAddress.Contains(" Sts "))
            {
                var num2 = 0;
                var num3 = fullAddress.LastIndexOf(" St ", StringComparison.Ordinal);
                var index = fullAddress.LastIndexOf(" Dve ", StringComparison.Ordinal);

                if(index > num3)
                {
                    num3 = index;
                    num2 = 1;
                }
                index = fullAddress.LastIndexOf(" Rd ", StringComparison.Ordinal);
                if(index > num3)
                {
                    num3 = index;
                }
                index = fullAddress.LastIndexOf(" Sts ", StringComparison.Ordinal);
                if(index > num3)
                {
                    num3 = index;
                    num2 = 1;
                }
                index = fullAddress.LastIndexOf(" Rds ", StringComparison.Ordinal);
                if(index > num3)
                {
                    num3 = index;
                    num2 = 1;
                }
                index = fullAddress.LastIndexOf(" Ave ", StringComparison.Ordinal);
                if(index > num3)
                {
                    num3 = index;
                    num2 = 1;
                }
                index = fullAddress.LastIndexOf(" Road ", StringComparison.Ordinal);
                if(index > num3)
                {
                    num3 = index;
                    num2 = 2;
                }
                index = fullAddress.LastIndexOf(" Street ", StringComparison.Ordinal);
                if(index > num3)
                {
                    num3 = index;
                    num2 = 4;
                }

                streetBlk = fullAddress.Substring(0, (num3 + 3) + num2);

                var strArray = fullAddress.Split(new char[] { ' ' });

                if(IsNumeric(strArray[strArray.GetUpperBound(0)]))
                    postalCode = strArray[strArray.GetUpperBound(0)];

                if(postalCode != "")
                    region = fullAddress.Replace(streetBlk, "").Replace(postalCode, "").Trim();
                else
                    region = fullAddress.Replace(streetBlk, "").Trim();

                region = region.Replace("&amp;", "");
            }
        }



    }
}
