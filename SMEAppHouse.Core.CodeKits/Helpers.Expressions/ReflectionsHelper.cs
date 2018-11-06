using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SMEAppHouse.Core.CodeKits.Helpers.Expressions
{
    public static class ReflectionsHelper
    {
        public static object GetPropertyValue<T>(this T adListing, string propertyName)
        {
            return adListing.GetType().GetProperties()
               .Single(pi => pi.Name == propertyName)
               .GetValue(adListing, null);
        }

        public static PropertyInfo GetPropertyInfo<T>(this T target, string propertyName)
        {
            return target.GetType().GetProperties()
                .Single(pi => pi.Name == propertyName);
        }

        public static void CopyPropertyValues(object source, object destination)
        {
            var destProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                foreach (var destProperty in destProperties)
                {
                    if (destProperty.Name == sourceProperty.Name &&
                destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    {
                        destProperty.SetValue(destination, sourceProperty.GetValue(
                            source, new object[] { }), new object[] { });

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="fieldSelector"></param>
        /// <returns></returns>
        public static bool UpdateWhenNotEqual<T>(this T target, T source, Expression<Func<T, object>> fieldSelector)
        {
            var propertyName = GetCorrectPropertyName(fieldSelector);
            return target.UpdateWhenNotEqual(source, propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool UpdateWhenNotEqual<T>(this T target, T source, string propertyName)
        {
            var propsTgt = target.GetType().GetProperties().ToList();
            var propsSrc = source.GetType().GetProperties().ToList();

            var propTgt = propsTgt.FirstOrDefault(p => p.Name == propertyName);
            var propSrc = propsSrc.FirstOrDefault(p => p.Name == propertyName);

            if (propTgt == null || propSrc == null) return false;
            var tgtVal = propTgt.GetValue(target);
            var srcVal = propSrc.GetValue(source);

            if (!((tgtVal != null && !tgtVal.Equals(srcVal)) |
                  (srcVal != null && !srcVal.Equals(tgtVal)))) return false;

            propTgt.SetValue(target, srcVal);
            return true;
        }

        public static string GetCorrectPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression)expression.Body).Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }

        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var dString = dictionary.ToDictionary(k => k.Key, k => k.Value == null ? "" : k.Value.ToString());
            var str = "{" + string.Join(",", dString.Select(kv => kv.Key.ToString() + "=" + kv.Value.ToString()).ToArray()) + "}";
            return str;
        }

        public static bool IsOfAnyEnumerableType(object obj)
        {
            var type = obj.GetType();
            var result = (typeof(IEnumerable).IsAssignableFrom(type));
            return result;
        }

        public static bool IsOfCollectionType(object obj)
        {
            var type = obj.GetType();
            var ifaces = type.GetInterfaces();

            return obj.GetType()
                        .GetInterfaces()
                        .Any(x => x.IsGenericType &&
                            x.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        public static string GetVariableName(object variable)
        {
            var result = variable.GetType().GetProperty("Name", typeof(string)).GetValue(variable, null).ToString();
            return result;
        }

        public static bool IsPropertyOfCollectionType(PropertyInfo property)
        {
            if (property.PropertyType.AssemblyQualifiedName == null)
                throw new InvalidOperationException("operation required a non-nulled property parameter");

            return property.PropertyType.AssemblyQualifiedName.Contains("System.Collections.Generic.ICollection");
        }

        public static Dictionary<string, string> GetFieldValues(object obj)
        {
            var resultsDic = obj.GetType()
                      .GetFields(BindingFlags.Public | BindingFlags.Static)
                      .Where(f => f.FieldType == typeof(string))
                      .ToDictionary(f => f.Name, f => (string)f.GetValue(null));
            return resultsDic;
        }

        public static Dictionary<string, object> GetPropertyValues(object obj, params string[] fieldExemptions)
        {
            var type = obj.GetType();
            if (!type.IsClass)
                throw new InvalidOperationException("An object of class type is required in parameter");

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            try
            {
                var result = (from p in properties
                              let isCollection = IsOfCollectionType(p)
                              let isCollection2 = IsPropertyOfCollectionType(p)
                              where !isCollection && !isCollection2
                                 && (fieldExemptions == null || (!fieldExemptions.Contains(p.Name)))
                              select p)
                              .ToDictionary(p => p.Name, p => p.GetValue(obj, null));

                return result;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public static string GetTypeName<T>(T item) where T : class
        {
            return typeof(T).GetProperties()[0].Name;
        }

        public static void SetProperties(object source, object target)
        {
            var customerType = target.GetType();
            foreach (var prop in source.GetType().GetProperties())
            {
                var propGetter = prop.GetGetMethod();
                var propSetter = customerType.GetProperty(prop.Name).GetSetMethod();
                var valueToSet = propGetter.Invoke(source, null);
                propSetter.Invoke(target, new[] { valueToSet });
            }
        }

        public static object ChangeTypeEx(object srcVal, Type conversionType)
        {
            object theVal;

            if (srcVal == null)
                theVal = conversionType == typeof(string) ? string.Empty : Activator.CreateInstance(conversionType);
            else
                theVal = conversionType.IsEnum
                    ? Enum.Parse(conversionType, srcVal.ToString())
                    : Convert.ChangeType(srcVal, conversionType);

            return theVal;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/4951233/compare-two-objects-and-find-the-differences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<PropertyVariance> DetailedCompare<T>(this T target, T source, string[] propertyIgnores)
        {
            return PropertyVariance.DetailedCompare(target, source, propertyIgnores);
        }
        
    }
}
