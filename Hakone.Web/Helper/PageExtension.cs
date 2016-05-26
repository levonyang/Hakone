using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain.Enum;
using Hakone.Cube;

namespace Hakone.Web
{
    public static class PageExtension
    {
        public static string AvatarHandler(this string avatar, int id)
        {
            if (!string.IsNullOrEmpty(avatar)) return avatar;

            return string.Format("/image/avatar/scenery-{0}.png", id.GetNumberFromNumber());
        }

        public static int GetNumberFromNumber(this int number)
        {
            var result = "0";

            if (number < 25)
            {
                result = number.ToString();
            }
            else
            {
                var lt = number.ToString().Substring(number.ToString().Length - 2, 2);
                result = lt.ToInt32() < 25 ? lt.ToInt32().ToString() : number.ToString().Substring(number.ToString().Length - 1, 1);

                if (result == "0") result = new Random().Next(1, 24).ToString();
            }

            return Convert.ToInt32(result);
        }

        public static string CommentFlagHandler(this int commentFlag)
        {
            var flag = commentFlag.ToEnum<ShopCommentFlag>();
            switch (flag)
            {
                case ShopCommentFlag.CollectShop:
                    return "推荐了该店铺";
                case ShopCommentFlag.HotShop:
                    return "推荐为热门店铺";
                case ShopCommentFlag.SelectedShop:
                    return "推荐为精选店铺";
            }

            return "评价了该店铺";
        }
    }
}