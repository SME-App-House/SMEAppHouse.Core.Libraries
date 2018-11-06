// ***********************************************************************
// Assembly         : SMEAppHouse.Core.CodeKits
// Author           : jcman
// Created          : 03-10-2018
//
// Last Modified By : jcman
// Last Modified On : 03-11-2018
// ***********************************************************************
// <copyright file="GenericComparer.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace SMEAppHouse.Core.CodeKits.Tools
{
    public class GenericComparer<T> : IEqualityComparer<T> where T : class
    {
        private readonly Func<T, object> _expr;
        private readonly Func<T, T, bool> _qualifier;

        public GenericComparer(Func<T, object> assessor)
            : this(assessor, null)
        {
        }

        public GenericComparer(Func<T, object> assessor, Func<T, T, bool> qualifier)
        {
            _expr = assessor;
            _qualifier = qualifier;
        }

        public bool Equals(T x, T y)
        {
            var first = _expr.Invoke(x);
            var sec = _expr.Invoke(y);
            return first != null && first.Equals(sec)
                && (_qualifier == null || _qualifier != null && _qualifier(x, y));
        }
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
