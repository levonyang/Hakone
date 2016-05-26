using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Web.Security;
using Hakone.Domain;
using Hakone.Domain.Enum;
using Hakone.Service;
using Hakone.Web.Models;
using Hakone.Cube;
using log4net;
using Ninject;

namespace Hakone.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AccountController(IUserService userService, IWorkContext workContext)
        {
            _userService = userService;
            _workContext = workContext;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = AutoMapper.Mapper.Map<User>(model);
                var registrationResult = _userService.RegisterUser(user);
                if (registrationResult.Success)
                {
                    AuthenticationService.SignIn(user, true);

                    if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                        return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }
                foreach (var error in registrationResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }

            }
            else
            {
                Error(GetModelStateErrors());
            }
            return View(model);
        }


        public ActionResult Login()
        {
            log.Debug("开始登录");
            var auth = Hakone.Web.OAuth.OAuth2Factory.Current;
            if (auth != null) //说明用户点击了授权，并跳回登陆界面来
            {
                log.Debug(auth.nickName + "logon on");
                var account = string.Empty;
                if (auth.Authorize(out account))//检测是否授权成功，并返回绑定的账号（）
                {
                    log.Debug(auth.nickName + "授权成功");
                    auth.SetBindAccount(HttpUtility.UrlDecode(auth.nickName));

                    User user = _userService.GetUserByUserNameNoCache(HttpUtility.UrlDecode(auth.nickName));

                    AuthenticationService.SignIn(user, true);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpPost]
        [CatchModelStateErrorsActionFilter]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (model.UserName != null)
                {
                    model.UserName = model.UserName.Trim();
                }
                UserLoginResult loginResult = _userService.ValidateUser(model.UserName, model.Password);

                switch (loginResult)
                {
                    case UserLoginResult.Successful:
                        {
                            User user = _userService.GetUserByUserNameOrEmail(model.UserName);
                            //sign in new customer
                            AuthenticationService.SignIn(user, model.RememberMe);

                            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return RedirectToAction("Index", "Home");
                            return Redirect(returnUrl);
                        }
                    case UserLoginResult.UserNotExist:
                        ModelState.AddModelError("", "用户不存在");
                        break;
                    case UserLoginResult.Locked:
                        ModelState.AddModelError("", "用户已锁定");
                        break;
                    case UserLoginResult.NotActive:
                        ModelState.AddModelError("", "用户没有激活");
                        break;
                    case UserLoginResult.WrongPassword:
                    default:
                        ModelState.AddModelError("", "密码错误");
                        break;
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public JsonResult CheckPassword(string password)
        {
            var user = _workContext.CurrentUser;
            if (user != null)
            {
                var result = _userService.ValidateUser(user.UserName, password);
                return Json(result == UserLoginResult.Successful, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUserName(string userName)
        {
            var result = _userService.GetUserByUserName(userName);
            return Json(result == null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmail(string email)
        {
            var user = _workContext.CurrentUser;
            if (user != null)
            {
                //when edit user info, only when email change:
                if (user.Email != email)
                {
                    var result = _userService.GetUserByEmail(email);
                    return Json(result == null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet); 
                }
            }
            else
            {
                //when register:
                var result = _userService.GetUserByEmail(email);
                return Json(result == null, JsonRequestBehavior.AllowGet);
                
            }
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }



        [HttpPost]
        public ActionResult ForgetPassword(ResetPasswordRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByEmail(model.Email);

                if (user == null)
                {
                    Error("对不起，找回密码失败，请确认你是否正确输入了 Email 地址！");
                }
                else
                {

                    SendResetPasswordEmail(user.Email, user.UserName, user.GUID.ToString());
                    Success(@"  <p>密码找回邮件已经发送成功！</p>
                        <p>现在请登录到你的电子邮箱中接收一封我们刚刚发送给你的的邮件，点击邮件中的链接地址即可重设密码。</p>
                        <p>邮件中的链接地址的有效时间为 24 小时，超过此时间后邮件中的链接地址将变得无效，然后你将需要重新提起密码重设申请</p>");
                }                
            }

            return View();
        }

        public ActionResult PasswordReset(string id)
        {
            var user = _userService.GetUserByGuid(id);
            
            if(user==null)
            {
                Error("请求错误");
                return View("ForgetPassword");
            }

            var model = new ResetPasswordModel();
            model.Guid = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordReset(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByGuid(model.Guid);
                if (_userService.ChangePassword(user.UserID, model.Password))
                {
                    Success("重设密码成功，请用新密码登录！");
                }
                else
                {
                    Error("重置密码失败了，请检查输入！");
                }
            }
            return View();
        }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
    }
}