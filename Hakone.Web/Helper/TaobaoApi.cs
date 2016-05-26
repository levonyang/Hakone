using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;

namespace Hakone.Web
{
    public class TaobaoApi
    {
        public static NTbkItem GetProduct(long itemId)
        {
            ITopClient client = new DefaultTopClient(url, appkey, secret);
            TbkItemInfoGetRequest req = new TbkItemInfoGetRequest();
            req.Fields = "seller_id,num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,click_url,nick,volume";
            req.Platform = 1L;
            req.NumIids = itemId.ToString();
            TbkItemInfoGetResponse rsp = client.Execute(req);
            return rsp.Results.FirstOrDefault();
        }

        #region 公共属性

        private static string url
        {
            get { return "http://gw.api.taobao.com/router/rest"; }
        }

        private static string appkey
        {
            get { return ConfigurationManager.AppSettings["appKey"]; }
        }

        private static string secret
        {
            get { return ConfigurationManager.AppSettings["appSecret"]; }
        }

        #endregion
    }
}