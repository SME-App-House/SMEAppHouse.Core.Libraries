// ***********************************************************************
// Assembly         : SMEAppHouse.Core.CodeKits
// Author           : jcman
// Created          : 03-10-2018
//
// Last Modified By : jcman
// Last Modified On : 03-11-2018
// ***********************************************************************
// <copyright file="AppDotTicker.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Threading;

namespace SMEAppHouse.Core.CodeKits.Tools
{
    public class AppDotTicker<T>
    {
        public Action OnCompletionEvent { get; set; }
        public Action OnTickEvent { get; set; }
        public int DelayMillisec { get; set; }

        private volatile bool _halt/* = false*/;
        private volatile bool _shutdown/* = false*/;

        public bool IsActive { get; private set; }

        public AppDotTicker() : this(1000)
        {
        }

        public AppDotTicker(int delayInterval)
        {
            DelayMillisec = delayInterval;

            new Thread(() =>
            {
                lock (typeof(T))
                {
                    while (true)
                    {
                        if (_shutdown)
                        {
                            OnCompletionEvent?.Invoke();
                            break;
                        }

                        if (_halt) continue;

                        Thread.Sleep(DelayMillisec);
                        OnTickEvent?.Invoke();

                    }

                    IsActive = false;
                }
            })
            {
                IsBackground = true
            }
            .Start();
        }
        public void Resume()
        {
            _halt = false;
            IsActive = true;
        }

        public void Stop()
        {
            _halt = true;
            IsActive = false;
        }

        public void Shutdown()
        {
            _halt = true;
            _shutdown = true;
            IsActive = false;
        }
    }
}
