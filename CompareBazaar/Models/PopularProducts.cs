using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareBazaar.Models
{
    public class PopularProducts
    {

        public string Id { get; set; }
        public int ProductId { get; set; }
        public string Vendor { get; set; }

        public int Value { get; set; }
    }
}
