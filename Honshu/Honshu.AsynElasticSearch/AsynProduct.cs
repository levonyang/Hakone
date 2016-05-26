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
    public class AsynProduct
    {
        private readonly ElasticClient _client;

        public AsynProduct()
        {
            _client = new ElasticClientWrapper().GetClient();
        }
        public void Asyn()
        {
            var products = ProductDataAccess.GetTopProductsToElasticSearch();
            InsertDocuments(products);

            ProductDataAccess.UpdateProductAsyncDate();
        }

        public void AsyncProductByShopId(int shopId)
        {
            DeleteDocumentsByShopId(shopId);
            var products = ProductDataAccess.GetProductsByShopId(shopId);
            InsertDocuments(products);
        }

        public void DeleteDocumentsByShopId(int shopId)
        {
            var docs = _client.Search<ProductES>(q => q
                .From(0)
                .Size(500)
                .Type(typeof(ProductES))
                .Query(f => f.Term(e => e.ShopId, shopId)));

            if (docs.Documents.Any())
            {
                _client.DeleteMany<ProductES>(docs.Documents);
            }
        }

        private void InsertDocuments(List<ProductES> products)
        {
            var descriptor = new BulkDescriptor();

            foreach (var product in products)
            {
                descriptor.Index<ProductES>(op => op
                    .Document(product)
                );
            }

            var result = _client.Bulk(descriptor);
        }

        public List<ProductES> SearchDocuments(string s)
        {
            //var docs = _client.Search<ProductES>(q => q
            //    .From(0)
            //    .Size(500)
            //    .Query(f => f.Term(e => e.ShopId, shopId)));
            //var n = docs.Documents;

            var request = new SearchRequest
            {
                From = 0,
                Size = 10,
                Query = new TermQuery { Field = "ProductName", Value = s }
                    || new MatchQuery { Field = "description", Query = "nest" }
            };

            return _client.Search<ProductES>(request).Documents.ToList();
        }
    }
}
