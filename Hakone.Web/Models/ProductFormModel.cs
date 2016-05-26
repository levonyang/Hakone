using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hakone.Web.Models
{
    public class ProductFormModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "商品名称不能为空")]
        [Display(Name = "商品名称")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "商品类别不能为空")]
        [Display(Name = "商品类别")]
        public int CatId { get; set; }

        [Required(ErrorMessage = "商品价格不能为空")]
        [Display(Name = "价格")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "商品销量不能为空")]
        [Display(Name = "销量")]
        public int AmountSales { get; set; }
    }
}