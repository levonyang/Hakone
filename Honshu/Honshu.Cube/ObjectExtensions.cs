using System;
namespace Honshu.Cube
{
    public static class ObjectExtensions
    {
        public static int TryIntParse(this object input)
        {
            var defaultValue = 0;
            if (input != null)
            {
                int.TryParse(input.ToString().Trim(), out defaultValue);
            }

            return defaultValue;
        }

        public static bool TryBoolParse(this object input)
        {
            var defaultValue = false;
            if (input != null)
            {
                bool.TryParse(input.ToString().Trim(), out defaultValue);
            }

            return defaultValue;
        }

        public static string TryStringParse(this object input)
        {
            if (input != null)
            {
                return input.ToString().Trim();
            }

            return string.Empty;
        }

        public static long TryLongParse(this object input)
        {
            long defaultValue = 0;

            if (input != null)
                long.TryParse(input.ToString().Trim(), out defaultValue);

            return defaultValue;
        }

        public static decimal TryDecimalParse(this object input)
        {
            decimal defaultValue = 0;

            if (input != null)
                decimal.TryParse(input.ToString().Trim(), out defaultValue);

            return defaultValue;
        }
        public static double TryDoubleParse(this object input)
        {
            double defaultValue = 0;

            if (input != null)
                double.TryParse(input.ToString().Trim().TrimEnd('\n').TrimEnd('\r'), out defaultValue);

            return defaultValue;
        }

        public static DateTime TryDateTimeParse(this object input)
        {
            DateTime defaultValue = DateTime.MinValue;

            if (input != null)
                DateTime.TryParse(input.ToString().Trim().TrimEnd('\n').TrimEnd('\r'), out defaultValue);

            return defaultValue;
        }


        public static string RemoveDash(this object input)
        {
            if (input != null)
                return input.ToString().Replace("-", "");
            return string.Empty;
        }

    }
}