using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheAspect.Attributes;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Service;

namespace Hakone.Service
{
    public class ShopCategoryService : GenericController<ShopCategory, Hakone.Domain.HakoneDBDataContext>, IShopCategoryService
    {
        [Cache.Cacheable("GetShopCats")]
        public List<ShopCategory> GetShopCats()
        {
            return SelectAllAsList();
        }
    }
}
