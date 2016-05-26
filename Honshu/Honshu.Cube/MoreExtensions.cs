using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honshu.Cube
{
    public static class MoreExtensions
    {
        public static DateTime DateConvert(this string input)
        {
            var date = DateTime.Now;
            int interval = 0;

            input = input.Replace("前", "");

            if (input.Contains("秒"))
            {
                interval = input.Replace("秒钟", "").Replace("秒", "").Trim().TryIntParse();
                date = date.AddSeconds(-interval);
            }
            else if (input.Contains("分钟"))
            {
                interval = input.Replace("分钟", "").Trim().TryIntParse();
                date = date.AddMinutes(-interval);
            }
            else if (input.Contains("小时"))
            {
                interval = input.Replace("小时", "").Trim().TryIntParse();
                date = date.AddHours(-interval);
            }
            else if (input.Contains("昨天"))
            {
                date = date.AddDays(-1);
                if (input.Contains(":"))
                {
                    input = input.Replace("昨天", "").Trim();
                    var times = input.Split(":");
                    date = new DateTime(date.Year, date.Month, date.Day, times[0].TryIntParse(), times[1].TryIntParse(), 0);
                }
                
            }
            else if (input.Contains("天"))
            {
                interval = input.Replace("天", "").Trim().TryIntParse();
                date = date.AddDays(-interval);
            }
            else if (input.Contains("月"))
            {
                interval = input.Replace("个月", "").Replace("月", "").Trim().TryIntParse();
                date = date.AddMonths(-interval);
            }
            else if (input.Contains("年"))
            {
                interval = input.Replace("年", "").Trim().TryIntParse();
                date = date.AddYears(-interval);
            }
            else
            {
                if (!input.StartsWith("20"))
                {
                    input = date.Year.ToString() + "-" + input;
                }
                date = input.TryDateTimeParse();
            }

            return date;
        }
        public static string DateConvert(this DateTime inputDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.Now.Ticks - inputDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "刚刚" : ts.Seconds + "秒钟前";
            }
            if (delta < 2 * MINUTE)
            {
                return "1分钟前";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + "分钟前";
            }
            if (delta < 90 * MINUTE)
            {
                return "1小时前";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + "小时前";
            }
            if (delta < 48 * HOUR)
            {
                return "昨天";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + "天前";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "一个月前" : months + "个月前";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "一年前" : years + "年前";
            }
        }
    }
}
