using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SMEAppHouse.Core.ProcessService.Specials
{
    public static class Utils
    {
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

                void WrappedAction()
                {
                    try
                    {
                        currentAction();
                    }
                    catch (AggregateException aEx)
                    {
                        throw aEx;
                    }
                    finally
                    {
                        currentHandle.Set();
                    }
                }

                ThreadPool.QueueUserWorkItem(x => WrappedAction());
            }

            WaitHandle.WaitAll(handles);

            handles.ToList().ForEach(h =>
            {
                h.Close();
                h.Dispose();
            });
        }

        /// <summary>
        /// http://geekswithblogs.net/mnf/archive/2007/05/28/WaitAll-to-support-calls-from-WindowsForms.aspx
        /// </summary>
        /// <param name="eventList"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public static bool WaitAll(List<ManualResetEvent> eventList, int millisecondsTimeout)
        {//Casting List<> of derived class to List<> of base class? see http://www.thescripts.com/forum/thread276346.html
            //new Converter<ManualResetEvent, WaitHandle>(ConvertDelegate));

            //and http://www.theserverside.net/tt/articles/showarticle.tss?id=AnonymousMethods
            var waitHandles = eventList.ConvertAll<WaitHandle>(ev => (WaitHandle)ev);
            return WaitAll(waitHandles, millisecondsTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="waitHandles"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public static bool WaitAll(List<WaitHandle> waitHandles, int millisecondsTimeout)
        {
            var bRet = true;

            //to avoid "WaitAll for multiple handles on an STA thread is not supported." SEE http://www.devnewsgroups.net/group/microsoft.public.dotnet.framework/topic28609.aspx
            //alternative solutions described in http://www.issociate.de/board/post/250510/WaitAll_for_multiple_handles_on_an_STA_thread_is_not_supported.html
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                //usually happens when tested from TestHarness or NUnit
                waitHandles.ForEach(handle =>
                {
                    bRet |= handle.WaitOne();
                });
            }
            else
            {
                bRet = millisecondsTimeout > 0
                        ? WaitHandle.WaitAll(waitHandles.ToArray(), millisecondsTimeout, false)
                        : WaitHandle.WaitAll(waitHandles.ToArray());
            }

            return bRet;
        }
    }
}
