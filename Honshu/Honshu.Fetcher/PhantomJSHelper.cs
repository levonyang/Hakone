using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
namespace Honshu.Fetcher
{
    public class PhantomJSHelper
    {
        public static Tuple<string, string> GetPageSource(string url, int waitSecond = 0)
        {
            var options = new PhantomJSOptions();
            options.AddAdditionalCapability("phantomjs.page.settings.userAgent", "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36");
            options.AddAdditionalCapability("phantomjs.page.settings.loadImages", false);
            var JsDriver = new PhantomJSDriver(options);
            JsDriver.ExecutePhantomJS("this.onResourceRequested = function(request, net) {" +
                "   if (request.url.indexOf('google-analytics') !== -1 || request.url.indexOf('.css') !==-1) {" +
                "       net.abort();" +
                "   }" +
                "};");
            JsDriver.Navigate().GoToUrl(url);

            JsDriver.WaitUntil(d => !d.Url.Contains("duomai.com") 
                && !d.Url.Contains("guangdiu.com") 
                && !d.Url.Contains("haohuola.com")
                && !d.Url.Contains("zhuayangmao.com")
                && !d.Url.Contains("union.")
                && !d.Url.Contains("click.taobao"));

            var result = JsDriver.PageSource;
            var lastUrl = JsDriver.Url;
            JsDriver.Close();
            JsDriver.Quit();

            return new Tuple<string, string>(result, lastUrl);
        }

        
    }

    public static class PhantomJSHelperExtention
    {
        public static bool WaitUntil(this IWebDriver driver, Func<IWebDriver, bool> expression, int timeOutSeconds = 10)
        {
            TimeSpan timeSpent = new TimeSpan();
            int timeSleepingSpan = 1000;

            bool result = false;
            while (timeSpent.TotalSeconds < timeOutSeconds)
            {
                result = expression.Invoke(driver);

                if (result == true)
                {
                    break;
                }
                Thread.Sleep(timeSleepingSpan);
                timeSpent = timeSpent.Add(new TimeSpan(0, 0, 0, 0, timeSleepingSpan));

            }
            return result;
        }
    }
}
