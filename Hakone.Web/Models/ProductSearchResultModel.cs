using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain;

namespace Hakone.Web.Models
{
    public class ProductSearchResultModel
    {
        public List<Product> RecommendProductsResult { get; set; }

        public List<Product> AllProductsResult { get; set; }

        public List<Product> TbkProductsResult { get; set; }
    }
}