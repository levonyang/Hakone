using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Service;

namespace Hakone.Service
{
    public class OAuthService : GenericController<OAuth2, Hakone.Domain.HakoneDBDataContext>, IOAuthService
    {
        public OAuth2 GetOAuth2(string openId, string server)
        {
            return SelectAll().FirstOrDefault(o => o.OpenID == openId && o.OAuthServer == server);
        }

        public OAuth2 Update(int id, string token, DateTime expiresTime, string headUrl, string nickName)
        {
            var auth = SelectAll().FirstOrDefault(r => r.ID == id);
            auth.Token = token;
            //auth.NickName = HttpUtility.UrlDecode(nickName);
            auth.ExpireTime = expiresTime;
            auth.HeadUrl = headUrl;

            Update(auth, true);

            return auth;
        }

        public void CreateNew(string server, string token, string openId, DateTime expiresTime, string nickName, string username, string headUrl)
        {
            var auth = new OAuth2();

            auth.OAuthServer = server;
            auth.Token = token;
            auth.OpenID = openId;
            auth.ExpireTime = expiresTime;
            auth.NickName = HttpUtility.UrlDecode(nickName);
            auth.BindAccount = username;
            auth.HeadUrl = headUrl;

            Insert(auth, true);
        }
    }
}
