using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honshu.Data.Entity
{
    public class ShopES
    {
        public int ID { get; set; }
        public string ShopName { get; set; }
        public string Photo { get; set; }
        public string MainBiz { get; set; }
        public int FavNum { get; set; }
        public string City { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LastModifyDate { get; set; }
        public DateTime LastCollectionDate { get; set; }
        public int ShopViews { get; set; }
        public int RedirectTimes { get; set; }
        public string PromoteURL { get; set; }
        public string ShortDesc { get; set; }
        public decimal PromoteAmount { get; set; }
        public int PromoteAccount { get; set; }
        public bool IsRecommend { get; set; }
        public bool IsSelected { get; set; }
        public bool IsHot { get; set; }
        public DateTime HotDate { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime RecommendDate { get; set; }
        public string ShopTags { get; set; }
        public int CatId { get; set; }
        public string ShopUrl { get; set; }
        public string ProductPhotos { get; set; }
    }
}
