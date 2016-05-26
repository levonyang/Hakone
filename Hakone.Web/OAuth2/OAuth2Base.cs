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
    /// 授权基类
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

        #region 基础属性
        /// <summary>
        /// 返回的开放ID。
        /// </summary>
        public string openID = string.Empty;
        /// <summary>
        /// 访问的Token
        /// </summary>
        public string token = string.Empty;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime expiresTime;

        /// <summary>
        /// 第三方账号昵称
        /// </summary>
        public string nickName = string.Empty;

        /// <summary>
        /// 第三方账号头像地址
        /// </summary>
        public string headUrl = string.Empty;
        /// <summary>
        /// 首次请求时返回的Code
        /// </summary>
        internal string code = string.Empty;
        internal abstract OAuthServer server
        {
            get;
        }
        #endregion

        #region 非公开的请求路径和Logo图片地址。

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

        #region WebConfig对应的配置【AppKey、AppSercet、CallbackUrl】
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

        #region 基础方法

        /// <summary>
        /// 获得Token
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
        /// 获取是否通过授权。
        /// </summary>
        public abstract bool Authorize();
        /// <param name="bindAccount">返回绑定的账号（若未绑定返回空）</param>
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

        #region 关联绑定账号

        /// <summary>
        /// 读取已经绑定的账号
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
        /// 添加绑定账号
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
                    Email = "请在此处输入您的邮件地址",
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
    /// 提供授权的服务商
    /// </summary>
    public enum OAuthServer
    {
        /// <summary>
        /// 新浪微博
        /// </summary>
        SinaWeiBo,
        /// <summary>
        /// 腾讯QQ
        /// </summary>
        QQ,
        /// <summary>
        /// 淘宝网
        /// </summary>
        TaoBao,
        /// <summary>
        /// 人人网（未支持）
        /// </summary>
        RenRen,
        /// <summary>
        /// 腾讯微博（未支持）
        /// </summary>
        QQWeiBo,
        /// <summary>
        /// 开心网（未支持）
        /// </summary>
        KaiXin,
        /// <summary>
        /// 飞信（未支持）
        /// </summary>
        FeiXin,
        /// <summary>
        /// 
        /// </summary>
        None,
    }
}
