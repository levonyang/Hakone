using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain;
using MvcPaging;

namespace Hakone.Web.Models
{
    public class DianListModel
    {
        public IPagedList<ShopES> PagingShops { get; set; }
    }
}