using AutoMapper;
using Store.Domain.Entities.Products;
using Store.Shard.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Mapping.Products
{
    public class ProductProfile :Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(D => D.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(D => D.Type, o => o.MapFrom(s => s.Type.Name));


            CreateMap<ProductBrand, BrandTypeResponse>();
            CreateMap<ProductType, BrandTypeResponse>();
        }
    }
}
