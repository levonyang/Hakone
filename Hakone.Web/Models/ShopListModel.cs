using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain;
using MvcPaging;

namespace Hakone.Web.Models
{
    public class ShopListModel
    {
        public IPagedList<ShopES> PagingShops { get; set; }

        //parameters:
        public string Keyword { get; set; }

        public string CatName{get;set;}
        public int? CatId { get; set; }
        public string ListFor { get; set; }
    }

}