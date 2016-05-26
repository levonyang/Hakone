using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace Hakone.Web.OAuth
{
    /// <summary>
    /// ��Ȩ������
    /// </summary>
    public class OAuth2Factory
    {
        /// <summary>
        /// ��ȡ��ǰ����Ȩ���͡�
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
                System.Web.HttpContext.Current.Session["OAuth2"] = ob;//������Session��������Ȩ����������á�
                return ob;
            }
        }
        /// <summary>
        /// ��ȡ�����õ�ǰSession�浵����Ȩ���͡� (ע���û�ʱ���Խ���ֵ��ΪNull)
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
        /// ��ȡ���е����ͣ��¿�����OAuth2��Ҫ������ע�����һ�£�
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
