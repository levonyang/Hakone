using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;

namespace Hakone.Service
{
    public interface IProductCategoryService
    {
        List<ProductCategory> GetProductCats();
    }
}
