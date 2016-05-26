using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hakone.Domain;
using Hakone.Domain.Enum;
using Hakone.Service;
using Hakone.Web.Models;
using Hakone.Cube;
using System.Text;

namespace Hakone.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IShopService _shopService;
        private readonly IShopCategoryService _shopCategoryService;
        private readonly IShopCommentService _shopCommentService;
        private readonly IUserFavShopService _userFavShopService;
        private readonly IProductService _productService;

        public ShopController(IShopService shopService
            , IShopCategoryService shopCategoryService
            , IShopCommentService shopCommentService
            , IProductService productService
            , IUserFavShopService userFavShopService
            , IUserService userService)
            : base(userService)
        {
            _shopService = shopService;
            _shopCategoryService = shopCategoryService;
            _shopCommentService = shopCommentService;
            _productService = productService;
            _userFavShopService = userFavShopService;

        }

        public ActionResult List(string listFor = "all", int? catId = null, int page = 1, string keyword = "")
        {
            keyword = keyword.Trim();
            var catItem = GetShopCats().SingleOrDefault(r => catId != null && r.Value == catId.Value.ToString());
            var viewModel = new ShopListModel
            {
                PagingShops = _shopService.GetPagedList(listFor, catId, keyword, page - 1, string.Empty, string.Empty, 24),  //_shopWithProductService.GetList(listFor, catId, page - 1, keyword, tag),
                Keyword = keyword,
                ListFor = listFor,
                CatName = catItem == null ? "全部" : catItem.Text,
                CatId = catId
            };

            ViewBag.Keyword = keyword;

            return View(viewModel);
        }

        //GET:店铺详情
        public ActionResult Detail(int id, int page = 1, string orderby ="dft")
        {
            var shop = _shopService.GetShop(id);
            if(shop.IsNull())
            {
                throw new HttpException(404, "Page not found");
            }

            var isFaved = IsFavShop(shop.ID, _userFavShopService.GetListByUser(UserId));

            var model = new ShopDetailModel
            {
                Shop = shop,
                Products = _productService.GetProductsByShop(shop.ID, page - 1, orderby),
                CollectionHtml = GetCollectActionboxHtml(isFaved, id)
            };

            ViewBag.OrderBy = orderby;
            ViewBag.UserId = UserId;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult CollectShop(int shopId, string comment, bool value)
        {
            try
            {
                _shopService.CollectShop(shopId, value);
                _shopCommentService.AddComment(UserId, shopId, comment, ShopCommentFlag.CollectShop.ToInt());
                _userFavShopService.AddOrRemove(UserId, shopId, string.Empty, value);
                return Json(value ? "收藏店铺成功" : "取消成功", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("发生错误，请确认输入或者联系管理员", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [Authorize]
        public JsonResult RecommendShop(int shopId, string comment, bool value)
        {
            try
            {
                _shopService.RecommendShop(shopId, value);
                _shopCommentService.AddComment(UserId, shopId, comment, ShopCommentFlag.RecommendShop.ToInt());
                return Json("推荐店铺成功", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("发生错误，请确认输入或者联系管理员", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public JsonResult HotShop(int shopId, string comment, bool value)
        {
            try
            {
                _shopService.HotShop(shopId, value);
                _shopCommentService.AddComment(UserId, shopId, comment, ShopCommentFlag.HotShop.ToInt());
                return Json(value ? "设置热门店铺成功" : "取消成功", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("发生错误，请确认输入或者联系管理员", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public JsonResult SelectedShop(int shopId, string comment, bool value)
        {
            try
            {
                _shopService.SelectedShop(shopId, value);
                _shopCommentService.AddComment(UserId, shopId, comment, ShopCommentFlag.SelectedShop.ToInt());
                return Json(value ? "设置精选店铺成功" : "取消成功", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("发生错误，请确认输入或者联系管理员", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [RoleBaseAuthorize(EnumUserRole.Administrator)]
        public JsonResult Delete(int shopId)
        {
            _shopService.Delete(shopId);
            return Json("删除店铺成功", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public JsonResult Reft(int shopId)
        {
            _shopService.UpdateFetchDate(shopId);
            return Json("刷新成功", JsonRequestBehavior.AllowGet);
        }

        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public ActionResult Create()
        {
            ViewBag.ShopCats = GetShopCats();
            return View();
        }

        [HttpGet]
        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public ActionResult Edit(int id)
        {
            var shop = _shopService.GetShop(id);
            var model = AutoMapper.Mapper.Map<ShopFormModel>(shop);
            ViewBag.ShopCats = GetShopCats();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public ActionResult Save(ShopFormModel form)
        {
            ViewBag.ShopCats = GetShopCats();

            if (ModelState.IsValid)
            {
                Shop shop;
                if (form.ID == 0)
                {
                    //检验shopname是否已经存在
                    if ( _shopService.GetShopByShopName(form.ShopName).IsNotNull())
                    {
                        Error("该店铺已经存在,请确认!");
                        return View("Create", form);

                    }
                    shop = new Shop();
                    shop = shop.SettingDefault(shop);
                }
                else
                {
                    shop = _shopService.GetShop(form.ID);
                }
                shop.ShopName = form.ShopName;
                shop.MainBiz = form.MainBiz;
                shop.Photo = form.Photo;
                shop.ShopTags = form.ShopTags ?? string.Empty;
                shop.ShortDesc = form.ShortDesc;
                shop.CatId = form.CatId;
                shop.ShopUrl = form.ShopUrl;
                shop.LastModifyDate =DateTime.Now;
                shop.PromoteURL = form.PromoteURL.IsNullOrEmpty() ? string.Empty : form.PromoteURL;

                if (ModelState.IsValid)
                {
                    _shopService.CreateOrUpdate(shop);
                }

                Success("操作成功");
            }

            return View(form.ID == 0 ? "Create" : "Edit", form);
        }

        [ChildActionOnly]
        public ActionResult Actionbox(ShopES shop)
        {
            var isFaved = IsFavShop(shop.ID, _userFavShopService.GetListByUser(UserId));

            var model = new ShopActionboxModel
            {
                ShopId = shop.ID,
                CollectActionboxHtml = GetCollectActionboxHtml(isFaved, shop.ID),
                RecommendActionboxHtml = GetRecommendActionboxHtml(shop.IsRecommend, shop.ID),
                HotActionboxHtml = GetHotActionboxHtml(shop.IsHot, shop.ID),
                SelectedActionboxHtml = GetSelectedActionboxHtml(shop.IsSelected, shop.ID)
            };

            return PartialView("_ShopActionbox", model);
        }

        [ChildActionOnly]
        public ActionResult ShopCommentInfo(int shopId)
        {
            //var model = _shopCommentService.GetShopComment(shopId, GetCommentFlagByListFor(listFor));
            var model = _shopCommentService.GetShopComment(shopId, shopId.GetNumberFromNumber());
            return PartialView("_ShopCommentInfo", model);
        }

        [ChildActionOnly]
        public ActionResult Comments(int shopId)
        {
            var model = _shopCommentService.GetList(shopId, 0);
            return PartialView("_ShopComments", model);
        }

        [RoleBaseAuthorize(EnumUserRole.Administrator, EnumUserRole.Editor)]
        public ActionResult Entire(string listFor ="all", int? catId=null, int page = 1, string orderby = "", string q = "", string city="")
        {
            q = q.Trim();
            var list = _shopService.GetPagedList(listFor, catId, q, page - 1, orderby, city);
            ViewBag.Keyword = q;
            ViewBag.ListFor = listFor;
            return View(list);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(ShopCommentFormModel comment)
        {
            if (comment.CommentContent.IsNotNullOrEmpty())
            {
                _shopCommentService.AddComment(UserId, comment.ShopId, comment.CommentContent,
                    ShopCommentFlag.Normal.ToInt());
                Success("添加评论成功");
            }

            var model = _shopCommentService.GetList(comment.ShopId, 0);
            return PartialView("_ShopComments", model);
        }

        #region  Helper

        private int GetCommentFlagByListFor(string listFor)
        {
            var listF = listFor.ToEnum<ShopListFor>();
            switch (listF)
            {
                case ShopListFor.recommend:
                    return ShopCommentFlag.RecommendShop.ToInt();
                case ShopListFor.hot:
                    return ShopCommentFlag.HotShop.ToInt();
                case ShopListFor.selected:
                    return ShopCommentFlag.SelectedShop.ToInt();
                default:
                    return ShopCommentFlag.Normal.ToInt();
            }
        }

        private List<SelectListItem> GetShopCats()
        {
            return _shopCategoryService.GetShopCats().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.CategoryName,
                                      Value = x.ID.ToString()
                                  }).ToList();
        }
        #endregion
    }
}