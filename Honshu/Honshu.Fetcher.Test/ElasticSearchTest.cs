using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honshu.AsynElasticSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Honshu.Fetcher.Test
{
    [TestClass]
    public class ElasticSearchTest
    {
        [TestMethod]
        public void CreateIndexTest()
        {
            var init = new InitIndex();
            init.CreateIndex();
        }

        [TestMethod]
        public void AsynProductTest()
        {
            var async = new AsynProduct();
            async.Asyn();
        }

        [TestMethod]
        public void DeleteDocumentsByShopIdTest()
        {
            var async = new AsynProduct();
            async.DeleteDocumentsByShopId(3676009);
        }
    }
}
