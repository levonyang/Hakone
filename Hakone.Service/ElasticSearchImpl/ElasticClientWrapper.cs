using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace Hakone.Service
{
    public class ElasticClientWrapper : ElasticClient
    {
        public string DefaultIndexName = "haodian8";
        public ElasticClient GetClient()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node).DefaultIndex(DefaultIndexName);
            return new ElasticClient(settings);
        }
    }
}
