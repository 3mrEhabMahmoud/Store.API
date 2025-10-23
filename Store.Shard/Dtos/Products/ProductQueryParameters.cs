using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Shard.Dtos.Products
{
    public class ProductQueryParameters
    {
        public int? BrandId { get; set; }
        public int? TypId { get; set; }
        public string? Sort { get; set; }
        public string? Search { get; set; }
        public int PageIndex { get; set; } = 5;
        public int Pagesize { get; set; } = 1;

    }
}
