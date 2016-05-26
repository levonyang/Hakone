using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;

namespace Hakone.Service
{
    public interface IUserFavProductService
    {
        void AddOrRemove(int userId, int productId, string ip, bool value);

        List<UserFavProduct> GetListByUser(int userId);
    }
}
