using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Products;
using Store.Services.Abstractions.Products;
using Store.Shard.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Products
{
    public class ProductService(IUnitofWork _unitofWork,IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<ProductResponse>> GetAllProductAsync()
        {
            var products = await _unitofWork.GetRepository<int, Product>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return result;
        }
        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _unitofWork.GetRepository<int, ProductType>().GetAsync(id);
            var result = _mapper.Map<IEnumerable<ProductResponse>>(product);
            return result;
        }
        public async Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync()
        {
            var brands = await _unitofWork.GetRepository<int, ProductBrand>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTypeResponse>>(brands);
            return result;
        }


        public async Task<IEnumerable<BrandTypeResponse>> GetAllTypesAsync()
        {
            var types = await _unitofWork.GetRepository<int, ProductType>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTypeResponse>>(types);
            return result;
        }

    }
}
