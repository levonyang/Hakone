using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Domain
{
    public partial class Shop
    {
        public Shop SettingDefault(Shop shop)
        {
            shop.EntryDate = DateTime.Now;
            shop.City = string.Empty;
            shop.DescriptionDetail = string.Empty;
            var s = new Shop
            {
                ShopOwerID = 0,
                ShopName = string.Empty,
                ShopOwner = string.Empty,
                ShopOwerQQ = string.Empty,
                Photo = string.Empty,
                WangWang = string.Empty,
                MainBiz = string.Empty,
                FavNum = 0,
                City = string.Empty,
                ShopIndex = 0,
                EntryDate = DateTime.Now,
                LastModifyDate = DateTime.Now,
                RecommendDate = DateTime.Now,
                LastCollectionDate = DateTime.Now,
                IsRecommend = false,
                ShopViews = 0,
                RedirectTimes = 0,
                PromoteURL = string.Empty,
                ShortDesc = string.Empty,
                DescriptionDetail = string.Empty,
                IsDeleted = false,
                PromoteAmount = 0M,
                PromoteAccount = 0,
                IsChecked = true,
                IsSelected = false,
                IsHot = false,
                HotDate = DateTime.Now,
                SelectedDate = DateTime.Now,
                ShopTags = string.Empty,
                FetchDate = DateTime.Now,
                AsyncDate =DateTime.Now,
                CatId = 0
            };

            return s;
        }
    }
}
