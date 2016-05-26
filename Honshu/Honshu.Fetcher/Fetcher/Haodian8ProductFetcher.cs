using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honshu.Data;
using Honshu.DataAccess;

namespace Honshu.Fetcher
{
    public class Haodian8ProductFetcher
    {
        public Tuple<string,int> Fetch()
        {
            var result = new Tuple<string, int>(string.Empty, 0);
            //获取需要更新商品的店铺：
            var shop = ShopDataAccess.GetTopShop();
            if (shop.IsNull()) return new Tuple<string, int>(string.Empty, 0);

            var fetcher = FetcherFactory.CreateFetcher(shop.ShopUrl);
            var products = fetcher.GetProducts(shop.ShopUrl.Trim().Trim('/'));

            if (products.Count > 8)
            {
                //删除原来的商品：
                ProductDataAccess.DeleteProduct(shop.ID);
                ProductDataAccess.InsertProducts(products, shop.ID);

                ShopDataAccess.UpdateShopAsyncDate(shop.ID);
            }

            shop.FetchDate =DateTime.Now;
            ShopDataAccess.UpdateFetchDate(shop.ID);

            return new Tuple<string, int>(string.Format("获取店铺：{0}完成，总过获取商品数量为：{1}个", shop.ShopName, products.Count / 2), shop.ID);

        }
    }
}
