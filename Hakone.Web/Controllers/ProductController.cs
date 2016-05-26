using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hakone.Cube;
using Hakone.Domain;
using Hakone.Domain.Enum;
using Hakone.Service;
using Hakone.Web.Models;

namespace Hakone.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IShopService _shopService;
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IUserFavProductService _userFavProductService;
        public ProductController(IShopService shopService, IProductService productService
            ,IProductCategoryService productCategoryService
            ,IUserFavProductService userFavProductService
            , IUserService userService) : base(userService)
        {
            _shopService = shopService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _userFavProductService = userFavProductService;
        }

        public ActionResult List(int catId = 0, string s = "", int? r = null, int page = 1) //s表示搜索关键字，t=""是，表示搜索推荐的商品，all表示搜索所有商品
        {
            var catItem = GetProductCats().SingleOrDefault(q => catId > 0 && q.Value == catId.ToString());
            var model = new ProductListModel { PagingProducts = _productService.GetList(catId, s, r ?? 1, page - 1) };
            model.CatId = catId;
            model.CatName = catItem.IsNull() ? "" : catItem.Text;
            ViewBag.Keyword = s.Trim();
            ViewBag.IsRecommend = r;
            ViewBag.CatId = catId;
            return View(model);
        }

        // GET: Product
        public ActionResult Detail(int id)
        {
            var product = _productService.GetProduct(id);
            if (product.IsNull())
            {
                throw new HttpException(404, "Page not found");
            }

            var tbkItem = TaobaoApi.GetProduct(product.ProductIndex);
            if (tbkItem.IsNotNull())
            {
                product.AmountSales = (int)tbkItem.Volume;
                product.Price = product.Price > Convert.ToDecimal(tbkItem.ZkFinalPrice) ? Convert.ToDecimal(tbkItem.ZkFinalPrice) : product.Price;
            }
            var shop = _shopService.GetShop(product.ShopId);
            var shopProducts = _productService.GetTopProducts(shop.ID).OrderByRandom().Take(10).ToList();
            var isFaved = IsFavProduct(product.ID, _userFavProductService.GetListByUser(UserId));

            var model = new ProductDetailModel
            {
                Shop = shop,
                Product = product,
                ShopProducts = shopProducts,
                ProductCollectionHtml = GetProductCollectActionboxHtml(isFaved, id)
            };

            return View(model);
        }

        [HttpGet]
        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public ActionResult Recommend(int id)
        {
            ViewBag.ProductCats = GetProductCats();
            var product = _productService.GetProduct(id);
            var model = AutoMapper.Mapper.Map<ProductFormModel>(product);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public ActionResult Recommend(ProductFormModel form)
        {
            if (ModelState.IsValid)
            {
                var product = _productService.GetProduct(form.ID);
                product.ProductName = form.ProductName;
                product.CatId = form.CatId;
                product.Price = form.Price;
                product.AmountSales = form.AmountSales;
                product.IsRecommend = true;

                 _productService.Recommend(product);

                Success("操作成功！");
            }

            ViewBag.ProductCats = GetProductCats();
            return View("Recommend", form);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Collect(int productId, bool value)
        {
            try
            {
                _productService.CollectProduct(productId, value);
                _userFavProductService.AddOrRemove(UserId, productId, string.Empty, value);

                return Json("收藏商品成功", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("发生错误，请确认输入或者联系管理员", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [RoleBaseAuthorize(EnumUserRole.Administrator)]
        public JsonResult Delete(int productId)
        {
            _productService.Delete(productId);
            return Json("删除店铺成功", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public JsonResult Goup(int productId)
        {
            var shopId = _productService.Goup(productId);
            _shopService.UpdateAsyncDate(shopId);
            return Json("置顶成功", JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult Actionbox(Product product)
        {
            var isFaved = IsFavProduct(product.ID, _userFavProductService.GetListByUser(UserId));

            var model = new ProductActionboxModel
            {
                ProductId = product.ID,
                CollectActionboxHtml = GetProductCollectActionboxHtml(isFaved, product.ID)
            };

            return PartialView("_ProductActionbox", model);
        }

        public ActionResult Redirect(int id)
        {
            var product = _productService.GetProduct(id);
            return View(product);
        }

        #region
        private List<SelectListItem> GetProductCats()
        {
            return _productCategoryService.GetProductCats().Select(x =>
                new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.ID.ToString()
                }).ToList();
        }
        #endregion
    }
}