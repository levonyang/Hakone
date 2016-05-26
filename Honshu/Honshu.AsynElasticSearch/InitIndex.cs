using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honshu.Data.Entity;

namespace Honshu.AsynElasticSearch
{
    public class InitIndex
    {
        public void CreateIndex()
        {
            var client = new ElasticClientWrapper().GetClient();
            try
            {
                client.DeleteIndex("haodian8");
            }
            catch { 
                //todo:
            }
            var createIndexResponse = client.CreateIndex("haodian8", c => c
                .Mappings(ms => ms
                    .Map<ProductES>(m => m.AutoMap())
                )
            );
        }
    }
}
