using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honshu.Data.Entity;
using Honshu.DataAccess;
using Nest;

namespace Honshu.AsynElasticSearch
{
    public class AsyncShop
    {
        private readonly ElasticClient _client;

        public AsyncShop()
        {
            _client = new ElasticClientWrapper().GetClient();
        }

        public void Asyn()
        {
            var list = ShopDataAccess.GetTopShopsToElasticSearch();
            InsertDocuments(list);

            ShopDataAccess.UpdateProductAsyncDate();
        }

        private void InsertDocuments(List<ShopES> list)
        {
            var descriptor = new BulkDescriptor();

            foreach (var item in list)
            {
                descriptor.Index<ShopES>(op => op
                    .Document(item)
                );
            }

            var result = _client.Bulk(descriptor);
        }
    }
}
