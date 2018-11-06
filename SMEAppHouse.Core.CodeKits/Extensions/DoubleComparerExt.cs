using System;

namespace SMEAppHouse.Core.CodeKits.Extensions
{
    public static class DoubleComparerExt
    {
        /// <summary>
        /// http://stackoverflow.com/questions/3420812/how-do-i-find-if-two-variables-are-approximately-equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="acceptableDifference">Example and usually the value is: 0.001 </param>
        /// <returns></returns>
        public static bool AlmostEquals(this double left, double right, double acceptableDifference= 0.001)
        {
            var leftAsBits = left.ToBits2Complement();
            var rightAsBits = right.ToBits2Complement();
            var floatingPointRepresentationsDiff = Math.Abs(leftAsBits - rightAsBits);
            return (floatingPointRepresentationsDiff <= acceptableDifference);
        }

        public static bool ApproximatelyEquals(double left, double right, double acceptableDifference)
        {
            return left.AlmostEquals(right, acceptableDifference);
        }

        private static unsafe long ToBits2Complement(this double value)
        {
            var valueAsDoublePtr = &value;
            var valueAsLongPtr = (long*)valueAsDoublePtr;
            var valueAsLong = *valueAsLongPtr;
            return valueAsLong < 0
                ? (long)(0x8000000000000000 - (ulong)valueAsLong)
                : valueAsLong;
        }
    }
}
