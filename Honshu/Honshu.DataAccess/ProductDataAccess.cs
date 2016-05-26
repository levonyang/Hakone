using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honshu.Data;
using Honshu.Data.Enum;
using Honshu.Data.LinqUtility;

namespace Honshu.DataAccess
{
    public class ProductDataAccess : GenericController<Product, HonshuDBDataContext>
    {
        public static void InsertProducts(List<Honshu.Data.Entity.Product> list, int shopId)
        {
            var sortNumber = 0;
            foreach (var product in list.Where(r => r.OrderType == Enums.OrderType.hotsell_desc.ToString()).Reverse())
            {
                sortNumber ++;
                InsertOrUpdateProduct(product, shopId, sortNumber);
            }

            sortNumber = 0;
            foreach (var product in list.Where(r => r.OrderType == Enums.OrderType.newOn_desc.ToString()).Reverse())
            {
                sortNumber++;
                UpdateProductNewSort(product.ProductIndex, shopId, sortNumber);
            }
        }

        private static void UpdateProductNewSort(long productIndex, int shopId, int sortNumber)
        {
            var p = SelectAll().FirstOrDefault(r => r.ProductIndex == productIndex);
            if (p.IsNotNull())
            {
                ProductDataAccess.DataContext.ExecuteCommand(@"Update Products Set SortNewNumber = {0} Where ID ={1}",
                      sortNumber, p.ID);
            }
        }

        private static void InsertOrUpdateProduct(Data.Entity.Product product, int shopId, int sortNumber)
        {
            var p = SelectAll().FirstOrDefault(r => r.ProductIndex == product.ProductIndex);
            if (p.IsNull())
            {
                //添加Product:
                p = new Product
                {
                    AmountSales = product.AmountSales,
                    CatId = 0,
                    DefaultSortDate = DateTime.Now.AddSeconds(sortNumber),
                    DirectTimes = 0,
                    EntryDate = DateTime.Now,
                    IsChecked = true,
                    IsDeleted = false,
                    IsRecommend = false,
                    LastCollectionDate = new DateTime(2010, 10, 10),
                    Photo = product.Photo,
                    Price = product.Price,
                    ProductIndex = product.ProductIndex,
                    ProductName = product.ProductName,
                    RecommendDate = new DateTime(2010,10,10),
                    ShopId = shopId,
                    SortNewNumber = 0,
                    SortSalesNumber = sortNumber,
                    UpdateDate = DateTime.Now,
                    AsyncDate =DateTime.Now, //这个即将以店铺为单位同步到ES，所以不必要设置一个旧时间
                };

                Insert(p, true);
            }
            else
            {
                ProductDataAccess.DataContext
                    .ExecuteCommand(@"Update Products Set ProductName ={0}, Price ={1}, AmountSales ={2}, Photo ={3}, 
                        SortNewNumber = 0, SortSalesNumber ={4}, DefaultSortDate ={5}, IsDeleted =0
                        , UpdateDate=GetDate(), IsChecked =1, ShopId ={6}  Where ID ={7}",
                      product.ProductName, product.Price, product.AmountSales, product.Photo, sortNumber
                      , DateTime.Now.AddSeconds(sortNumber), shopId, p.ID);
            }
        }

        public static void DeleteProduct(int shopId)
        {
            using (var db = new HonshuDBDataContext())
            {
                ProductDataAccess.DataContext
                    .ExecuteCommand(
                        @"Update Products Set IsDeleted =1, SortNewNumber =0,SortSalesNumber =0, DefaultSortDate =GETDATE() Where ShopId = {0}"
                        , shopId);
            }
        }

        public static List<Honshu.Data.Entity.ProductES> GetTopProductsToElasticSearch()
        {
            using (HonshuDBDataContext db = new HonshuDBDataContext())
            {
                var sql = @"Select Top {0} P.*
                        , S.ShopName
                        , S.IsHot IsHotShop, S.HotDate ShopHotDate
                        , S.IsRecommend IsRecommendShop, S.RecommendDate ShopRecommendDate
                        , S.IsSelected IsSelectedShop, S.SelectedDate ShopSelectedDate
                        From Products P Inner Join Shops S On P.ShopId = S.ID  
                        Where P.IsDeleted = 0 And DATEDIFF(d,P.AsyncDate,GETDATE()) > 60 Order By P.AsyncDate";
                var result = db.ExecuteQuery<Honshu.Data.Entity.ProductES>(string.Format(sql, AsyncCountEachTime)).ToList();

                return result;
            }
        }

        public static void UpdateProductAsyncDate()
        {
            using (HonshuDBDataContext db = new HonshuDBDataContext())
            {
                var sql = @"Update Products Set AsyncDate = GetDate() Where ID In (Select Top {0} P.ID
                        From Products P Inner Join Shops S On P.ShopId = S.ID Where P.IsDeleted = 0 And DATEDIFF(d,P.AsyncDate,GETDATE()) > 60 Order By P.AsyncDate)";
                db.ExecuteCommand(string.Format(sql, AsyncCountEachTime));
            }
        }

        public static List<Honshu.Data.Entity.ProductES> GetProductsByShopId(int shopId)
        {
            using (var db = new HonshuDBDataContext())
            {
                var sql = @"Select P.*
                        , S.ShopName
                        , S.IsHot IsHotShop, S.HotDate ShopHotDate
                        , S.IsRecommend IsRecommendShop, S.RecommendDate ShopRecommendDate
                        , S.IsSelected IsSelectedShop, S.SelectedDate ShopSelectedDate
                        From Products P Inner Join Shops S On P.ShopId = S.ID Where ShopId = {0} And P.IsDeleted = 0";
                var result = db.ExecuteQuery<Honshu.Data.Entity.ProductES>(sql, shopId).ToList();

                return result;
            }
        }

        private static string AsyncCountEachTime
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AsyncProductCountEachTime");
            }
        }
    }
}
