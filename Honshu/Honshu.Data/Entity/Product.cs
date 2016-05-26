using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honshu.Data.Entity
{
    public class Product
    {
        public long ProductIndex { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public int AmountSales { get; set; }
        public string OrderType { get; set; }
    }
}
