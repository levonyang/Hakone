using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Honshu.Cube
{
    public static class StringExtensions
    {
        public static string Format(this string input, object obj)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(obj))
                input = input.Replace("{" + prop.Name + "}", (prop.GetValue(obj) ?? "(null)").ToString());

            return input;
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
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string RemoveRMBSymbol(this string input)
        {
            return input.Replace("￥", "").Replace("&yen;", "").Replace("&#165", "").Replace("&#65509;","");
        }

        public static string RemoveAdjProductName(this string productName)
        {
            string adjWords = "春装;夏装;秋装;春夏装;春秋装;秋冬装;春夏款;春秋款;秋冬款;春夏;春秋;秋冬;爆款;热款;2014;2015;2016;2017";
            var arrWords = adjWords.Split(';');
            foreach (var key in arrWords)
            {
                productName = productName.Replace(key, string.Empty);
            }

            return productName;
        }

        public static string GetUrlParamValue(this string url, string key)
        {
            Uri myUri = new Uri(url);
            return HttpUtility.ParseQueryString(myUri.Query).Get(key);
        }

        public static string RemoveParammeters(this string url)
        {
            var urlSplit = url.Split('?');
            return urlSplit[0];
        }

        public static string RemoveOneParameters(this string url, string key)
        {
            url = url.Replace("&amp;", "&");

            var paramValue = url.GetUrlParamValue(key);
            if (!paramValue.IsNullOrEmpty())
            {
                url = url.Replace(string.Format("&{0}={1}", key, paramValue), "");
                url = url.Replace(string.Format("{0}={1}", key, paramValue), "");
            }

            return url;
        }

        public static string RemoveSpecialParameter(this string url)
        {
            return url.RemoveOneParameters("t").RemoveOneParameters("tag");
        }

        public static string RemoveSomeParameters(this string url)
        {
            if (url.IsNullOrEmpty()) return url;

            if ((url.Contains("amazon.cn") && !url.Contains("node="))
                || url.Contains("jd.com")
                || url.Contains("yhd.com")
                || url.Contains("item.yixun.com")
                || url.Contains("item.jd.hk")
                || url.Contains("item.yohobuy.com")
                || url.Contains("j1.com/product")
                || url.Contains("suning.com/product")
                || url.Contains("suning.com")
                || url.Contains("womai.com/product")
                || url.Contains("product.suning.com")
                || url.Contains("product.suning.com")
                || url.Contains("accorhotels.com")
            )
            {
                return url.RemoveParammeters();
            }
            return url.RemoveSpecialParameter();
        }

        public static string AddHttpForUrl(this string url)
        {
            if (url.IsNullOrEmpty()) return url;

            if (url.StartsWith("//"))
            {
                return string.Format("http:{0}", url);
            }

            return url;
        }

        public static string YiqiUrlHandler(this string url)
        {
            if (url.Contains("click.mz.simba.taobao.com") || url.Contains("click.tanx.com"))
            {
                return url.GetUrlParamValue("u").UrlDecode();
            }

            if (url.Contains("ju.taobao.com")
            || url.Contains("item.htm")
            || url.Contains("subject.tmall.com"))
            {
                return url;
            }


            return url.RemoveParammeters();
        }

        public static string HtmlDecode(this string input)
        {
            return HttpUtility.HtmlDecode(input);
        }

        public static string HtmlEncode(this string input)
        {
            return HttpUtility.HtmlEncode(input);
        }

        public static string UrlDecode(this string input)
        {
            return HttpUtility.UrlDecode(input);
        }

        public static string UrlEncode(this string input)
        {
            return HttpUtility.UrlEncode(input);
        }

        public static string RemoveImgSize(this string imageUrl, int size)
        {
            string replaceText = string.Format("_{0}x{0}.jpg", size);
            return imageUrl.Replace(replaceText, string.Empty);
        }

        public static string StringStrip(this string input)
        {
            return input.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static string ReplaceTextarea(this string input)
        {
            return input.Replace("<textarea class=\"data-lazyload\">", "").Replace("</textarea>", "");
        }

        public static string GetImageBySize(this string imageUrl, int size)
        {
            imageUrl = imageUrl.Replace("_b.jpg", "");
            if (Regex.IsMatch(imageUrl, @"_\d+?x\d+?\.jpg"))
            {
                return Regex.Replace(imageUrl, @"_\d+?x\d+?\.jpg", string.Format("_{0}x{0}.jpg", size));
            }
            return imageUrl + string.Format("_{0}x{0}.jpg", size);
        }

        public static string RemoveJSWrapper(this string js)
        {
            return js.Replace("<script type='text/javascript'>", "").Replace("</script>", "");
        }

        public static string GetTaobaoUrl(this long itemId)
        {
            return string.Format("http://item.taobao.com/item.htm?id={0}", itemId);
        }
        public static string GetTaobaoUrl(this string itemId)
        {
            return string.Format("http://item.taobao.com/item.htm?id={0}", itemId);
        }

        public static bool IsNormalProductName(this string productName)
        {
            var arrStr = "补邮,专拍,补拍,差价,补运费;专用链接".Split(',');
            return arrStr.All(str => !productName.Contains(str));
        }

    }
}
