using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Cube;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Service;

namespace Hakone.Service
{
    public class UserRoleService : GenericController<UserRole, Hakone.Domain.HakoneDBDataContext>, IUserRoleService
    {
        public string GetRoleName(int roldId)
        {
            var role = UserRoleService.SelectAll().FirstOrDefault(r => r.RoleID == roldId);

            return role.IsNull() ? string.Empty : role.RoleName;
        }
    }
}
