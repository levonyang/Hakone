using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Reflection;
using System.Web;
using Hakone.Cube;
using Hakone.Service;
using Hakone.Domain;
using log4net;
using Ninject;

namespace Hakone.Web.OAuth
{
    /// <summary>
    /// ��Ȩ����
    /// </summary>
    public abstract class OAuth2Base
    {
        protected WebClient wc = new WebClient();
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected OAuth2Base()
        {
            wc = new WebClient {Encoding = Encoding.UTF8};
            wc.Headers.Add("Pragma", "no-cache");
            OAuthService = new OAuthService();
            UserService = new UserService();
        }

        #region ��������
        /// <summary>
        /// ���صĿ���ID��
        /// </summary>
        public string openID = string.Empty;
        /// <summary>
        /// ���ʵ�Token
        /// </summary>
        public string token = string.Empty;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime expiresTime;

        /// <summary>
        /// �������˺��ǳ�
        /// </summary>
        public string nickName = string.Empty;

        /// <summary>
        /// �������˺�ͷ���ַ
        /// </summary>
        public string headUrl = string.Empty;
        /// <summary>
        /// �״�����ʱ���ص�Code
        /// </summary>
        internal string code = string.Empty;
        internal abstract OAuthServer server
        {
            get;
        }
        #endregion

        #region �ǹ���������·����LogoͼƬ��ַ��

        internal abstract string OAuthUrl
        {
            get;
        }
        internal abstract string TokenUrl
        {
            get;
        }
        internal abstract string LoginUrl
        {
            get;
        }
        #endregion

        #region WebConfig��Ӧ�����á�AppKey��AppSercet��CallbackUrl��
        internal string AppKey
        {
            get
            {
                return Tool.GetConfig(server.ToString() + ".AppKey");
            }
        }
        internal string AppSercet
        {
            get
            {
                return Tool.GetConfig(server.ToString() + ".AppSercet");
            }
        }
        internal string CallbackUrl
        {
            get
            {
                return Tool.GetConfig(server.ToString() + ".CallbackUrl");
            }
        }
        #endregion

        #region ��������

        /// <summary>
        /// ���Token
        /// </summary>
        /// <returns></returns>
        protected string GetToken(string method)
        {
            string result = string.Empty;
            try
            {
                string para = "grant_type=authorization_code&client_id=" + AppKey + "&client_secret=" + AppSercet + "&code=" + code + "&state=" + server;
                para += "&redirect_uri=" + System.Web.HttpUtility.UrlEncode(CallbackUrl) + "&rnd=" + DateTime.Now.Second;
                if (method == "POST")
                {
                    if (string.IsNullOrEmpty(wc.Headers["Content-Type"]))
                    {
                        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    }
                    result = wc.UploadString(TokenUrl, method, para);
                }
                else
                {
                    result = wc.DownloadString(TokenUrl + "?" + para);
                }
            }
            catch (Exception err)
            {
                //CYQ.Data.Log.WriteLogToTxt(err);
            }
            return result;
        }
        /// <summary>
        /// ��ȡ�Ƿ�ͨ����Ȩ��
        /// </summary>
        public abstract bool Authorize();
        /// <param name="bindAccount">���ذ󶨵��˺ţ���δ�󶨷��ؿգ�</param>
        public bool Authorize(out string bindAccount)
        {
            bindAccount = string.Empty;
            if (Authorize())
            {
                bindAccount = GetBindAccount();
                return true;
            }
            return false;
        }

        #endregion

        #region �������˺�

        /// <summary>
        /// ��ȡ�Ѿ��󶨵��˺�
        /// </summary>
        /// <returns></returns>
        public string GetBindAccount()
        {
            var account = string.Empty;
            if (OAuthService == null) _log.Debug("9999999999999999");
            var oa = OAuthService.GetOAuth2(openID, server.ToString());
            if (oa == null) return account;

            var updateedAuth = OAuthService.Update(oa.ID, token, expiresTime, headUrl, nickName);
            account = updateedAuth.BindAccount;
            return account;
        }
        /// <summary>
        /// ��Ӱ��˺�
        /// </summary>
        /// <param name="bindAccount"></param>
        /// <returns></returns>
        public bool SetBindAccount(string bindAccount)
        {
            if (!string.IsNullOrEmpty(openID) && !string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(bindAccount))
            {
                var oa = OAuthService.GetOAuth2(openID, server.ToString());

                var username = string.Format("{0}#{1}", bindAccount, server.ToString());

                if (oa == null)
                {
                    OAuthService.CreateNew(server.ToString(), token, openID, expiresTime, nickName, username, headUrl);
                }

                var user = UserService.GetUserByUserNameNoCache(bindAccount);
                if (!user.IsNull()) return true;

                //add a record to user table:
                user = new User
                {
                    BindAccount = username,
                    UserName = bindAccount,
                    Password = string.Empty,
                    Email = "���ڴ˴����������ʼ���ַ",
                    LastActiveIP = Tool.GetIP()
                };
                UserService.RegisterUser(user);
                return true;
            }
            return false;
        }
        #endregion


        private IOAuthService OAuthService { get; set; }


        private IUserService UserService { get; set; }
    }
    /// <summary>
    /// �ṩ��Ȩ�ķ�����
    /// </summary>
    public enum OAuthServer
    {
        /// <summary>
        /// ����΢��
        /// </summary>
        SinaWeiBo,
        /// <summary>
        /// ��ѶQQ
        /// </summary>
        QQ,
        /// <summary>
        /// �Ա���
        /// </summary>
        TaoBao,
        /// <summary>
        /// ��������δ֧�֣�
        /// </summary>
        RenRen,
        /// <summary>
        /// ��Ѷ΢����δ֧�֣�
        /// </summary>
        QQWeiBo,
        /// <summary>
        /// ��������δ֧�֣�
        /// </summary>
        KaiXin,
        /// <summary>
        /// ���ţ�δ֧�֣�
        /// </summary>
        FeiXin,
        /// <summary>
        /// 
        /// </summary>
        None,
    }
}
