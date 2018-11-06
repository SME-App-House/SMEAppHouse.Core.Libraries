using System;
using System.Globalization;

namespace SMEAppHouse.Core.CodeKits.Extensions
{
    public static class LongExt
    {
        private static readonly long[] NumberOfBytesInUnit;
        private static readonly Func<long, string>[] BytesToUnitConverters;

        static LongExt()
        {
            NumberOfBytesInUnit = new[]    
        {
            1L << 10,    // Bytes in a Kibibyte
            1L << 20,    // Bytes in a Mebibyte
            1L << 30,    // Bytes in a Gibibyte
            1L << 40,    // Bytes in a Tebibyte
            1L << 50,    // Bytes in a Pebibyte
            1L << 60     // Bytes in a Exbibyte
        };

            // Shift the long (integer) down to 1024 times its number of units, convert to a double (real number), 
            // then divide to get the final number of units (units will be in the range 1 to 1023.999)
            Func<long, int, string> formatAsProportionOfUnit = (bytes, shift) => (((double)(bytes >> shift)) / 1024).ToString("0.###");

            BytesToUnitConverters = new Func<long, string>[]
        {
            bytes => bytes.ToString(CultureInfo.InvariantCulture) + " B",
            bytes => formatAsProportionOfUnit(bytes, 0) + " KiB",
            bytes => formatAsProportionOfUnit(bytes, 10) + " MiB",
            bytes => formatAsProportionOfUnit(bytes, 20) + " GiB",
            bytes => formatAsProportionOfUnit(bytes, 30) + " TiB",
            bytes => formatAsProportionOfUnit(bytes, 40) + " PiB",
            bytes => formatAsProportionOfUnit(bytes, 50) + " EiB"
        };
        }

        public static string ToReadableByteSizeString(this long bytes)
        {
            if (bytes < 0)
                return "-" + Math.Abs(bytes).ToReadableByteSizeString();

            var counter = 0;
            while (counter < NumberOfBytesInUnit.Length)
            {
                if (bytes < NumberOfBytesInUnit[counter])
                    return BytesToUnitConverters[counter](bytes);
                counter++;
            }
            return BytesToUnitConverters[counter](bytes);
        }
    }
}
