using System;
using System.Collections.Generic;
using System.Linq;

namespace SMEAppHouse.Core.CodeKits.Helpers.Expressions
{
    public class PropertyVariance
    {
        public string Prop { get; set; }

        public object ValA { get; set; }

        public object ValB { get; set; }

        /// <summary>
        /// http://stackoverflow.com/questions/4951233/compare-two-objects-and-find-the-differences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<PropertyVariance> DetailedCompare<T>(T target, T source, string[] propertyIgnores)
        {
            var props = target.GetType().GetProperties().ToList();

            Func<string, bool> containsName = (s) =>
            {
                if (propertyIgnores != null && propertyIgnores.Any())
                {
                    return propertyIgnores.Contains(s);
                }
                return false;
            };

            if (!props.Any()) return null;
            var variances = new List<PropertyVariance>();
            foreach (var p in props)
            {
                var v = new PropertyVariance();

                if (containsName(p.Name)) continue;
                v.Prop = p.Name;
                v.ValA = p.GetValue(target);
                v.ValB = p.GetValue(source);

                if ((v.ValA != null && !v.ValA.Equals(v.ValB)) |
                    (v.ValA == null && v.ValB != null) |
                    (v.ValA != null && v.ValB == null))
                    variances.Add(v);
            }
            return variances;
        }
    }
}