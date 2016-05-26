using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honshu.Data.Entity;

namespace Honshu.Fetcher
{
    public interface IProductFetcher
    {
        List<Product> GetProducts(string shopUrl);
    }
}
