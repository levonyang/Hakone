using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Hakone.Domain;
using Hakone.Service;

namespace Hakone.Service
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly TimeSpan _expirationTimeSpan;
        private User _cachedUser;

        public FormsAuthenticationService(IUserService userService,IUserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _expirationTimeSpan = FormsAuthentication.Timeout;
        }

        public void SignIn(Domain.User user, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            var roles = _userRoleService.GetRoleName(user.RoleID);

            var ticket = new FormsAuthenticationTicket(1, user.UserName, now, now.Add(_expirationTimeSpan),
                createPersistentCookie, roles, FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;

            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            HttpContext.Response.Cookies.Add(cookie);

            _cachedUser = user;
        }

        public void SignOut()
        {
            _cachedUser = null;
            FormsAuthentication.SignOut();
        }

        public Domain.User GetAuthenticatedCustomer()
        {
            if (_cachedUser != null) return _cachedUser;
            if (HttpContext == null || HttpContext.Request == null || !HttpContext.Request.IsAuthenticated)
            {
                return null;
            }
            var user = GetAuthenticatedUser();
            if (user != null && user.IsActivated && !user.IsLocked)
                _cachedUser = user;
            return _cachedUser;
        }

        public bool IsCurrentUser
        {
            get { return GetAuthenticatedCustomer() != null; }
        }

        public virtual User GetAuthenticatedUser()
        {
            HttpCookie authCookie =
                          HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return null;
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var userName = authTicket.Name;

            if (String.IsNullOrWhiteSpace(userName))
                return null;

            var user = _userService.GetUserByUserName(userName);

            return user;
        }

        public HttpContextBase HttpContext
        {
            get { return new HttpContextWrapper(System.Web.HttpContext.Current); }
        }


        public void ClearCacheUser()
        {
            _cachedUser = null;
        }
    }
}
