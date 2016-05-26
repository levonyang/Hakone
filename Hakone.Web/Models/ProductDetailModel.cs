using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain;

namespace Hakone.Web.Models
{
    public class ProductDetailModel
    {
        public Product Product { get; set; }

        public Shop Shop { get; set; }

        public List<Product> ShopProducts { get; set; }

        public string ProductCollectionHtml { get; set; }
    }
}