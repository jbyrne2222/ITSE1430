using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Data
{
    public static class ProductDatabaseExtensions
    {
        public static void Seed ( this IProductDatabase source)
        {
            source.Add(new Product() {
                Name = "iPhone X",
                IsDiscontinued = true,
                Price = 1500, });

            source.Add(new Product() {
                Name = "Windows Phone",
                IsDiscontinued = true,
                Price = 15, });

            source.Add(new Product() {
                Name = "Samsung S8",
                IsDiscontinued = false,
                Price = 800, });
        }
    }
}
