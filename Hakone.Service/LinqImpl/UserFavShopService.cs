using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Cube;
using Hakone.Service;
using CacheAspect.Attributes;
using CacheAspect;

namespace Hakone.Service
{
    public class UserFavShopService : GenericController<UserFavShop, Hakone.Domain.HakoneDBDataContext>, IUserFavShopService
    {
        [Cache.TriggerInvalidation(CacheKey.GetUserFavShopListByUser, CacheSettings.UseProperty, "userId")]
        public void AddOrRemove(int userId, int shopId, string ip, bool value)
        {
            if (!value)
            {
                Remove(userId, shopId);
                return;
            }

            if (ExistsAlready(userId, shopId)) return;

            var entity = new UserFavShop
            {
                UserID = userId,
                ShopID = shopId,
                EntryDate = DateTime.Now.ToTimeStamp()
            };

            Insert(entity, true);
        }

        private bool ExistsAlready(int userId, int shopId)
        {
            return SelectAll().FirstOrDefault(r => r.UserID == userId && r.ShopID == shopId) != null;
        }

        public void Remove(int userId, int shopId)
        {
            var entity = SelectAll().FirstOrDefault(r => r.UserID == userId && r.ShopID == shopId);
            if (entity == null) return;

            Delete(entity,true);
        }

        [Cache.Cacheable(CacheKey.GetUserFavShopListByUser, CacheSettings.UseProperty, "userId")]
        public List<UserFavShop> GetListByUser(int userId)
        {
            return SelectAll().Where(r => r.UserID == userId).ToList();
        }
    }
}
