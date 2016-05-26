using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Honshu.Cube;
using Honshu.Data.Entity;
using Honshu.Data.Enum;

namespace Honshu.Fetcher
{
    public class TmallFetcher : IProductFetcher
    {
        public List<Product> GetProducts(string shopUrl)
        {
            var result = new List<Product>();
            var httpResult = HttpHelper.GetHtml(string.Format("{0}/search.htm", shopUrl));
            if (httpResult.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(httpResult.Html)) return result;

            var domT = CsQuery.CQ.Create(httpResult.Html);
            var input = domT["#J_ShopAsynSearchURL"];
            var asynUrl = input.Val();
            if (string.IsNullOrEmpty(asynUrl)) return result;
            result = GetProductList(shopUrl, asynUrl, Enums.OrderType.hotsell_desc.ToString());
            result.AddRange(GetProductList(shopUrl, asynUrl, Enums.OrderType.newOn_desc.ToString()));

            return result;
        }

        private static List<Product> GetProductList(string shopUrl, string asynUrl, string orderType)
        {
            var result = new List<Product>();
            for (var page = 1; page < 8; page++)
            {
                if (result.Count > 300) break;//有300个商品的话就不要再获取了
                var listHtml =
                    HttpHelper.GetHtml(
                        string.Format("{0}{1}&{2}={3}&orderType={4}&scene=taobao_shop", shopUrl, asynUrl, "pageNo", page, orderType)
                            .Replace("&amp;", "&")).Html;

                if (string.IsNullOrEmpty(listHtml)) break;
                if (listHtml.Contains("没找到符合条件的商品,换个条件或关键词试试吧") || listHtml.Contains("没有对应的宝贝") || listHtml.Contains("请输一下校验码") ||
                    listHtml.Contains("亲，慢慢来，请先坐下来喝口水")) break;

                listHtml = listHtml.Trim().Trim('(').Trim(')').Trim('"').Replace("\\\"", "\"");
                var dom = CsQuery.CQ.Create(listHtml);

                var items = dom[".J_TItems dl.item"];
                foreach (var item in items)
                {
                    var e = item.Cq();
                    var price = e.Find("span.c-price")[0].InnerText.Trim().TryDecimalParse();
                    if (price < 3 || price > 50000) continue;

                    //try
                    //{
                        var product = new Product
                        {
                            ProductIndex = e[0].GetAttribute("data-id").TryLongParse(),
                            Photo = e.Find("dt.photo").Find("img")[0].GetAttribute("data-ks-lazyload"),
                            ProductName = e.Find("dt.photo").Find("img")[0].GetAttribute("alt"),
                            Price = e.Find("span.c-price")[0].InnerText.Trim().TryDecimalParse(),
                            AmountSales =
                                e.Find("span.sale-num").Any()
                                    ? e.Find("span.sale-num")[0].InnerText.Trim().TryIntParse()
                                    : 0,
                            OrderType = orderType
                        };

                        if (!product.ProductName.IsNormalProductName()) continue;

                        result.Add(product);
                    //}
                    //catch (Exception ex)
                    //{
                    //    // ignored
                    //}
                }

                if (!dom["a.ui-page-s-next"].Any()) break;
            }

            return result;
        }
    }
}
