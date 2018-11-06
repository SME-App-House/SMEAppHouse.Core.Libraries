using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SMEAppHouse.Core.CodeKits.Extensions;
using SMEAppHouse.Core.CodeKits.Helpers.Expressions;
using Timer = System.Timers.Timer;

namespace SMEAppHouse.Core.CodeKits
{
    public static class CodeKit
    {
        private delegate bool DirectoryExistsDelegate(string folder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Copies all public, readable properties from the source object to the
        /// target. The target type does not have to have a parameterless constructor,
        /// as no new instance needs to be created.
        /// </summary>
        /// <remarks>Only the properties of the source and target types themselves
        /// are taken into account, regardless of the actual types of the arguments.</remarks>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <typeparam name="TTarget">Type of the target</typeparam>
        /// <param name="source">Source to copy properties from</param>
        /// <param name="target">Target to copy properties to</param>
        public static void CopyObjectProperties<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            PropertyCopier<TSource, TTarget>.Copy(source, target);
        }

        public static bool DirectoryExists(string path, int millisecondsTimeout = 5000)
        {
            try
            {
                var callback = new DirectoryExistsDelegate(Directory.Exists);
                var result = callback.BeginInvoke(path, null, null);

                if (result.AsyncWaitHandle.WaitOne(millisecondsTimeout, false))
                {
                    return callback.EndInvoke(result);
                }
                else
                {
                    callback.EndInvoke(result);  // Needed to terminate thread?

                    return false;
                }
            }

            catch (Exception)
            {
                return false;
            }
        }

        public static long GetObjectSize(object testObject)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, testObject);
            var array = ms.ToArray();
            return array.Length;
        }

        /// <summary>
        /// Generate random integer between min and max.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        public static TimeSpan RandomTime(int startHour, int endHour)
        {
            var rand = new Random();
            var startTime = Convert.ToInt32(TimeSpan.FromHours(startHour).TotalMinutes);
            var endTime = Convert.ToInt32(TimeSpan.FromHours(endHour).TotalMinutes) + 1;     // e.g. To make 11:00 inclusive

            var timeSpans = Enumerable.Range(1, 10000)
                .Select(v => TimeSpan.FromMinutes(rand.Next(startTime, endTime)));
            var randIdx = rand.Next(1, 1000);
            return timeSpans.ToList()[randIdx];
        }


        /// <summary>
        /// Calculate Number of Pages (for paging)
        /// http://www.codekeep.net/snippets/d6bb7cac-856b-4e0b-84d1-74633c3a8298.aspx
        /// Dave Donaldson
        /// </summary>
        /// <param name="totalNumberOfItems"></param>
        /// <param name="pageSize"></param>
        /// <returns>Integer of pages required when paging.</returns>
        public static int CalculateNumberOfPages(int totalNumberOfItems, int pageSize)
        {
            var result = totalNumberOfItems % pageSize;
            if (result == 0)
                return totalNumberOfItems / pageSize;
            else
                return totalNumberOfItems / pageSize + 1;
        }


        /// <summary>
        /// Extract only the hex digits from a string.
        /// </summary>
        public static string ExtractHexDigits(string input)
        {
            // remove any characters that are not digits (like #)
            var isHexDigit
               = new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
            var newnum = "";
            foreach (var c in input)
            {
                if (isHexDigit.IsMatch(c.ToString()))
                    newnum += c.ToString();
            }
            return newnum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(T item) where T : class
        {
            return typeof(T).GetProperties()[0].Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="killProc"></param>
        /// <returns></returns>
        public static bool FindApplicationProcess(string procName, bool killProc = false)
        {
            var exeName = string.Empty;
            return FindApplicationProcess(procName, ref exeName, killProc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="exeName"></param>
        /// <param name="killProc"></param>
        /// <returns></returns>
        public static bool FindApplicationProcess(string procName, ref string exeName, bool killProc = false)
        {
            var found = false;
            foreach (var clsProcess in Process.GetProcesses())
            {
                if (!clsProcess.ProcessName.Contains(procName)) continue;
                found = true;
                clsProcess.Refresh();
                exeName = clsProcess.MainModule.FileName;
                if (killProc && !clsProcess.HasExited)
                    clsProcess.Kill();
            }
            return found;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sOriginal"></param>
        /// <returns></returns>
        public static string StringToBase64(string sOriginal)
        {
            var byt = Encoding.UTF8.GetBytes(sOriginal);
            var target = Convert.ToBase64String(byt);
            return target;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sBase64Str"></param>
        /// <returns></returns>
        public static string Base64ToString(string sBase64Str)
        {
            var byt = Convert.FromBase64String(sBase64Str);
            var target = Encoding.UTF8.GetString(byt);
            return target;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ReverseString(string target)
        {
            var charArray = target.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TOut FuncInvoke<TOut>(Func<TOut> func)
        {
            return func();
        }

        /// <summary>
        /// Round a floating point value (double in C#) to a precision of 0.05. 
        /// Unfortunately, Math.Round only lets you round a value to the nearest decimal. That means you 
        /// can only round to 0.1, 0.01, 0.001, ... 
        /// To round your values to 0.05, or 0.25, or whatever, use the following function:
        /// </summary>
        /// <param name="x"></param>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public static double Round(double x, int numerator, int denominator)
        { // returns the number nearest x, with a precision of numerator/denominator
            // example: Round(12.1436, 5, 100) will round x to 12.15 (precision = 5/100 = 0.05)
            var y = (long)Math.Floor(x * denominator + (double)numerator / 2.0);
            return (double)(y - y % numerator) / (double)denominator;
        }

        /// <summary>
        /// Works like Task.Delay in .NET 4.5 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static Task Delay(double milliseconds)
        {
            var tcs = new TaskCompletionSource<bool>();
            var timer = new Timer();
            timer.Elapsed += (obj, args) => tcs.TrySetResult(true);
            timer.Interval = milliseconds;
            timer.AutoReset = false;
            timer.Start();
            return tcs.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amountOfTime"></param>
        /// <param name="timeGranule"></param>
        public static void Delay2(double amountOfTime, Rules.TimeIntervalTypesEnum timeGranule = Rules.TimeIntervalTypesEnum.MilliSeconds)
        {
            var dateSince = DateTime.Now;
            do
            {
                Thread.Sleep(1);
            }
            while (DateTimeExt.CalculateElapsedTime(dateSince, DateTime.Now, timeGranule) < amountOfTime);
        }

        public static Task Delay3(int ms, Action doThis)
        {
            return Task.Factory.StartNew(() =>
            {
                Task.Delay(ms);
                doThis();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Stream StringToStream(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="test"></param>
        /// <returns></returns>
        public static bool IsEntryFound(string entry, params string[] test)
        {
            return IsEntryFound(entry, true, test);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="caseDownAll"></param>
        /// <param name="test"></param>
        /// <returns></returns>
        public static bool IsEntryFound(string entry, bool caseDownAll, params string[] test)
        {
            if (caseDownAll)
            {
                var _test = test.Select(p => p.ToLower()).ToArray();
                return _test.Contains(entry.ToLower());
            }
            else
                return test.Contains(entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int GetHashCodeSafe<T>(T target)
        {
            return target == null ? 0 : target.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvertToGeneric<T>(object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<string> GetAllUrLsInText(string text)
        {
            List<string> entries = null;

            //@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)"
            //"http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?"

            var regx = new Regex(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)", RegexOptions.IgnoreCase);
            var mactches = regx.Matches(text);
            foreach (Match match in mactches)
            {
                if (entries == null)
                    entries = new List<string>();

                entries.Add(match.Value);
            }
            return entries;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/540078/wait-for-pooled-threads-to-complete
        /// </summary>
        /// <param name="actions"></param>
        public static void SpawnAndWait(IEnumerable<Action> actions)
        {
            var enumerable = actions as Action[] ?? actions.ToArray();
            var list = enumerable.ToList();
            var handles = new ManualResetEvent[enumerable.Count()];
            for (var i = 0; i < list.Count; i++)
            {
                handles[i] = new ManualResetEvent(false);
                var currentAction = list[i];
                var currentHandle = handles[i];
                Action wrappedAction = () => { try { currentAction(); } finally { currentHandle.Set(); } };
                ThreadPool.QueueUserWorkItem(x => wrappedAction());
            }

            WaitHandle.WaitAll(handles);
        }

        public static bool AreStringsAnagrams(string a, string b)
        {
            var target1 = a.ToCharArray();
            var target2 = b.ToCharArray();

            Array.Sort(target1);
            Array.Sort(target2);

            return new string(target1).Equals(new string(target2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static bool IsArrayOf<T>(this Type type)
        {
            return type == typeof(T[]);
        }

    }
}
