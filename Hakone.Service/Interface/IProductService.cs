using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;
using MvcPaging;

namespace Hakone.Service
{
    public interface IProductService
    {
        Product GetProduct(int productId);
        List<Product> GetTopProducts(int shopIndex);
        IPagedList<Product> GetProductsByShop(int shopId, int page, string orderby);

        IPagedList<Product> GetUserFavs(int userId, int page);
        IPagedList<Product> GetList(int catId, string s, int r, int page);

        void Recommend(Product product);

        void Delete(int productId);

        /// <summary>
        /// 返回ShopId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int Goup(int productId);

        void CollectProduct(int productId, bool value);

        List<Product> LatestRecommendProducts();
    }
}
