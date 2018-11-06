using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace SMEAppHouse.Core.CodeKits.Extensions
{
    public static class EnumExt
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetEnumValues<T>() where T : new()
        {
            var valueType = new T();
            return typeof(T).GetFields()
                .Select(fieldInfo => (T)fieldInfo.GetValue(valueType))
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetEnumNames<T>()
        {
            return typeof(T).GetFields()
                .Select(info => info.Name)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Return the translation of an enumeration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetEnumName<T>(T t)
        {
            return Enum.GetName(typeof(T), (T)t);
        }

        /// <summary>
        /// Extension method to get the description of a enumeration using
        /// the <see cref="DescriptionAttribute"/>.
        /// </summary>
        /// <param name="currentEnum">The current enum.</param>
        /// <returns>The description attribute value or enum's string representation.</returns>
        public static string GetDescription(this Enum currentEnum)
        {
            var fi = currentEnum.GetType().GetField(currentEnum.ToString());
            var da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            var description = da != null ? da.Description : currentEnum.ToString();
            return description;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/479410/enum-tostring-with-user-friendly-strings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerationValue"></param>
        /// <returns></returns>
        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            var type = enumerationValue.GetType();

            if (!type.IsEnum)
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length <= 0) return enumerationValue.ToString();
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : enumerationValue.ToString();
            //If we have no description attribute, just return the ToString of the enum
        }

        /// <summary>
        /// Gets all items for an enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAllItems<T>(this Enum value)
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                yield return (T)item;
            }
        }

        /// <summary>
        /// Gets all items for an enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAllItems<T>() where T : struct
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                yield return (T)item;
            }
        }

        /// <summary>
        /// Gets all combined items from an enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <example>
        /// Displays ValueA and ValueB.
        /// <code>
        /// EnumExample dummy = EnumExample.Combi;
        /// foreach (var item in dummy.GetAllSelectedItems<EnumExample>())
        /// {
        ///    Console.WriteLine(item);
        /// }
        /// </code>
        /// </example>
        public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
        {
            var valueAsInt = Convert.ToInt32(value, CultureInfo.InvariantCulture);

            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var itemAsInt = Convert.ToInt32(item, CultureInfo.InvariantCulture);

                if (itemAsInt == (valueAsInt & itemAsInt))
                {
                    yield return (T)item;
                }
            }
        }

        /// <summary>
        /// Determines whether the enum value contains a specific value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///     <c>true</c> if value contains the specified value; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// EnumExample dummy = EnumExample.Combi;
        /// if (dummy.Contains<EnumExample>(EnumExample.ValueA))
        /// {
        ///     Console.WriteLine("dummy contains EnumExample.ValueA");
        /// }
        /// </code>
        /// </example>
        public static bool Contains<T>(this Enum value, T request)
        {
            var valueAsInt = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            var requestAsInt = Convert.ToInt32(request, CultureInfo.InvariantCulture);

            if (requestAsInt == (valueAsInt & requestAsInt))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// method to read in an enum and return a string array
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T[] EnumToArray<T>()
        {
            var enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T must be a System.Enum");
            }
            return (Enum.GetValues(enumType) as IEnumerable<T>).ToArray();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="coll"></param>
        ///// <returns></returns>
        //public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        //{
        //    var c = new ObservableCollection<T>();
        //    foreach (var e in coll) c.Add(e);
        //    return c;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCare"></param>
        /// <returns></returns>
        public static TEnum ParseEnum<TEnum>(string value, bool ignoreCare = true) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            TEnum tmp;
            if (!Enum.TryParse(value, ignoreCare, out tmp))
                tmp = new TEnum();

            return tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T GetRandomValue<T>()
        {
            //var v = Enum.GetValues(typeof(T));
            //return (T)v.GetValue(new Random().Next(v.Length));

            return Enum.GetValues(typeof(T)).Cast<T>().OrderBy(e => Guid.NewGuid()).First();
        }
    }
}