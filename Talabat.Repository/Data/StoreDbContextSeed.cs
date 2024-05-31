using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;

namespace Talabat.Repository.Data
{
    public class StoreDbContextSeed
    {


        public static async Task SeedAsync(StoreDbContext _Context)
        {


            if (_Context.ProductBrands.Count() == 0)
            {

                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");

                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

                if (Brands?.Count() > 0)
                {
                    foreach (var brand in Brands)
                    {
                        _Context.Set<ProductBrand>().Add(brand);
                    }
                    await _Context.SaveChangesAsync();

                }
            }



            if (_Context.ProductCategories.Count() == 0)
            {
                var CategoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");

                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);

                if (categories?.Count() > 0)
                {
                    foreach (var category in categories)
                    {
                        _Context.Set<ProductCategory>().Add(category);
                    }
                    await _Context.SaveChangesAsync();

                }

            }



            if (_Context.Products.Count() == 0)
            {

                var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                    {
                        _Context.Set<Product>().Add(product);
                    }
                    await _Context.SaveChangesAsync();

                }
            }


            if (_Context.DeliveryMethods.Count() == 0)
            {

                var DeliveryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");

                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);

                if (DeliveryMethods?.Count() > 0)
                {
                    foreach (var delivery in DeliveryMethods)
                    {
                        _Context.Set<DeliveryMethod>().Add(delivery);
                    }
                    await _Context.SaveChangesAsync();

                }
            }
        }
    }
}
