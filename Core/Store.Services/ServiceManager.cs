using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Store.Domain.Contracts;
using Store.Domain.Entities.Identity;
using Store.Services.Abstractions;
using Store.Services.Abstractions.Auth;
using Store.Services.Abstractions.Cache;
using Store.Services.Abstractions.Products;
using Store.Services.Auth;
using Store.Services.Cache;
using Store.Services.Products;
using Store.Shard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services
{
    public class ServiceManager(IUnitofWork _unitofWork,
        IMapper _mapper,
        IBasketRepository basketRepository,
        ICacheRepository _cacheRepository,
        UserManager<AppUser> _userManager,
        IOptions<JwtOptions> _options) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitofWork, _mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository, _mapper);

        public ICacheService CacheService { get; } = new CacheService(_cacheRepository);
        public IAuthService AuthService { get; } = new AuthService(_userManager, _options);

    }
}