using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;
using MvcPaging;

namespace Hakone.Service
{
    public interface IShopService
    {
        IPagedList<Shop> GetList(int page, int r, string orderby, string s, int pagesize = 100);
        Shop GetShop(int shopId);
        Shop GetShopByShopIndex(int shopIndex);
        Shop GetShopByShopName(string shopName);

        Shop CreateOrUpdate(Shop shop);

        void CollectShop(int shopId, bool value);
        void RecommendShop(int shopId, bool value);
        void HotShop(int shopId, bool value);
        void SelectedShop(int shopId, bool value);
        IPagedList<ShopES> GetUserFavs(int userId, int page);

        void Delete(int shopId);

        void UpdateFetchDate(int shopId);
        void UpdateAsyncDate(int shopId);

        //for elasticsearch:
        List<ShopES> GetLatestSelectedShops();
        List<ShopES> GetLatestRecommendShops();
        IPagedList<ShopES> GetPagedList(string listFor, int? catId, string keyword, int page, string orderby = "", string city = "", int pagesize = 100);

    }
}
;