using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SMEAppHouse.Core.CodeKits.Exceptions
{
    public static class ExceptionHelpers
    {
        /// <summary>
        /// https://stackoverflow.com/a/18793185/3796898
        /// </summary>
        /// <param name="e"></param>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public static string GetExceptionMessages(this Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            return msgs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <param name="includeInnerExceptions"></param>
        /// <param name="caller"></param>
        /// <param name="lineNo"></param>
        /// <returns></returns>
        public static T ThrowException<T>(T exception, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0) where T : Exception
        {
            return ThrowException<T>(exception, true, "", caller, lineNo);
        }

        /// <summary>
        /// http://grenangen.se/node/75
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <param name="includeInnerExceptions"></param>
        /// <param name="prefix"></param>
        /// <param name="caller"></param>
        /// <param name="lineNo"></param>
        /// <returns></returns>
        public static T ThrowException<T>(T exception, bool includeInnerExceptions, string prefix, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNo = 0) where T : Exception
        {
            var pref = !string.IsNullOrEmpty(prefix) ? $"{prefix} -> " : "";
            return (T)new Exception($"{pref}Called by:{caller} @ Line No.:{lineNo} Details: {(includeInnerExceptions ? exception.GetExceptionMessages() : exception.Message)}");
        }

        #region http://stackoverflow.com/a/13664823/3796898

        public static IEnumerable<Exception> GetAllExceptions(this Exception ex)
        {
            var currentEx = ex;
            yield return currentEx;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx;
            }
        }

        public static IEnumerable<string> GetAllExceptionAsString(this Exception ex)
        {
            var currentEx = ex;
            yield return currentEx.ToString();
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx.ToString();
            }
        }

        public static IEnumerable<string> GetAllExceptionMessages(this Exception ex)
        {
            var currentEx = ex;
            yield return currentEx.Message;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx.Message;
            }
        }

        #endregion
    }
}
