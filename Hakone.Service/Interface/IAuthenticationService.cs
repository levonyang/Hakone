using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;

namespace Hakone.Service
{
    public interface IAuthenticationService
    {
        void SignIn(User user, bool createPersistentCookie);
        void SignOut();
        User GetAuthenticatedCustomer();

        void ClearCacheUser();

        bool IsCurrentUser { get; }
    }
}
