using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hakone.Web.Models
{
    public class ShopActionboxModel
    {
        public int ShopId { get; set; }
        public string CollectActionboxHtml { get; set; }
        public string RecommendActionboxHtml { get; set; }
        public string HotActionboxHtml { get; set; }
        public string SelectedActionboxHtml { get; set; }
    }
}