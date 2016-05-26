using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hakone.Service;
using Hakone.Web.Models;

namespace Hakone.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShopService _shopService;
        private readonly IProductService _productService;
        public HomeController(IShopService shopService, ProductService productService)
        {
            _shopService = shopService;
            _productService = productService;
        }
        public ActionResult Index()
        {
            var model = new HomeModel
            {
                LatestSelectedShops = _shopService.GetLatestSelectedShops(),
                LatestRecommendShops = _shopService.GetLatestRecommendShops(),
                LatestRecommendProducts = _productService.LatestRecommendProducts()
            };

            ViewBag.MetaDescription = ConstConfig.IndexMetaDescription;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}