using Store.Domain.Entities.Products;
using Store.Shard.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications.Products
{
    public class ProductsWithBrandAndTypeSpecifications :BaseSpecifications<int,Product>
    {
        public ProductsWithBrandAndTypeSpecifications(int id):base(p=>p.Id ==id)
        {
            ApplyIncludes();
        }
        public ProductsWithBrandAndTypeSpecifications(ProductQueryParameters parameters) :base
            (
            p=>(!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId)
            &&
            (!parameters.TypId.HasValue || p.TypeId== parameters.TypId) 
            &&
            (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower()))
            )
        {

            //pageIndex =3
            //pagesize =5
            //Skip =2*5
            //Take = 5
            ApplyPagination(parameters.Pagesize, parameters.PageIndex);
            ApplySorting(parameters.Sort);
            ApplyIncludes();

        }
        private void ApplySorting(string? sort)
        {
            //priceasc
            //pricedesc
            //nameasc
            if (!string.IsNullOrEmpty(sort))
            {
                //check value
                switch (sort.ToLower())
                {
                    case "priceasc":
                        //OrderBy(p=>p.Price);
                        AddOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        //orderByDescending(p=>p.Price)
                        AddOrderByDescending(p => p.Name);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                //order by =p=>p.Name
                AddOrderBy(p => p.Name);
            }

        }
        private void ApplyIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);
        }
    }
}
