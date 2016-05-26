using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Domain
{
    public class ProductES
    {
        public int ID { get; set; }
        public long ProductIndex { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public int DirectTimes { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime RecommendDate { get; set; }
        public DateTime LastCollectionDate { get; set; }
        public bool IsRecommend { get; set; }
        public int AmountSales { get; set; }

        public int CatId { get; set; }
        public int ShopId { get; set; }
        public DateTime DefaultSortDate { get; set; }
        public int SortNewNumber { get; set; }
        public int SortSalesNumber { get; set; }
        //public bool IsDeleted { get; set; }
        //public bool IsChecked { get; set; }
        //public DateTime AsyncDate { get; set; }

        public bool IsHotShop { get; set; }
        public DateTime ShopHotDate { get; set; }
        public bool IsRecommendShop { get; set; }
        public DateTime ShopRecommendDate { get; set; }
        public bool IsSelectedShop { get; set; }
        public DateTime ShopSelectDate { get; set; }
    }
}
