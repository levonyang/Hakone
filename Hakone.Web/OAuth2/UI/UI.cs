using System;
using System.Collections.Generic;
using System.Text;

namespace Hakone.Web.OAuth
{
    public class UI
    {
        public static string GetHtml()
        {
            string html = "";
            StringBuilder sb = new StringBuilder(html);
            foreach (KeyValuePair<string,OAuth2Base> ob in OAuth2Factory.ServerList)
            {
                if (!string.IsNullOrEmpty(ob.Value.AppKey))
                {
                    sb.Append(string.Format(ob.Value.LoginUrl, string.Format(ob.Value.OAuthUrl, ob.Value.AppKey, System.Web.HttpUtility.UrlEncode(ob.Value.CallbackUrl), ob.Key)));
                }
            }
            return sb.ToString();
        }
    }

}
