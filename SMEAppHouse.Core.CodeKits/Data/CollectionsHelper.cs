using System;
using System.Collections.Generic;
using System.Linq;

namespace SMEAppHouse.Core.CodeKits.Data
{
    public static class CollectionsHelper
    {

        /// <summary>
        /// A ForEach extension method for the enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="numberOfItems"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetItems<T>(this IList<T> list, int numberOfItems) //Can also handle arrays
        {
            for (var index = Math.Max(list.Count - numberOfItems, 0); index < list.Count; index++)
                yield return list[index];
        }

        /// <summary>
        /// Pull an item from the queue based on a supplied condition.
        /// </summary>
        /// <param name="queuedList"></param>
        /// <param name="qualifier"></param>
        /// <returns></returns>
        public static T DequeueConditional<T>(Queue<T> queuedList, Func<T, bool> qualifier)
        {
            if (queuedList == null || queuedList.Count <= 0) return default(T);

            for (var i = 0; i < queuedList.Count; i++)
            {
                T item;

                lock (queuedList)
                {
                    item = queuedList.Dequeue();
                }

                if (qualifier(item))
                    return item;

                lock (queuedList)
                {
                    queuedList.Enqueue(item);
                }
            }
            return default(T);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queuedList"></param>
        /// <param name="numberOfItems"></param>
        /// <param name="qualifier"></param>
        /// <returns></returns>
        public static List<T> DequeueItems<T>(this Queue<T> queuedList, int numberOfItems, Func<T, bool> qualifier)
        {
            if (queuedList != null && !queuedList.Any()) return null;

            var results = new List<T>();

            for (var i = 0; i < numberOfItems; i++)
            {
                if (queuedList == null || queuedList.Count == 0) continue;
                var item = DequeueConditional(queuedList, qualifier);
                if (item != null) results.Add(item);
            }
            return results;
        }

        public static List<T> DequeueItems<T>(this Queue<T> queuedList, int numberOfItems)
        {
            return DequeueItems<T>(queuedList, numberOfItems, (i) => true);
        }
    }
}
