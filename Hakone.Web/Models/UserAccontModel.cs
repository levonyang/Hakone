using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hakone.Domain;

namespace Hakone.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "请输入用户名")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住登录状态?")]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        [Remote("CheckPassword", "Account", ErrorMessage = "当前密码输入错误")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请输入新密码")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public class EditUserInfoModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "请输入正确的email")]
        [Remote("CheckEmail", "Account", ErrorMessage = "该邮箱已经存在")]
        public string Email { get; set; }

        public int? BornYear { get; set; }
        public int? BornMonth { get; set; }
        public int? BornDay { get; set; }
        public bool? Gender { get; set; }
        public string GenderName { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Remote("CheckUserName", "Account", ErrorMessage = "该用户名已经存在")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "请输入正确的email")]
        [Remote("CheckEmail", "Account", ErrorMessage = "该邮箱已经存在")]
        public string Email { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20, ErrorMessage = "{0}由6到20个字符或数字组成。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }

    public class ResetPasswordRequestModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "请输入正确的email")]
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20, ErrorMessage = "{0}由6到20个字符或数字组成。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        public string Guid { get; set; }
    }
}