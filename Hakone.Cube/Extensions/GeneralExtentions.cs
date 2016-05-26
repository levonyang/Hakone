using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hakone.Cube
{
    public static partial  class GeneralExtentions
    {
        public static string FormatString(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string Format(this string input, object p)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(p))
                input = input.Replace("{" + prop.Name + "}", (prop.GetValue(p) ?? "(null)").ToString());
            return input;
        }

        public static bool Match(this string value, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(value);
        }

        public static string GetMatchContent(this string content, string regexExpression)
        {
            var result = string.Empty;

            var regContent = new Regex(regexExpression, RegexOptions.Compiled);
            var matchContent = regContent.Match(content);
            if (matchContent.Success)
            {
                result = matchContent.Value;
            }

            return result;
        }

        public static string GetMatchContent(this string content, string regexStart, string regexEnd)
        {
            var result = string.Empty;

            var regContent = new Regex(string.Format("(?<={0})(.|\n)+?(?={1})", regexStart, regexEnd),
                RegexOptions.Compiled);
            var matchContent = regContent.Match(content);
            if (matchContent.Success)
            {
                result = matchContent.Value.Trim().Trim(',');
            }

            return result;
        }

        public static string RemoveHTML(this string str)
        {
            if (!string.IsNullOrEmpty(str))
                return Regex.Replace(str, "<[^>]*>", "").Trim();
            else
                return null;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + " ...";
        }

        public static string RemoveRMBSymbol(this string input)
        {
            return input.Replace("￥", "").Replace("&yen;", "").Replace("&#165", "").Replace("&#65509;", "");
        }

        public static string RemoveAdjProductName(this string productName)
        {
            string adjWords = "新款;春季;夏季;秋季;冬季;包邮;春装;夏装;秋装;春夏装;春秋装;秋冬装;春夏款;春秋款;秋冬款;春夏;春秋;秋冬;爆款;热款;2014;2015;2016;2017";
            var arrWords = adjWords.Split(';');
            foreach (var key in arrWords)
            {
                productName = productName.Replace(key, string.Empty);
            }

            return productName;
        }

        public static string GetPhotoBySize(this string input, int size)
        {
            string resut = input.Replace("_b.jpg", string.Format("_{0}x{0}.jpg", size));
            if (resut.IndexOf("_b") > -1)
                resut = input.Replace("_b", string.Format("_{0}x{0}", size));
            resut = Regex.Replace(resut, @"\d+?x\d+\.", string.Format("{0}x{0}.", size.ToString()));
            return resut;
        }

        public static string GetPhotoReplaceSize(this string input, int size)
        {
            return Regex.Replace(input, @"\d+?x\d+\.", string.Format("{0}x{0}.", size.ToString()));
        }

        public static void Raise(this EventHandler eventHandler,
            object sender, EventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static void Raise<T>(this EventHandler<T> eventHandler,
            object sender, T e)
           where T : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static int ToTimeStamp(this DateTime date)
        {
            return Convert.ToInt32(date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }
        public static DateTime ToDateTime(this int timeStamp)
        {
            return (new DateTime(1970, 1, 1)).AddSeconds(Convert.ToInt32(timeStamp));
        }

        public static IEnumerable<T> OrderByRandom<T>(this IEnumerable<T> source)
        {
            var rnd = new Random();
            return source.OrderBy(x => rnd.Next());
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int n)
        {
            return source.OrderByRandom().Take(n);
        }
    }
}
