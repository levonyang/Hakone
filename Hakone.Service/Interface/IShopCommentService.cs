using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;
using MvcPaging;

namespace Hakone.Service
{
    public interface IShopCommentService
    {
        void AddComment(int userId, int shopId, string comment, int commentFlag);

        ShopCommentInfo GetShopComment(int shopId, int commentFlag);

        IPagedList<ShopCommentInfo> GetList(int shopId, int page);
    }
}
