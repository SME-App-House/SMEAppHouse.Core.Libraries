using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SMEAppHouse.Core.CodeKits.Extensions
{
    public static class ThreadExtension
    {
        public static void WaitAll(this IEnumerable<Thread> threads)
        {
            if (threads == null) return;
            foreach (var thread in threads)
            { thread.Join(); }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/14630770/sequential-processing-of-asynchronous-tasks
        /// </summary>
        /// <param name="onComplete"></param>
        /// <param name="errorHandler"></param>
        /// <param name="actions"></param>
        public static void RunSequential(Action onComplete, Action<Exception> errorHandler,
                          params Func<Task>[] actions)
        {
            RunSequential(onComplete, errorHandler,
                          actions.AsEnumerable().GetEnumerator());
        }

        public static void RunSequential(Action onComplete, Action<Exception> errorHandler,
                                  IEnumerator<Func<Task>> actions)
        {
            if (!actions.MoveNext())
            {
                onComplete();
                return;
            }

            var task = actions.Current();
            task.ContinueWith(t => errorHandler(t.Exception),
                              TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => RunSequential(onComplete, errorHandler, actions),
                              TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        ///// <summary>
        ///// When you're calling UI stuff from a non-UI thread (i.e. the FileSystemWatcher 
        ///// is firing the event from a non-UI thread) you get an exception: 
        ///// The calling thread must be STA, because many UI components require this. 
        ///// This extension method solves it.
        ///// 
        ///// Usage E.g:
        ///// 
        ///// this.Dispatcher.InvokeOrExecute(() => { 
        /////         var imagePreview = new ImagePreview();
        /////         imagePreview.Show(); 
        ///// });
        ///// 
        ///// </summary>
        ///// <param name="dispatcher"></param>
        ///// <param name="action"></param>
        //public static void InvokeOrExecute(this Dispatcher dispatcher, Action action)
        //{
        //    if (dispatcher.CheckAccess()) action();
        //    else dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        //}
    }
}
