using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain;

namespace Hakone.Web.Models
{
    public class HomeModel
    {
        public List<ShopES> LatestSelectedShops { get; set; }
        public List<ShopES> LatestRecommendShops { get; set; }

        public List<Product> LatestRecommendProducts { get; set; }
    }
}