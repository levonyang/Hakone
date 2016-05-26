using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honshu.Fetcher
{
    public class FetcherFactory
    {
        public static IProductFetcher CreateFetcher(string shopUrl)
        {
            if (shopUrl.Contains("tmall")) return new TmallFetcher();

            return new TaobaoFetcher();
        }
    }
}
