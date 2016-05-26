using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Service
{
    public class CacheKey
    {
        public const string GetLatestSelectedShops = "ShopWithProductService.GetLatestSelectedShops";
        public const string GetLatestRecommendShops = "ShopWithProductService.GetLatestRecommendShops";
        public const string GetShopList = "ShopWithProductService.GetList";

        public const string GetTopProducts = "ProductService.GetTopProducts";

        public const string GetUserFavProductListByUser = "UserFavProductService.GetUserFavProductListByUser";
        public const string GetUserFavShopListByUser = "UserFavShopService.GetUserFavShopListByUser";

        public const string GetUserByUserName = "UserService.GetUserByUserName";
    }
}
