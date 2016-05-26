using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Hakone.Cube;
using Hakone.Domain;
using Hakone.Domain.Enum;
using Hakone.Service;
using Hakone.Web.Models;
using Ninject;

namespace Hakone.Web.Controllers
{
    [Authorize]
    public class MyController : BaseController
    {
        private readonly IWorkContext _workContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly IProductService _productService;
        private readonly IShopService _shopService;
        private readonly IUserService _userService;


        public MyController(IWorkContext portalContext
            ,IAuthenticationService authenticationService
            ,IShopService shopService
            ,IProductService productService,
            IUserService userService) : base(userService)
        {
            _workContext = portalContext;
            _authenticationService = authenticationService;
            _shopService = shopService;
            _productService = productService;
            _userService = userService;
        }

        public ActionResult FavoriteShop(int page=1)
        {
            var model = _shopService.GetUserFavs(UserId, page - 1);
            return View(model);
        }

        public ActionResult FavoriteProduct(int page=1)
        {
            var model = _productService.GetUserFavs(UserId, page - 1);
            return View(model);
        }

        public ActionResult ProfileSetting()
        {
            var user = _workContext.CurrentUser;

            var model = AutoMapper.Mapper.Map<EditUserInfoModel>(user);

            ViewBag.BornYearList = GetYearDropdownDataSource(user.BornYear);
            ViewBag.BornMonthList = GetMonthDropdownDataSource(user.BornMonth);
            ViewBag.BornDayList = GetDayDropdownDataSource(user.BornDay);

            return View(model);
        }

        [HttpPost]
        [CatchModelStateErrorsActionFilter]
        public ActionResult ProfileSetting(EditUserInfoModel model)
        {
            var user = _workContext.CurrentUser;

            if (model.GenderName.IsNotNull())
            {
                model.Gender = model.GenderName != "男";
            }

            if (_userService.EditUserProfile(user.UserID, model.Email, model.BornYear, model.BornMonth, model.BornDay,
                model.Gender))
            {
                Success("个人资料修改成功");
                _authenticationService.ClearCacheUser();

            }
            else
            {
                Error();
            }

            ViewBag.BornYearList = GetYearDropdownDataSource(model.BornYear);
            ViewBag.BornMonthList = GetMonthDropdownDataSource(model.BornMonth);
            ViewBag.BornDayList = GetDayDropdownDataSource(model.BornDay);

            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var user = _workContext.CurrentUser;
            var model = new ChangePasswordModel();
            if (_userService.IsEmptyPassword(user.UserID)) model.Password = string.Empty;
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _workContext.CurrentUser;
                if (_userService.ChangePassword(user.UserID, model.NewPassword))
                {
                    Success("密码修改成功");
                }
                else
                {
                    Error("密码修改失败");
                }
                return View();
            }

            Error();
            return View();
        }

        public JsonResult CheckPassword(string password)
        {
            //var user = _workContext.CurrentUser;
            //if (user != null)
            //{
            //    var res = _accountService.ValidateUser(user.Username, password);
            //    return Json(res == UserLoginResult.Successful, JsonRequestBehavior.AllowGet);
            //}
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        private static List<SelectListItem> GetYearDropdownDataSource(int? year)
        {
            var items = new List<SelectListItem> {new SelectListItem {Text = "出生年份", Value = "0"}};

            for (var i = 1961; i <= DateTime.Now.Year; i++)
            {
                items.Add(new SelectListItem {Text = i.ToString(), Value = i.ToString(), Selected = year == i});
            }

            return items;
        }

        private static List<SelectListItem> GetDayDropdownDataSource(int? day)
        {
            var items = new List<SelectListItem> {new SelectListItem {Text = "出生日", Value = "0"}};

            for (var i = 1; i <= 31; i++)
            {
                items.Add(new SelectListItem {Text = i.ToString(), Value = i.ToString(), Selected = day == i});
            }

            return items;
        }

        private static List<SelectListItem> GetMonthDropdownDataSource(int? month)
        {
            var items = new List<SelectListItem> {new SelectListItem {Text = "出生月份", Value = "0"}};

            for (var i = 1; i <= 12; i++)
            {
                items.Add(new SelectListItem {Text = i.ToString(), Value = i.ToString(), Selected = month == i});
            }

            return items;
        }
    }
}