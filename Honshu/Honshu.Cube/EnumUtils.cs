using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Honshu.Cube
{
    /*
    C# Generic Enum Parser With Extension Methods
    Code Snippet By: Pinal Bhatt [www.PBDesk.com]
    */

    public static class EnumUtils
    {
        #region String to Enum
        public static T ParseEnum<T>(string inString, bool ignoreCase = true, bool throwException = true) where T : struct
        {
            return (T)ParseEnum<T>(inString, default(T), ignoreCase, throwException);
        }

        public static T ParseEnum<T>(string inString, T defaultValue,
                               bool ignoreCase = true, bool throwException = false) where T : struct
        {
            T returnEnum = defaultValue;

            if (!typeof(T).IsEnum || String.IsNullOrEmpty(inString))
            {
                throw new InvalidOperationException("Invalid Enum Type or Input String 'inString'. " + typeof(T).ToString() + "  must be an Enum");
            }

            try
            {
                bool success = Enum.TryParse<T>(inString, ignoreCase, out returnEnum);
                if (!success && throwException)
                {
                    throw new InvalidOperationException("Invalid Cast");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Invalid Cast", ex);
            }

            return returnEnum;
        }
        #endregion

        #region Int to Enum
        public static T ParseEnum<T>(int input, bool throwException = true) where T : struct
        {
            return (T)ParseEnum<T>(input, default(T), throwException);
        }
        public static T ParseEnum<T>(int input, T defaultValue, bool throwException = false) where T : struct
        {
            T returnEnum = defaultValue;
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Invalid Enum Type. " + typeof(T).ToString() + "  must be an Enum");
            }
            if (Enum.IsDefined(typeof(T), input))
            {
                returnEnum = (T)Enum.ToObject(typeof(T), input);
            }
            else
            {
                if (throwException)
                {
                    throw new InvalidOperationException("Invalid Cast");
                }
            }

            return returnEnum;

        }
        #endregion

        #region String Extension Methods for Enum Parsing
        public static T ToEnum<T>(this string inString, bool ignoreCase = true, bool throwException = true) where T : struct
        {
            return (T)ParseEnum<T>(inString, ignoreCase, throwException);
        }
        public static T ToEnum<T>(this string inString, T defaultValue, bool ignoreCase = true, bool throwException = false) where T : struct
        {
            return (T)ParseEnum<T>(inString, defaultValue, ignoreCase, throwException);
        }
        public static int ToInt(this Enum enumValue)
        {
            return (int)((object)enumValue);
        }
        public static string GetDescription<TEnum>(this TEnum EnumValue) where TEnum : struct
        {
            return EnumUtils.GetEnumDescription((Enum)(object)((TEnum)EnumValue));
        }
        #endregion

        #region Int Extension Methods for Enum Parsing
        public static T ToEnum<T>(this int input, bool throwException = true) where T : struct
        {
            return (T)ParseEnum<T>(input, default(T), throwException);
        }

        public static T ToEnum<T>(this int input, T defaultValue, bool throwException = false) where T : struct
        {
            return (T)ParseEnum<T>(input, defaultValue, throwException);
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
        #endregion

        #region Helper
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        #endregion
    }

    class ExampleProgram
    {
        static void Main(string[] args)
        {
            int i = 7;
            Console.WriteLine(EnumUtils.ParseEnum<Colors>(i));
            string s = "indigo";
            Console.WriteLine(EnumUtils.ParseEnum<Colors>(s));
            s = "blue";
            Console.WriteLine(s.ToEnum<Colors>(true));
            i = 4;
            Console.WriteLine(i.ToEnum<Colors>());
        }

        public enum Colors
        {
            Red = 1,
            Orange = 2,
            Yellow = 3,
            Green = 4,
            Blue = 5,
            Indigo = 6,
            Violet = 7
        }
    }
}
