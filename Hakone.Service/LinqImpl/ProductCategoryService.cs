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
    public class ProductCategoryService : GenericController<ProductCategory, Hakone.Domain.HakoneDBDataContext>, IProductCategoryService
    {
        [Cache.Cacheable("GetProductCats")]
        public List<ProductCategory> GetProductCats()
        {
            return SelectAllAsList();
        }
    }
}
