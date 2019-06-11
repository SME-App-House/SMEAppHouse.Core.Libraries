using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SMEAppHouse.Core.CodeKits.Helpers
{
    /// <summary>
    /// 
    /// sources:
    ///     http://www.getcodesamples.com/src/3098626E/B3D29EEA
    /// 
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private const string INDENT_STRING = "    ";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatJson(string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ToList().ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ToList().ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        var escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ToList().ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static string FixJsonString(string jsonStr)
        {
            jsonStr = jsonStr.Trim();
            jsonStr = jsonStr.Trim(new char[] { '\r', '\n' });
            jsonStr = jsonStr.Replace(" ", "");
            jsonStr = jsonStr.Replace("\r\n", "");
            jsonStr = jsonStr.Replace("\r", "");
            jsonStr = jsonStr.Replace("\n", "");

            jsonStr = jsonStr.Replace(",!", "");

            var _nwjsonStr = String.Empty;
            for (var i = 0; i < jsonStr.Length; i++)
            {
                //jsonStr
            }

            return jsonStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string NormalizeJson(string json)
        {
            try
            {
                //const string strFindRegex = @"(?[,{]\s*)(?\w+):";
                //const string strReplaceRegex = @"${prefix}""${field}"":";

                const string strFindRegex = @"(?{arrow left}prefix{arrow right}[,{]\s*)(?{arrow left}field{arrow right}\w+):";
                const string strReplaceRegex = @"${prefix}""${field}"":";

                var strWellFormedJson = Regex.Replace(json, strFindRegex, strReplaceRegex);
                // functions are valid in javascript, but not valid for a JsonReader
                const string strNoFunctionRegex = @"function..\s+{.*}";
                strWellFormedJson = Regex.Replace(strWellFormedJson, strNoFunctionRegex, "false");
                // Json should end with a close-curly-bracket
                if (!strWellFormedJson.EndsWith("}"))
                {
                    var intLastCurlyBracket = strWellFormedJson.LastIndexOf("}");
                    strWellFormedJson = strWellFormedJson.Substring(0, intLastCurlyBracket + 1);
                }
                return strWellFormedJson;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string ReadJsonFileAsString(string jsonFile)
        {
            var jsonData = String.Empty;
            try
            {
                var myFile = new StreamReader(jsonFile);

                jsonData = myFile.ReadToEnd();

                myFile.Close();
            }
            catch (Exception)
            {
                //log.Warn(e.Message);
            }

            return jsonData;
        }

        public static JObject ReadJsonFileAsJObject(string jsonFile)
        {
            JObject _jObj = null;
            using (var file = File.OpenText(jsonFile))
            {
                using (var reader = new JsonTextReader(file))
                {
                    _jObj = (JObject)JToken.ReadFrom(reader);
                }
            }
            return _jObj;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/5156664/how-to-flatten-an-expandoobject-returned-via-jsonresult-in-asp-net-mvc
        /// </summary>
        /// <param name="expando"></param>
        /// <returns></returns>
        public static string Flatten(this ExpandoObject expando)
        {
            var sb = new StringBuilder();
            var contents = new List<string>();
            var d = expando as IDictionary<string, object>;
            sb.Append("{");

            foreach (var kvp in d)
            {
                contents.Add($"{kvp.Key}: {JsonConvert.SerializeObject(kvp.Value)}");
            }
            sb.Append(String.Join(",", contents.ToArray()));

            sb.Append("}");

            return sb.ToString();
        }

        public static dynamic DeserializeJson(string jsonData)
        {
            return JsonConvert.DeserializeObject(jsonData, typeof(object));
        }

        /// <summary>
        /// http://stackoverflow.com/questions/2246694/how-to-convert-json-object-to-custom-c-sharp-object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            var obj = Activator.CreateInstance<T>();
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            var serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }
        public static string Serialize<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            var retVal = Encoding.UTF8.GetString(ms.ToArray());
            return retVal;
        }

        public static List<JToken> GetAllTokens(this JToken token)
        {
            IEnumerable<JToken> tokens = new List<JToken>();

            if (!token.HasValues)
                return new List<JToken>() { token.Parent };

            var children = token.Children();
            foreach (var jToken in children)
            {
                var cTokens = jToken.GetAllTokens();
                if (cTokens != null && cTokens.Any())
                    tokens = tokens.Concat(cTokens);
            }
            return tokens.ToList();
        }

        public static List<JToken> GetAllObjects(this JToken token)
        {
            IEnumerable<JToken> tokens = new List<JToken>();

            if (!token.HasValues)
                return new List<JToken>() { token.Parent };

            var children = token.Children();
            foreach (var jToken in children)
            {
                var cTokens = jToken.GetAllTokens();
                if (cTokens != null && cTokens.Any())
                    tokens = tokens.Concat(cTokens);
            }
            return tokens.ToList();
        }

        public static T GetValue<T>(this JToken jToken, string key, T defaultValue = default(T))
        {
            dynamic ret = jToken[key];
            if (ret == null) return defaultValue;
            if (ret is JObject) return JsonConvert.DeserializeObject<T>(ret.ToString());
            return (T)ret;
        }


        public static T GetJToken<T>(JObject obj, string field) where T : JToken
        {
            JToken value;
            if (obj.TryGetValue(field, out value)) return GetJToken<T>(value);
            else return null;
        }

        private static T GetJToken<T>(JToken token) where T : JToken
        {
            if (token == null) return null;
            if (token.Type == JTokenType.Null) return null;
            if (token.Type == JTokenType.Undefined) return null;
            return (T)token;
        }

        public static string ReadString(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return (string)value.Value;
        }


        public static bool ReadBoolean(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToBoolean(value.Value);

        }

        public static bool? ReadNullableBoolean(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToBoolean(value.Value);
        }

        public static int ReadInteger(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToInt32((long)value.Value);

        }

        public static int? ReadNullableInteger(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToInt32((long)value.Value);

        }



        public static long ReadLong(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToInt64(value.Value);

        }

        public static long? ReadNullableLong(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToInt64(value.Value);
        }


        public static double ReadFloat(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToDouble(value.Value);

        }

        public static double? ReadNullableFloat(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToDouble(value.Value);

        }




        public static DateTime ReadDate(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) throw new Newtonsoft.Json.JsonSerializationException();
            return Convert.ToDateTime(value.Value);

        }

        public static DateTime? ReadNullableDate(JToken token)
        {
            var value = GetJToken<JValue>(token);
            if (value == null) return null;
            return Convert.ToDateTime(value.Value);

        }

        public static object ReadObject(JToken token)
        {
            var value = GetJToken<JToken>(token);
            if (value == null) return null;
            if (value.Type == JTokenType.Object) return value;
            if (value.Type == JTokenType.Array) return ReadArray<object>(value, ReadObject);

            var jvalue = value as JValue;
            if (jvalue != null) return jvalue.Value;

            return value;
        }

        public static T ReadStronglyTypedObject<T>(JToken token) where T : class
        {
            var value = GetJToken<JObject>(token);
            if (value == null) return null;
            return (T)Activator.CreateInstance(typeof(T), new object[] { token });

        }


        public delegate T ValueReader<T>(JToken token);



        public static T[] ReadArray<T>(JToken token, ValueReader<T> reader)
        {
            var value = GetJToken<JArray>(token);
            if (value == null) return null;

            var array = new T[value.Count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = reader(value[i]);
            }
            return array;

        }



        public static Dictionary<string, T> ReadDictionary<T>(JToken token)
        {
            var value = GetJToken<JObject>(token);
            if (value == null) return null;

            var dict = new Dictionary<string, T>();

            return dict;
        }

        public static Array ReadArray<K>(JArray jArray, ValueReader<K> reader, Type type)
        {
            if (jArray == null) return null;

            var elemType = type.GetElementType();

            var array = Array.CreateInstance(elemType, jArray.Count);
            for (var i = 0; i < array.Length; i++)
            {
                if (elemType.IsArray)
                {
                    array.SetValue(ReadArray<K>(GetJToken<JArray>(jArray[i]), reader, elemType), i);
                }
                else
                {
                    array.SetValue(reader(jArray[i]), i);
                }

            }
            return array;

        }
    }
}
