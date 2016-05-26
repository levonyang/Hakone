using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Hakone.Cube;
using Hakone.Service;
using log4net;
using log4net.Config;
using Newtonsoft.Json;

namespace Hakone.Web.OAuth
{
    public class SinaWeiBoOAuth : OAuth2Base
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal override OAuthServer server
        {
            get
            {
                return OAuthServer.SinaWeiBo;
            }
        }
        internal override string LoginUrl
        {
            get
            {
                return "<a href='{0}'><i class='iconfont weibo'>&#xe617;</i>ʹ��΢���˺ŵ�¼</a>";
            }
        }
        internal override string OAuthUrl
        {
            get
            {
                return "https://api.weibo.com/oauth2/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}";
            }
        }
        internal override string TokenUrl
        {
            get
            {
                return "https://api.weibo.com/oauth2/access_token";
            }
        }
        internal string UserInfoUrl = "https://api.weibo.com/2/users/show.json?access_token={0}&uid={1}";
        public override bool Authorize()
        {
            XmlConfigurator.Configure();
            _log.Debug("�����е���������������������������������������");

            if (!string.IsNullOrEmpty(code))
            {
                var result = GetToken("POST");//һ���Է������ݡ�
                //�ֽ�result;
                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        var myObject = JsonConvert.DeserializeObject<dynamic>(result);
                        token = myObject.access_token;
                        openID = myObject.uid;
                        _log.Debug("1111111111111111");
                        expiresTime = DateTime.Now.AddSeconds(Convert.ToDouble(myObject.expires_in));

                        _log.Debug("222222222222222");
                        if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(openID))
                        {
                            _log.Debug("333333333333333");
                            //��ȡ΢���ǳƺ�ͷ��
                            result = wc.DownloadString(string.Format(UserInfoUrl, token, openID));
                            if (!string.IsNullOrEmpty(result)) //���أ�callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} ); 
                            {
                                nickName = Tool.GetJosnValue(result, "screen_name");
                                headUrl = Tool.GetJosnValue(result, "profile_image_url");
                                _log.Debug("4444444444444444444");
                                return true;
                            }
                        }
                        else
                        {
                            _log.Debug("SinaWeiBoOAuth.Authorize():" + result);
                        }

                    }
                    catch (Exception err)
                    {
                        _log.Debug(err);
                    }
                }
            }
            _log.Debug("666666666666666666666666");
            return false;
        }
    }
}
