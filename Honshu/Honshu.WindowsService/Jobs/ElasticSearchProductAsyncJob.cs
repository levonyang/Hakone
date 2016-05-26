using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Honshu.AsynElasticSearch;
using Quartz;

namespace Honshu.WindowsService
{
    public class ElasticSearchProductAsyncJob : IJob
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            Log.Info("同步商品Job开始");

            var async = new AsynProduct();
            async.Asyn();

            Log.Info("同步商品Job结束");
        }
    }
}
