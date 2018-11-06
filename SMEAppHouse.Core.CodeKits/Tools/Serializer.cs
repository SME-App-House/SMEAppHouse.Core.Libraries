using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SMEAppHouse.Core.CodeKits.Tools
{
    public enum SerializationFormatterEnum
    {
        Binary,
        Xml
    }

    public static class Serializer
    {
        /// <summary>
        /// 
        /// </summary>
        #region Serialization methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeToXml<T>(T value)
        {
            try
            {
                //var serializer = new XmlSerializer(obj.GetType());
                //using (var writer = new StringWriter())
                //{
                //    serializer.Serialize(writer, obj);
                //    return writer.ToString();
                //}

                if (value == null)
                    return null;

                var rootAttr = new XmlRootAttribute()
                {
                    Namespace = typeof(T).Namespace,
                    ElementName = typeof(T).Name,
                    IsNullable = true
                };

                var serializer = new XmlSerializer(typeof(T), rootAttr);
                var settings = new XmlWriterSettings
                {
                    Encoding = new UnicodeEncoding(false, false),
                    Indent = false,
                    OmitXmlDeclaration = false
                };

                using (var textWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, value);
                    }
                    return textWriter.ToString();
                }

            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to serialize {value}";
                throw new Exception(errMsg, sX);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string xml)
        {
            try
            {
                if (string.IsNullOrEmpty(xml))
                    return default(T);

                var rootAttr = new XmlRootAttribute()
                {
                    Namespace = typeof(T).Namespace,
                    ElementName = typeof(T).Name,
                    IsNullable = true
                };

                var serializer = new XmlSerializer(typeof(T), rootAttr);

                var settings = new XmlReaderSettings();

                // No settings need modifying here
                using (var textReader = new StringReader(xml))
                {
                    using (var xmlReader = XmlReader.Create(textReader, settings))
                    {
                        return (T)serializer.Deserialize(xmlReader);
                    }
                }

                //var serializer = new XmlSerializer(typeof(T));
                //using (var reader = new StringReader(xml))
                //{
                //    return (T)serializer.Deserialize(reader);
                //}
            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to deserialize XML string into {typeof (T)}";
                throw new Exception(errMsg, sX);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString<T>(T obj)
        {
            string result;
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                using (TextReader tr = new StreamReader(ms))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    result = tr.ReadToEnd();
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString2<T>(T obj)
        {
            string result;
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                var msArr = ms.ToArray();
                result = Encoding.UTF8.GetString(msArr);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serData"></param>
        /// <returns></returns>
        public static T DeserializeFromString<T>(string serData)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(Encoding.Default.GetBytes(serData)))
            {
                ms.Position = 0;
                return (T)bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="pathSpec"></param>
        /// <param name="serializationFormatterEnum"></param>
        public static void SerializeToFile<T>(T obj, string pathSpec, SerializationFormatterEnum serializationFormatterEnum)
        {
            try
            {
                switch (serializationFormatterEnum)
                {
                    case (SerializationFormatterEnum.Binary):

                        /*
                         using (FileStream fs = File.Open("MyFile.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                         {
                             // use fs
                         }
                         */

                        using (var fs = new FileStream(pathSpec, FileMode.Create,
                                            FileAccess.Write, FileShare.Write))
                            (new BinaryFormatter()).Serialize(fs, obj);

                        break;

                    case (SerializationFormatterEnum.Xml):
                        var serializer = new XmlSerializer(typeof(T));
                        TextWriter textWriter = new StreamWriter(pathSpec);
                        serializer.Serialize(textWriter, obj);
                        textWriter.Close();
                        break;

                    default:
                        throw new Exception("Invalid Formatter option");
                }
            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to serialize {obj} into file {pathSpec}";
                throw new Exception(errMsg, sX);
            }
            catch (Exception ex)
            {
                var errMsg = $"Unable to serialize {obj} into file {pathSpec}. Detail: {ex.Message}";
                throw new Exception(errMsg, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathSpec"></param>
        /// <param name="serializationFormatterEnum"></param>
        /// <returns></returns>
        public static T DeserializeFromFile<T>(string pathSpec, SerializationFormatterEnum serializationFormatterEnum) where T : class
        {
            try
            {
                switch (serializationFormatterEnum)
                {
                    case (SerializationFormatterEnum.Binary):
                        using (var strm = new FileStream(pathSpec,
                                            FileMode.Open, FileAccess.Read))
                        {
                            IFormatter fmt = new BinaryFormatter();
                            var o = fmt.Deserialize(strm);
                            if (!(o is T))
                                throw new ArgumentException("Bad Data File");
                            return o as T;
                        }

                    case (SerializationFormatterEnum.Xml):

                        TextReader rdr = new StreamReader(pathSpec);
                        var serializer = new XmlSerializer(typeof(T));

                        var obj = (T)serializer.Deserialize(rdr);
                        rdr.Close();

                        return obj;

                    default:
                        throw new Exception("Invalid Formatter option");
                }
            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to deserialize {typeof (T)} from file {pathSpec}";
                throw new Exception(errMsg, sX);
            }
        }

        #endregion Serialization methods
    }

}
