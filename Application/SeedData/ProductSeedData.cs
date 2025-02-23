using Domain;

using Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SeedData
{
    public class ProductSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Check if data already exists
                if (context.Products.Any())
                {
                    return; // DB has been seeded
                }

                context.Products.AddRange(
                    new Product { Name = "Laptop", Description = "High-end gaming laptop", Price = 150000, ImageUrl = "laptop.jpg", Category = "Electronics" },
                    new Product { Name = "Smartphone", Description = "Latest 5G smartphone", Price = 35000, ImageUrl = "smartphone.jpg", Category = "Electronics" },
                    new Product { Name = "Table", Description = "Wooden dining table", Price = 10000, ImageUrl = "table.jpg", Category = "Furniture" },
                    new Product { Name = "Headphones", Description = "Noise-cancelling headphones", Price = 2500, ImageUrl = "headphones.jpg", Category = "Electronics" }
                );
                context.SaveChanges();
            }
        }
    }
}
