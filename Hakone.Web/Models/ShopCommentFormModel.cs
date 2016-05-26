using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hakone.Web.Models
{
    public class ShopCommentFormModel
    {
        [Required(ErrorMessage = "评论内容不能为空")]
        [Display(Name = "评论内容")]
        public string CommentContent { get; set; }

        public int ShopId { get; set; }
    }
}