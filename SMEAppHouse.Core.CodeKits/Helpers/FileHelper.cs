using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using SMEAppHouse.Core.CodeKits.Encryptions;

namespace SMEAppHouse.Core.CodeKits.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="includeTime"></param>
        /// <returns></returns>
        public static string FormatShortDateForfilename(DateTime date, bool includeTime = false)
        {
            var result = string.Empty;

            result += date.Year.ToString().PadLeft(4, '0');
            result += date.Month.ToString().PadLeft(2, '0');
            result += date.Day.ToString().PadLeft(2, '0');


            if (!includeTime) return result;
            result += date.Hour.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            result += date.Minute.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="prefix"></param>
        /// <param name="suffix"></param>
        /// <param name="extension"></param>
        /// <param name="includeTime"></param>
        /// <returns></returns>
        public static string FormatShortDateForfilename(DateTime date, string prefix, string suffix, string extension, bool includeTime = false)
        {
            prefix = !string.IsNullOrEmpty(prefix) ? $"{prefix}-" : "";
            suffix = !string.IsNullOrEmpty(suffix) ? $"-{suffix}" : "";
            extension = !string.IsNullOrEmpty(extension) ? $".{extension}" : "";

            var format = includeTime 
                ? $"{prefix}{date:yyyy-MM-dd_hh-mm-ss-tt}{suffix}{extension}" 
                : $"{prefix}{date:yyyy-MM-dd}{suffix}{extension}";

            return format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void WriteToFile2(string filePath, string data)
        {
            var sw = File.AppendText(filePath);
            try
            {
                sw.WriteLine(data);
            }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        /// <param name="append"></param>
        public static void WriteToFile(string filePath, string data, bool append = true)
        {
            try
            {
                using (var stream = new FileStream(filePath
                                                , append ? FileMode.Append : FileMode.Append
                                                , append ? FileAccess.Write : FileAccess.ReadWrite
                                                , FileShare.Read))
                {
                    using (var sw = new StreamWriter(stream) { AutoFlush = true })
                    {
                        sw.WriteLine(data);
                    }
                }

                //TextWriter tw = new StreamWriter(path, append);
                //tw.WriteLine(data);
                //tw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Read the file and display it line by line.
        /// </summary>
        /// <returns></returns>
        public static string[] ReadFromFileEachLine(string textFile)
        {
            try
            {
                List<string> result = null;
                var _line = string.Empty;
                var file = new StreamReader(textFile);

                while ((_line = file.ReadLine()) != null)
                {
                    if (result == null)
                        result = new List<string>();
                    result.Add(_line);
                }

                file.Close();

                if (result != null && result.Count > 0)
                    return result.ToArray();

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textFile"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadFromFileEachLine2(string textFile)
        {
            var line = string.Empty;
            using (var reader = File.OpenText(textFile))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                        yield return line;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textFile"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadFromFileEachLine3<T>(string textFile, Func<string, T> parserAction)
        {
            var line = string.Empty;
            using (var reader = File.OpenText(textFile))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        var _t = default(T);
                        if (parserAction != null)
                            _t = parserAction(line);
                        yield return _t;
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringData"></param>
        /// <param name="parserAction"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadFromFileEachLine3<T>(string[] stringData, Func<string, T> parserAction)
        {
            if (stringData != null && stringData.Count() > 0)
            {
                foreach (var s in stringData)
                {
                    var _t = default(T);
                    if (parserAction != null)
                        _t = parserAction(s);
                    yield return _t;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="?"></typeparam>
        /// <param name="textfile"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadFromFileEachLine4(string textfile)
        {
            List<string> _results = null;
            using (var stream = new FileStream(textfile,
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream))
                {

                    var _line = string.Empty;
                    while (reader.Peek() >= 0)
                    {
                        _line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(_line))
                        {
                            if (_results == null)
                                _results = new List<string>();

                            _results.Add(_line);
                        }
                    }
                }
            }
            return _results;
        }

        /// <summary>
        /// Calculate MD5 Checksum for a File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileMD5HashChecksum(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            var retVal = md5.ComputeHash(stream);
            stream.Close();

            //Best/Fastest method:
            return BitConverter.ToString(retVal).Replace("-", string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long GetFileSize(string filePath)
        {
            if (File.Exists(filePath))
            {
                var _fileInfo = new FileInfo(filePath);
                return _fileInfo.Length;
            }
            return 0;
        }

        /// <summary>
        /// Iterate on each file and subdirectory to change the Read-only file 
        /// attribute for each file in a folder
        /// </summary>
        /// <param name="directory"></param>
        public static void Recurse(DirectoryInfo directory, bool asReadOnly = false)
        {
            foreach (var fi in directory.GetFiles("*.*", SearchOption.AllDirectories))
            {
                fi.IsReadOnly = asReadOnly; // or true
            }

            foreach (var subdir in directory.GetDirectories("*", SearchOption.AllDirectories))
            {
                Recurse(subdir);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenPathUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool WriteContentToFile(string contentPathUri, string content, string encryptionKey = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(encryptionKey))
                    content = Cryptor.EncryptStringAES(content, encryptionKey);

                File.WriteAllText(contentPathUri, content);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="LogMessage"></param>
        public static void LogMessage(string logFile, string LogMessage)
        {
            try
            {
                using (var sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(LogMessage);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenPathUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool ReadContentFromFile(string contentPathUri, out string content, string encryptionKey = null)
        {
            try
            {
                content = File.ReadAllText(contentPathUri);

                if (!string.IsNullOrEmpty(encryptionKey))
                    content = Cryptor.DecryptStringAES(content, encryptionKey);

                return true;
            }
            catch { content = string.Empty; return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool IsFilePathAccessible(string filepath)
        {
            try
            {
                using (var stream = new FileStream(filepath, FileMode.Open))
                {
                    stream.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}