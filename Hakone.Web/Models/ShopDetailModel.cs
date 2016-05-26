using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain;
using MvcPaging;

namespace Hakone.Web.Models
{
    public class ShopDetailModel
    {
        public Shop Shop { get; set; }
        public IPagedList<Product> Products { get; set; }

        public string CollectionHtml { get; set; }
    }
}