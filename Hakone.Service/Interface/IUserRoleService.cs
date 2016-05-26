using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Service
{
    public interface IUserRoleService
    {
        string GetRoleName(int roleId);
    }
}
