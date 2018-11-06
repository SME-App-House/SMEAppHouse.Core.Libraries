using System;

namespace SMEAppHouse.Core.CodeKits.Extensions
{
    class Person
    {
        /// <summary>
        /// 
        /// </summary>
        public int Age { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime Birthdate
        {
            get { return Age > 0 ? MakeFakeBirthdate(Age) : new DateTime(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="age"></param>
        /// <param name="defaultMonth"></param>
        /// <param name="defaultDay"></param>
        /// <returns></returns>
        public static DateTime MakeFakeBirthdate(int age, int defaultMonth = 1, int defaultDay = 1)
        {
            var birthDay = DateTime.Now.AddYears(-1 * age);
            var year = birthDay.Year;
            birthDay = new DateTime(year, defaultMonth, defaultDay);
            return birthDay;
        }
    }
}
