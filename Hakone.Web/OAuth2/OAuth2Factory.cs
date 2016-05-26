using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace Hakone.Web.OAuth
{
    /// <summary>
    /// 授权工厂类
    /// </summary>
    public class OAuth2Factory
    {
        /// <summary>
        /// 获取当前的授权类型。
        /// </summary>
        public static OAuth2Base Current
        {
            get
            {
                if (System.Web.HttpContext.Current == null) return null;

                var url = System.Web.HttpContext.Current.Request.Url.Query;
                if (url.IndexOf("state=", StringComparison.Ordinal) <= -1) return null;

                var code = Tool.QueryString(url, "code");
                var state = Tool.QueryString(url, "state");
                if (!ServerList.ContainsKey(state)) return null;

                var ob = ServerList[state];
                ob.code = code;
                System.Web.HttpContext.Current.Session["OAuth2"] = ob;//对象存进Session，后期授权后会增加引用。
                return ob;
            }
        }
        /// <summary>
        /// 读取或设置当前Session存档的授权类型。 (注销用户时可以将此值置为Null)
        /// </summary>
        public static OAuth2Base SessionOAuth
        {
            get
            {
                if (System.Web.HttpContext.Current.Session == null) return null;

                var o = System.Web.HttpContext.Current.Session["OAuth2"];
                return o as OAuth2Base;
            }
            set
            {
                System.Web.HttpContext.Current.Session["OAuth2"] = value;
            }
        }

        private static Dictionary<string, OAuth2Base> _serverList;
        /// <summary>
        /// 获取所有的类型（新开发的OAuth2需要到这里注册添加一下）
        /// </summary>
        internal static Dictionary<string, OAuth2Base> ServerList
        {
            get
            {
                return _serverList ??
                       (_serverList = new Dictionary<string, OAuth2Base>(StringComparer.OrdinalIgnoreCase)
                       {
                           {OAuthServer.TaoBao.ToString(), new TaoBaoAuth()},
                           {OAuthServer.SinaWeiBo.ToString(), new SinaWeiBoOAuth()},
                           {OAuthServer.QQ.ToString(), new QQOAuth()}
                       });
            }
        }
    }
}
