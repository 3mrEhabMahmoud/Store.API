using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Domain.Entities.Products;
using Store.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Persistence
{
    public class Dblnitializer(StoreDbContext _context) : IDblnitializer
    {
        public async Task InitializeAsync()
        {
            //create Db
            //update Db
            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _context.Database.MigrateAsync();

            }
            //Data seeding

            if(!_context.ProductBrands.Any())
            { 
            //ProductBrands

            //1. Real All Data From Json File 'brands.json'
            //C:\progict\Ass C#\Store\Infrastructure\Store.Persistence\Data\DataSeeding\brands.json
            var brandsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Persistence\Data\DataSeeding\brands.json");

            //2. convert the JsonString To List<ProductBrand>

            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsdata);

            //3.Add List To the Db
            if (brands is not null && brands.Count > 0)
            {
                await _context.ProductBrands.AddRangeAsync(brands);
            }
        }

            //productTypes
            if (!_context.ProductTypes.Any())
            {
                //ProductBrands

                //1. Real All Data From Json File 'brands.json'
                //C:\progict\Ass C#\Store\Infrastructure\Store.Persistence\Data\DataSeeding\brands.json
                var typesdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Persistence\Data\DataSeeding\types.json");

                //2. convert the JsonString To List<ProductTypes>

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);

                //3.Add List To the Db
                if (types is not null && types.Count > 0)
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                }
            }

            //product
            if (!_context.Products.Any())
            {
                //ProductBrands

                //1. Real All Data From Json File 'brands.json'
                //C:\progict\Ass C#\Store\Infrastructure\Store.Persistence\Data\DataSeeding\brands.json
                var Productsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Persistence\Data\DataSeeding\Products.json");

                //2. convert the JsonString To List<Product>

                var Products = JsonSerializer.Deserialize<List<Product>>(Productsdata);

                //3.Add List To the Db
                if (Products is not null && Products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(Products);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
