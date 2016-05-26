using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Honshu.Fetcher;
using Quartz;

namespace Honshu.WindowsService
{
    public class TaobaoProductFetcherJob :IJob
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            Log.Info("开始获取店铺商品");

            var fetcher = new Haodian8ProductFetcher();
            var result = fetcher.Fetch();
            //同步商品到ElasticSearch:
            if (result.Item2 > 0)
            {
                var asyncProduct = new AsynElasticSearch.AsynProduct();
                asyncProduct.AsyncProductByShopId(result.Item2);
                Log.Info("同步店铺商品到ElasticSearch完成！");
            }

            Log.Info(result.Item1);
        }
    }
}
