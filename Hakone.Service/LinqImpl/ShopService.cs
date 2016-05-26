using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheAspect;
using CacheAspect.Attributes;
using Hakone.Cube;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Domain.Enum;
using Hakone.Service;
using MvcPaging;

namespace Hakone.Service
{
    public partial class ShopService : GenericController<Shop, Hakone.Domain.HakoneDBDataContext> , IShopService 
    {
        public IPagedList<Shop> GetList(int page, int r, string orderby, string s, int pagesize=100)
        {
            if (orderby.IsNullOrEmpty()) orderby = ShopOrderBy.UpdateDate.ToString();

            var shopOrderby = @orderby.ToEnum<ShopOrderBy>();

            var list = SelectAll();
            if (s.IsNotNullOrEmpty()) list = list.Where(q => q.ShopName.Contains(s));
            if (r == 0) list = list.Where(q => q.IsRecommend == false);

            switch (shopOrderby){
                case ShopOrderBy.ByCollectionDate:
                    list = list.OrderByDescending(q => q.LastCollectionDate);
                    break;
                case ShopOrderBy.ByRecommendDate:
                    list = list.OrderByDescending(q => q.RecommendDate);
                    break;
                case ShopOrderBy.EntryDate:
                    list = list.OrderByDescending(q => q.EntryDate);
                    break;
                case ShopOrderBy.PromoteAccount:
                    list = list.OrderByDescending(q => q.PromoteAccount);
                    break;
                case ShopOrderBy.PromoteAmount:
                    list = list.OrderByDescending(q => q.PromoteAmount);
                    break;
                case ShopOrderBy.UpdateDate:
                    list = list.OrderByDescending(q => q.LastModifyDate);
                    break;
                case ShopOrderBy.ByDefault:
                    break;
                default:
                    list = list.OrderByDescending(q => q.LastModifyDate);
                    break;
            }
            list = list.Skip(page * pagesize).Take(pagesize);
            return list.ToPagedList(page, pagesize, list.Count() < pagesize ? list.Count() : 100000);
        }


        public Shop GetShop(int shopId)
        {
            return GetEntity(shopId);
        }

        public Shop GetShopByShopIndex(int shopIndex)
        {
            return SelectAll().FirstOrDefault(q => q.ShopIndex == shopIndex);
        }

        public Shop GetShopByShopName(string shopName)
        {
            return SelectAll().FirstOrDefault(q => q.ShopName == shopName);
        }

        public Shop CreateOrUpdate(Shop shop)
        {
            if (shop.ID > 0)
            {
                Update(shop,true);
            }
            else
            {
                Insert(shop, true);
            }

            return shop;
        }

        public void CollectShop(int shopId, bool value)
        {
            var shop = GetShop(shopId);
            shop.LastCollectionDate = DateTime.Now;
            Update(shop, true);
        }

        public void RecommendShop(int shopId, bool value)
        {
            var shop = GetShop(shopId);
            shop.IsRecommend = value;
            shop.RecommendDate = DateTime.Now;
            shop.AsyncDate = DateTime.Now.AddDays(-3000);
            Update(shop, true);
        }

        public void HotShop(int shopId, bool value)
        {
            var shop = GetShop(shopId);
            shop.IsHot = value;
            shop.HotDate = DateTime.Now;
            shop.AsyncDate = DateTime.Now.AddDays(-3000);
            Update(shop, true);
        }

        public void SelectedShop(int shopId, bool value)
        {
            var shop = GetShop(shopId);
            shop.IsSelected = value;
            shop.SelectedDate = DateTime.Now;
            shop.AsyncDate = DateTime.Now.AddDays(-3000);
            Update(shop, true);
        }


        public void Delete(int shopId)
        {
            var shop = GetShop(shopId);
            shop.IsDeleted = true;
            shop.AsyncDate = DateTime.Now.AddDays(-3000);
            Update(shop, true);
        }


        public void UpdateFetchDate(int shopId)
        {
            var shop = GetShop(shopId);
            shop.FetchDate = DateTime.Now.AddDays(-3000);
            Update(shop, true);
        }

        public void UpdateAsyncDate(int shopId)
        {
            var shop = GetShop(shopId);
            shop.FetchDate = shop.AsyncDate.AddDays(-3000);
            Update(shop, true);
        }


        public IPagedList<ShopES> GetUserFavs(int userId, int page)
        {
            const string sql = @"Select A.*,
                                STUFF
                                    (
                                        (
                                            SELECT Top 4 ', ' + Products.Photo
                                            FROM Products(NoLock)
                                            WHERE Products.ShopId = A.ID
                                            ORDER BY Products.DefaultSortDate Desc
                                            FOR XML PATH (''),TYPE
                                        ).value('.','nvarchar(max)')
                                        ,1,2,''
                                    ) AS ProductPhotos From Shops A 
	                                inner join UserFavShops B On A.ID = B.ShopID and B.UserID ={0} 
	                                Order By B.EntryDate Desc";
           var list = DataContext.ExecuteQuery<ShopES>(sql, userId).ToList();

            return list.ToPagedList(page, 21);
        }
    }
}
