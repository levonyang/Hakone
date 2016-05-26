using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;
using Hakone.Service;
using Ninject;

namespace Hakone.Service
{
    public class WebWorkContext : IWorkContext
    {
        private User _cachedUser;

        public Domain.User CurrentUser
        {
            get
            {
                return AuthenticationService.GetAuthenticatedCustomer();
            }
            set
            {
                _cachedUser = value;
            }
        }

        public bool IsAdmin { get; set; }

        public bool IsCurrentUser { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
    }
}
