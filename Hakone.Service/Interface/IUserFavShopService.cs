using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;

namespace Hakone.Service
{
    public interface IUserFavShopService
    {
        /// <summary>
        /// 根据 value 的值，确定是否添加收藏或者删除
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="shopId"></param>
        /// <param name="ip"></param>
        /// <param name="value"></param>
        void AddOrRemove(int userId, int shopId, string ip, bool value);

        List<UserFavShop> GetListByUser(int userId);
    }
}
