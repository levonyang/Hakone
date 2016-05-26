using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hakone.Web
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class RoleBaseAuthorize : AuthorizeAttribute
    {
        private string[] UserProfilesRequired { get; set; }

        public RoleBaseAuthorize(params object[] userProfilesRequired)
        {
            if (userProfilesRequired.Any(p => p.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("userProfilesRequired");

            this.UserProfilesRequired = userProfilesRequired.Select(p => Enum.GetName(p.GetType(), p)).ToArray();
        }

        public override void OnAuthorization(AuthorizationContext context)
        {
            var authorized = this.UserProfilesRequired.Any(role => HttpContext.Current.User.IsInRole(role));

            if (authorized) return;

            var url = new UrlHelper(context.RequestContext);
            var logonUrl = url.Action("index", "error", new { Id = 500, Area = "" });
            context.Result = new RedirectResult(logonUrl);

            return;
        }
    }
}