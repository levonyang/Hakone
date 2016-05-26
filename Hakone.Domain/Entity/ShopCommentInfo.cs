using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Domain
{
    public class ShopCommentInfo
    {
        public string Avatar { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public string CommentContent { get; set; }

        public int CommentFlag { get; set; }
    }
}
