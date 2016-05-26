using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Domain.Enum
{
    public enum ShopOrderBy
    {
        ByDefault = 0, //最新编辑推荐(站内推荐)
        ByRecommendDate,//按级别推荐
        ByCollectionDate,//最新被收藏
        UpdateDate,
        EntryDate,
        PromoteAccount,
        PromoteAmount
    }

    public enum ShopProductsOrderBy
    {
        dft =0,
        newup,
        sales
    }
}
