using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hakone.Domain.Enum;
using Hakone.Service;
using Hakone.Web.Models;

namespace Hakone.Web.Controllers
{
    public class DianController : BaseController
    {
        private readonly IShopService _shopService;

        public DianController(IShopService shopService, IUserService userService)
            : base(userService)
        {
            _shopService = shopService;
        }

        public ActionResult Index(string listFor = "all", int? catId = null, int page = 1, string city = "", string q = "")
        {
            var model = new DianListModel
            {
                PagingShops = _shopService.GetPagedList(listFor, catId, q.Trim(), page - 1, ShopOrderBy.PromoteAccount.ToString(), city, 30), //_shopService.GetList(page - 1, r, ShopOrderBy.PromoteAmount.ToString(), s, 30)
            };
            ViewBag.Keyword = q.Trim();

            return View(model);
        }
    }
}