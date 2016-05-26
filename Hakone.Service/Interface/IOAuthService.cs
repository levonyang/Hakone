using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;

namespace Hakone.Service
{
    public interface IOAuthService
    {
        OAuth2 GetOAuth2(string openId, string server);
        OAuth2 Update(int id, string token, DateTime expiresTime, string headUrl, string nickName);
        void CreateNew(string server, string token, string openId, DateTime expiresTime, string nickName, string username, string headUrl);
    }
}
