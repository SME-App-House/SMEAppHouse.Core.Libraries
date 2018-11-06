using System;
using System.Collections.Generic;
using System.Text;

namespace SMEAppHouse.Core.CodeKits.Encryptions
{
    public static class Base64EncodeDecodeUtil
    {
        private const char Base64Padding = '=';

        private static readonly HashSet<char> Base64Characters = new HashSet<char>()
            { 
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 
                'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
                'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
            };

        public static void CheckBase64String(string param, string paramName)
        {
            if (CheckBase64StringSafe(param) == false)
            {
                throw (new ArgumentException($"Parameter '{paramName}' is not a valid Base64 string."));
            }
        }

        public static bool CheckBase64StringSafe(string param)
        {
            if (param == null)
            {
                // null string is not Base64 something
                return false;
            }

            // replace optional CR and LF characters
            param = param.Replace("\r", string.Empty).Replace("\n", string.Empty);

            if (param.Length == 0 ||
                (param.Length % 4) != 0)
            {
                // Base64 string should not be empty
                // Base64 string length should be multiple of 4
                return false;
            }

            // replace pad chacters
            var lengthNoPadding = param.Length;
            int lengthPadding;

            param = param.TrimEnd(Base64Padding);
            lengthPadding = param.Length;

            if ((lengthNoPadding - lengthPadding) > 2)
            {
                // there should be no more than 2 pad characters
                return false;
            }

            foreach (var c in param)
            {
                if (Base64Characters.Contains(c) == false)
                {
                    // string contains non-Base64 character
                    return false;
                }
            }

            // nothing invalid found
            return true;
        }

        public static string base64Decode(string data)
        {
            try
            {
                var encoder = new UTF8Encoding();
                var utf8Decode = encoder.GetDecoder();

                var todecode_byte = Convert.FromBase64String(data);
                var charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                var decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                var result = new string(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        public static string base64Encode(string data)
        {
            try
            {
                var encData_byte = new byte[data.Length];
                encData_byte = Encoding.UTF8.GetBytes(data);
                var encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

    }
}
