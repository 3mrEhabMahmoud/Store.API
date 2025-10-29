using AutoMapper;
using Store.Domain.Contracts;
using Store.Services.Abstractions;
using Store.Services.Abstractions.Products;
using Store.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services
{
    public class ServiceManager(IUnitofWork _unitofWork, IMapper _mapper,IBasketRepository basketRepository) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitofWork, _mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository, _mapper);
    }
}