using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Service;
using Hakone.Cube;
using MvcPaging;
using CacheAspect.Attributes;
using Hakone.Domain.Enum;

namespace Hakone.Service
{
    public partial class ProductService : GenericController<Product, Hakone.Domain.HakoneDBDataContext>, IProductService
    {
        public Product GetProduct(int productId)
        {
            return GetEntity(productId);
        }

        [Cache.Cacheable(CacheKey.GetTopProducts)]
        public List<Product> GetTopProducts(int shopId)
        {
            return 
                SelectAll()
                    .Where(r => r.ShopId == shopId && r.IsDeleted == false)
                    .OrderByDescending(r => r.DefaultSortDate)
                    .Take(20)
                    .ToList();
        }

        public IPagedList<Product> GetProductsByShop(int shopId, int page, string orderby)
        {
            var list = ProductService.SelectAll().Where(r => r.ShopId == shopId && r.IsDeleted == false);
            var orderType = orderby.ToEnum<ShopProductsOrderBy>();
            switch (orderType)
            {
                case ShopProductsOrderBy.newup:
                    list = list.OrderByDescending(r => r.SortNewNumber);
                    break;
                case  ShopProductsOrderBy.sales:
                    list = list.OrderByDescending(r => r.SortSalesNumber);
                    break;
                default:
                    list = list.OrderByDescending(r => r.DefaultSortDate);
                    break;
            }

            return list.ToPagedList(page, 60);
        }


        public void Recommend(Product product)
        {
            product.RecommendDate = DateTime.Now;
            product.AsyncDate = DateTime.Now.AddDays(-100);
            Update(product, true);
        }

        public void Delete(int productId)
        {
            var product = GetProduct(productId);
            product.IsDeleted = true;
            Update(product, true);
        }


        public void CollectProduct(int productId, bool value)
        {
            var product = GetProduct(productId);
            product.LastCollectionDate = DateTime.Now;
            Update(product, true);
        }


        public IPagedList<Product> GetUserFavs(int userId, int page)
        {
            const string sql = @"Select A.* From Products A 
                                 inner join UserFavProducts B On A.ID = B.ProductID and B.UserID =@UserId 
                                 Order By B.EntryDate Desc";
            var list = DefaultContext.Sql(sql).Parameter("UserId", userId).QueryMany<Product>();

            return list.ToPagedList(page, 21);
        }

        public int Goup(int productId)
        {
            var product = GetProduct(productId);
            product.DefaultSortDate = DateTime.Now;
            Update(product, true);
            return product.ShopId;
        }


        public List<Product> LatestRecommendProducts()
        {
            return SelectAll()
                .Where(r => r.IsRecommend == true && r.IsDeleted == false).OrderByDescending(r => r.RecommendDate).Take(30).ToList();
        }
    }
}
