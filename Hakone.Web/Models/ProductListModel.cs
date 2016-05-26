using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain;
using MvcPaging;

namespace Hakone.Web.Models
{
    public class ProductListModel
    {
        public IPagedList<Product> PagingProducts { get; set; }
        public int CatId { get; set; }
        public string CatName { get; set; }
    }
}