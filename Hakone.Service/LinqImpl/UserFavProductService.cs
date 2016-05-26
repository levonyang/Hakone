using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Service;
using Hakone.Cube;
using CacheAspect.Attributes;
using CacheAspect;

namespace Hakone.Service
{
    public class UserFavProductService : GenericController<UserFavProduct, Hakone.Domain.HakoneDBDataContext>, IUserFavProductService
    {
        [Cache.TriggerInvalidation(CacheKey.GetUserFavProductListByUser, CacheSettings.UseProperty, "userId")]
        public void AddOrRemove(int userId, int productId, string ip, bool value)
        {
            if (!value)
            {
                Remove(userId, productId);
                return;
            }

            if (ExistsAlready(userId, productId)) return;

            var entity = new UserFavProduct
            {
                UserID = userId,
                ProductID = productId,
                EntryDate = DateTime.Now.ToTimeStamp()
            };

            Insert(entity, true);
        }

        private bool ExistsAlready(int userId, int productId)
        {
            return SelectAll().FirstOrDefault(r => r.UserID == userId && r.ProductID == productId) != null;
        }

        public void Remove(int userId, int productId)
        {
            var entity = SelectAll().FirstOrDefault(r => r.UserID == userId && r.ProductID == productId);
            if (entity == null) return;

            Delete(entity, true);
        }

        [Cache.Cacheable(CacheKey.GetUserFavProductListByUser, CacheSettings.UseProperty, "userId")]
        public List<UserFavProduct> GetListByUser(int userId)
        {
            return SelectAll().Where(r => r.UserID == userId).ToList();
        }
    }
}
