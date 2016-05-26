using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Hakone.SendCloud;
using Hakone.Cube;
using Hakone.Web.Attributes;
using Hakone.Service;
using Hakone.Domain;

namespace Hakone.Web.Controllers
{
    [HandleExceptions]
    public class BaseController : Controller
    {
        private readonly IUserService _userService;
        public BaseController() { }
        public BaseController(IUserService userService)
        {
            _userService = userService;
        }
        public void Success(string str = "操作成功")
        {
            ViewBag.SuccessMsg = str;
        }

        public void Error(string str = "操作失败")
        {
            ViewBag.ErrorMsg = str;
        }

        public void Warning(string str = "操作警告")
        {
            ViewBag.Warning = str;
        }

        public int UserId
        {
            get {
                return Request.IsAuthenticated ? _userService.GetUserByUserName(System.Web.HttpContext.Current.User.Identity.Name).UserID : 0;
            }
        }

        public bool IsFavShop(int shopId, List<UserFavShop> list)
        {
            return list.FirstOrDefault(r => r.ShopID == shopId) != null; //list已经使用userId过滤过了，所以这里不用加user限制条件了。下同
        }

        public bool IsFavProduct(int productId, List<UserFavProduct> list)
        {
            return list.FirstOrDefault(r => r.ProductID == productId) != null;
        }

        public string GetCollectActionboxHtml(bool isFaved,int shopId)
        {
            return isFaved ? FormatActionboxHtml("collectshop", "shopActionClick(this, " + shopId + ", false)", "取消该店铺收藏", "&#xe607;", true)
                : FormatActionboxHtml("collectshop", "shopActionClick(this, " + shopId + ", true)", "收藏该店铺", "&#xe607;", false);
        }

        public string GetRecommendActionboxHtml(bool isRecommend, int shopId)
        {
            var cssSelected = isRecommend && Request.IsAuthenticated
                &&
                (System.Web.HttpContext.Current.User.IsInRole("Editor") ||
                 System.Web.HttpContext.Current.User.IsInRole("Administrator"));
            return FormatActionboxHtml("recommendshop", "shopActionClick(this, " + shopId + ", true)", "推荐店铺", "&#xe615;", cssSelected);
        }

        public string GetHotActionboxHtml(bool isHot, int shopId)
        {
            return isHot ? FormatActionboxHtml("hotshop", "shopActionClick(this, " + shopId + ", false)", "取消热门店铺", "&#xe61c;", true)
                : FormatActionboxHtml("hotshop", "shopActionClick(this, " + shopId + ", true)", "设为热门店铺", "&#xe61c;", false);
        }

        public string GetSelectedActionboxHtml(bool isSelected, int shopId)
        {
            return isSelected ? FormatActionboxHtml("selectedshop", "shopActionClick(this, " + shopId + ", false)", "取消精选店铺", "&#xe622;", true)
                : FormatActionboxHtml("selectedshop", "shopActionClick(this, " + shopId + ", true)", "设为精选店铺", "&#xe622;", false);
        }

        public string GetProductCollectActionboxHtml(bool isFaved, int productId)
        {
            return isFaved ? FormatActionboxHtml("collect", "productCollectClick(this, " + productId + ", false)", "取消该商品收藏", "&#xe607;", true)
                : FormatActionboxHtml("collect", "productCollectClick(this, " + productId + ", true)", "收藏该商品", "&#xe607;", false);
        }

        public void SendResetPasswordEmail(string toEmail, string userName, string guid)
        {
            const string xsmtpapi = "{\"to\": [\"{EmailTo}\"], \"sub\" : { \"%UserName%\" : [\"{UserName}\"], \"%Guid%\" : [\"{Guid}\"]}}";

            var mail = new MailEndityWithTemplate
            {
                TemplateInvokeName = "ResetPassword",
                ApiKey = ConfigurationManager.AppSettings["SendCloud-ApiKey"].ToString(),
                ApiUser = ConfigurationManager.AppSettings["SendCloud-ApiUser"].ToString(),
                XsmtpApi = xsmtpapi.Format(new { EmailTo = toEmail, UserName = userName, Guid = guid })
            };

            MailSend.Send(mail);
        }

        private static string FormatActionboxHtml(string action, string jsEvent, string title, string icon, bool cssclass)
        {
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                jsEvent = string.Empty;
            }
            return ConstConfig.ActionEventHtmlTemplate.Format(new
            {
                action = action,
                JsEvent = jsEvent,
                title = title,
                href = CommonHelper.AjaxActionLink(),
                icon = icon,
                cssclass = cssclass ? "selected" : ""
            });
        }

        public string GetModelStateErrors()
        {
            var result =new StringBuilder();
            var allErrors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in allErrors)
            {
                result.AppendFormat("<p>{0}</p>", error.ErrorMessage);
            }

            return result.ToString();
        }
    }
}