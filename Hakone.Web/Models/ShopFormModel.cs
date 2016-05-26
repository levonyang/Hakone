using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hakone.Domain;

namespace Hakone.Web.Models
{
    public class ShopFormModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "店铺名称不能为空")]
        [Display(Name = "店铺名称")]
        public string ShopName { get; set; }

        [Required(ErrorMessage = "简单描述不能为空")]
        [Display(Name = "简单描述")]
        public string ShortDesc { get; set; }

        [Display(Name = "标签")]
        public string ShopTags { get; set; }

        [Required(ErrorMessage = "店铺类别不能为空")]
        [Display(Name = "店铺类别")]
        public int CatId { get; set; }

        [Required(ErrorMessage = "logo图标不能为空")]
        [Display(Name = "logo图标")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "主营项目不能为空")]
        [Display(Name = "主营项目")]
        public string MainBiz { get; set; }

        [Required(ErrorMessage = "店铺首页地址不能为空")]
        [Display(Name = "首页地址")]
        public string ShopUrl { get; set; }

        [Display(Name = "推广地址")]
        public string PromoteURL { get; set; }
    }
}