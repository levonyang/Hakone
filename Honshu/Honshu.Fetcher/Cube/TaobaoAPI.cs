using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using Honshu.Cube;

namespace Honshu.Fetcher.Cube
{
    public class TaobaoAPI
    {
        public static Product GetProduct(string itemId)
        {
            ITopClient client = new DefaultTopClient(TaobaoUrl, AppKey, AppSecret);
            ProductGetRequest req = new ProductGetRequest();
            req.Fields = @"name,cat_name,price,pic_url,sale_num,vertical_market,sell_pt";
            req.ProductId = itemId.TryLongParse();
            ProductGetResponse response = client.Execute(req);

            return response.Product;
        }

        public static Item GetItem(string iid)
        {
            ITopClient client = new DefaultTopClient(TaobaoUrl, AppKey, AppSecret);
            var request = new ItemGetRequest
            {
                Fields = @"nick,pic_url,detail_url,title,price,express_fee",
                NumIid = iid.TryLongParse() 
            };

            var response = client.Execute(request);
            if (response.IsError)
            {
                return null;
            }

            return response.Item;
        }

        #region 属性

        private static string TaobaoUrl
        {
            get { return "http://gw.api.taobao.com/router/rest"; }
        }

        private static string AppKey
        {
            get { return ConfigurationManager.AppSettings["appKey"]; }
        }

        private static string AppSecret
        {
            get { return ConfigurationManager.AppSettings["appSecret"]; }
        }
        #endregion
    }

}
