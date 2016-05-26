using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Domain.Enum
{
    public enum ProductOrderBy
    {
        ByDefault = 0, //最新编辑推荐（站内推荐）
        ByOrderNumber, //按推广用金额排序
        ByCollectionDate, //最新被收藏
        UpdateDate,
        EntryDate
    }
}
