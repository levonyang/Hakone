using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Cube;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Service;
using MvcPaging;

namespace Hakone.Service
{
    public class ShopCommentService : GenericController<ShopComment, Hakone.Domain.HakoneDBDataContext>,IShopCommentService
    {
        public void AddComment(int userId, int shopId, string comment, int commentFlag)
        {
            if (!comment.IsNotNullOrEmpty()) return;

            var entity = new ShopComment
            {
                UserId = userId,
                ShopID = shopId,
                CommentFlag = commentFlag,
                CommentContent = comment,
                EntryDate = DateTime.Now,
            };

            Insert(entity, true);
        }

        public ShopCommentInfo GetShopComment(int shopId, int commentFlag = 1)
        {
//            if (commentFlag == 0) commentFlag = 1;

//            const string sql = @"Select Top 1 U.UserName, U.UserId, U.Avatar, S.CommentFlag, S.CommentContent From ShopComment S Inner Join [User] U On S.UserId = U.UserID
//                        Where S.ShopID = @ShopID And S.CommentFlag = @CommentFlag
//                        Order By S.EntryDate Desc";

//            var result = DefaultContext.Sql(sql)
//                .Parameter("ShopID", shopId)
//                .Parameter("CommentFlag", commentFlag)
//                .QuerySingle<ShopCommentInfo>();

//            if (result.IsNull())
//            {
//                result = DefaultContext.Sql(sql)
//                .Parameter("ShopID", shopId)
//                .Parameter("CommentFlag", 1)
//                .QuerySingle<ShopCommentInfo>();
//            }

            var u = FakeUser.GetList().FirstOrDefault(r => r.UserId == commentFlag);

            const string sql =
                @"Select N'' As UserName, 18 As UserId, Null As Avatar, 5 As CommentFlag, ShortDesc As CommentContent From Shops Where ID = @ShopID";
            var result = DefaultContext.Sql(sql)
                .Parameter("ShopID", shopId)
                .QuerySingle<ShopCommentInfo>();
            result.UserName = u.UserName;
            result.UserId = u.UserId;

            return result;
        }


        public MvcPaging.IPagedList<ShopCommentInfo> GetList(int shopId, int page)
        {
            const string sql = @"Select U.UserName, U.UserId, U.Avatar, S.CommentFlag, S.CommentContent From ShopComment S Inner Join [User] U On S.UserId = U.UserID
                        Where S.ShopID = @ShopID
                        Order By S.EntryDate desc";

            var result = DefaultContext.Sql(sql)
                .Parameter("ShopID", shopId)
                .QueryMany<ShopCommentInfo>();

            return result.ToPagedList(page, 30);
        }
    };
}
