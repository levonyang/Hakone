using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honshu.Data;
using Honshu.Data.Entity;
using Honshu.Data.LinqUtility;

namespace Honshu.DataAccess
{
    public class ShopDataAccess : GenericController<Shop, HonshuDBDataContext>
    {
        public static void UpdateFetchDate(int shopId)
        {
            ShopDataAccess.DataContext.ExecuteCommand("Update Shops Set FetchDate = GetDate() Where ID = {0}", shopId);
        }

        public static Shop GetTopShop()
        {
            return SelectAll().Where(r => r.FetchDate.AddDays(5) < DateTime.Now)
                .OrderBy(r => r.FetchDate)
                .FirstOrDefault(r => r.IsHot == true || r.IsRecommend == true || r.IsSelected == true);
        }

        public static List<ShopES> GetTopShopsToElasticSearch()
        {
            using (var db = new HonshuDBDataContext())
            {
                var sql = @"SELECT Top {0}	*,
	                        STUFF
	                        (
		                        (
			                        SELECT Top 4 ', ' + Products.Photo
			                        FROM Products(NoLock)
			                        WHERE Products.ShopId = Shops.ID
			                        ORDER BY Products.DefaultSortDate Desc
			                        FOR XML PATH (''),TYPE
		                        ).value('.','nvarchar(max)')
		                        ,1,2,''
	                        ) AS ProductPhotos
                        FROM Shops(NoLock) Where IsDeleted = 0 And DATEDIFF(d,AsyncDate,GETDATE()) > 60 Order By AsyncDate";
                return db.ExecuteQuery<Honshu.Data.Entity.ShopES>(string.Format(sql, AsyncCountEachTime)).ToList();
            }
        }

        public static void UpdateProductAsyncDate()
        {
            using (var db = new HonshuDBDataContext())
            {
                const string sql = @"Update Shops Set AsyncDate = DATEADD(ms,-ID,GETDATE()) Where ID In (Select Top {0} ID From Shops(NoLock) 
                        Where IsDeleted = 0 And DATEDIFF(d,AsyncDate,GETDATE()) > 60 Order By AsyncDate)";
                db.ExecuteCommand(string.Format(sql, AsyncCountEachTime));
            }
        }

        public static void UpdateShopAsyncDate(int shopId)
        {
            const string sql = "UPDATE Shops SET AsyncDate = AsyncDate -1000 WHERE ID = @p0";
            DataContext.ExecuteCommand(sql, shopId);
        }

        private static string AsyncCountEachTime
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AsyncShopCountEachTime");
            }
        }
    }
}
