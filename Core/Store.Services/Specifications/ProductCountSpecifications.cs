using Store.Domain.Entities.Products;
using Store.Shard.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications
{
    public class ProductCountSpecifications : BaseSpecifications<int,Product>
    {
        public ProductCountSpecifications(ProductQueryParameters parameters):base(
            p=>
            (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId)
            &&
            (!parameters.TypId.HasValue || p.TypeId == parameters.TypId)
            &&
            (string.IsNullOrEmpty(parameters.Search)|| p.Name.ToLower().Contains(parameters.Search.ToLower()))
            )
        {
            
        }
    }
}
