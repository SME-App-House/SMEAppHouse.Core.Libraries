using System;
using System.Threading;

namespace SMEAppHouse.Core.CodeKits.Tools
{
    public static class RetryCodeKit
    {
        public static bool TryAndIgnore(Action tryAction)
        {
            Exception x = null;
            return TryAndIgnore(tryAction, ref x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tryAction"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool TryAndIgnore(Action tryAction, ref Exception exception)
        {
            Exception x = null;
            try
            {
                tryAction();
                return true;
            }
            catch (Exception ex)
            {
                x = ex;
                return false;
            }
        }

        public static void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, retryCount);
        }
        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3, bool ignoreFinalException = false)
        {
            Exception exception = null;

            for (var retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Thread.Sleep(retryInterval);
                }
            }

            if (!ignoreFinalException && exception != null)
                throw exception;

            return default(T);
        }

        /// <summary>
        /// int retryCtr, Exception exception, ref bool cancelExec
        /// </summary>
        /// <param name="retryCtr"></param>
        /// <param name="exception"></param>
        /// <param name="cancelExec"></param>
        public delegate void DoWhileErrorCallbackDelegate(int retryCtr, Exception exception, ref bool cancelExec);

        public static bool DoWhileError(Action action,
                                        int iterationLimit = 3,
                                        int iterationTimeout = 3000)
        {
            DoWhileErrorCallbackDelegate exceptionOccuredEvent = delegate { };
            //Action<int, Exception, bool> exceptionOccuredEvent = null;
            return DoWhileError(action, exceptionOccuredEvent, iterationLimit, iterationTimeout);
        }

        public static bool DoWhileError(Action action,
                                        DoWhileErrorCallbackDelegate exceptionOccuredEvent, //Action<int, Exception, bool> exceptionOccuredEvent,
                                        int iterationLimit = 3,
                                        int iterationTimeout = 3000)
        {
            if (action == null) return true;
            var iteration = 0;

            while (true)
            {
                try
                {
                    action();
                    return true;
                }
                catch (Exception exception)
                {
                    var cancel = false;
                    var ex = exception;
                    exceptionOccuredEvent?.Invoke(iteration, ex, ref cancel);

                    if (cancel)
                        break;
                }

                if (iterationLimit > 0)
                {
                    if (iteration == iterationLimit)
                        return false;
                    else
                        iteration++;
                }
                Thread.Sleep(iterationTimeout);
            }
            return false;
        }

        public static bool LoopRetry(Func<bool> retryAction,
                                    Func<bool> successQualifier,
                                    int iterationLimit = 0,
                                    int iterationTimeout = 5000)
        {
            Exception exceptionThrown = null;
            return LoopRetry(retryAction, successQualifier, null,
                                    ref exceptionThrown,
                                    iterationLimit, iterationTimeout);
        }
        public static bool LoopRetry(Func<bool> retryAction,
                                    Func<bool> successQualifier,
                                    Action<Exception> extraRoutineAfterFailure,
                                    ref Exception exceptionThrown,
                                    int iterationLimit = 0,
                                    int iterationTimeout = 5000)
        {
            if (retryAction == null || successQualifier == null) return false;
            var ctr = 0;
            do
            {
                ctr++;

                var succ = Retry(retryAction, ref exceptionThrown);

                if (extraRoutineAfterFailure != null && exceptionThrown != null)
                    extraRoutineAfterFailure(exceptionThrown);

                var ok = successQualifier();

                if (ok) return succ;

                if ((ctr == iterationLimit) || (iterationLimit > 0 && ctr == iterationLimit))
                    break;

            }
            while (true);
            return false;
        }

        public static bool Retry(Func<bool> retryAction, int iteration = 5, int iterationTimeout = 5000)
        {
            Exception exceptionThrown = null;
            return Retry(retryAction, ref exceptionThrown, iteration, iterationTimeout);
        }
        public static bool Retry(Func<bool> retryAction, ref Exception finalExceptionThrown, int maxretry = 5, int iterationTimeout = 5000)
        {
            if (retryAction == null) return false;
            for (var ctr = 0; ctr < maxretry; ctr++)
            {
                try
                {
                    if (retryAction())
                    {
                        finalExceptionThrown = null;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    finalExceptionThrown = ex;
                }
                CodeKit.Delay2(iterationTimeout);
            }
            return false;
        }

        public static void LoopAction(Func<bool> loopAction,
                                    bool exitWhenActionIsTrue = true,
                                    int iterationLimit = 0,
                                    int iterationTimeout = 5000)
        {
            Exception exceptionThrown = null;
            LoopAction(loopAction, ref exceptionThrown, exitWhenActionIsTrue, iterationLimit, iterationTimeout);

        }
        public static void LoopAction(Func<bool> loopAction,
                                    ref Exception exceptionThrown,
                                    bool exitWhenActionIsTrue,
                                    int iterationLimit = 0,
                                    int iterationTimeout = 5000)
        {
            if (loopAction == null) return;
            var itr = 0;
            while (true)
            {
                try
                {
                    if (iterationLimit > 0 &&
                        itr >= iterationLimit)
                        break;
                    else
                        itr++;

                    var result = loopAction();

                    if (exitWhenActionIsTrue && result)
                        break;

                    CodeKit.Delay(iterationTimeout);
                }
                catch (Exception ex)
                {
                    exceptionThrown = ex;
                    break;
                }
            }
        }

    }
}
