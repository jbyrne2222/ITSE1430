using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Data.Memory
{
    /// <summary>Provides an in-memory product database.</summary>
    public class MemoryProductDatabase
    {
        public MemoryProductDatabase()
        {
            //Array version
            //var prods = new Product[]
            //var prods = new []
            //    {
            //        new Product(),
            //        new Product()
            //    };
            
            //_products = new Product[25];
            _products = new List<Product>() 
            {
                new Product() { Id = _nextId++, Name = "iPhone X",
                                IsDiscontinued = true, Price = 1500, },

                new Product() { Id = _nextId++, Name = "Windows Phone",
                                IsDiscontinued = true, Price = 15, },

                new Product() { Id = _nextId++, Name = "Samsung S8",
                                IsDiscontinued = false, Price = 800, },
            };

            //var product = new Product() {
            //    Id = _nextId++,
            //    Name = "iPhone X",
            //    IsDiscontinued = true,
            //    Price = 1500,
            //};
            //_products.Add(product);

            //product = new Product() {
            //    Id = _nextId++,
            //    Name = "Windows Phone",
            //    IsDiscontinued = true,
            //    Price = 15,
            //};
            //_products.Add(product);

            //product = new Product() {
            //    Id = _nextId++,
            //    Name = "Samsung S8",
            //    IsDiscontinued = false,
            //    Price = 800,
            //};
            //_products.Add(product);
        }

        public Product Add ( Product product, out string message )
        {
            if (product == null)
            {
                message = "Product cannot be null.";
                return null;
            };

            //Validate product
            //var error = product.Validate();
            var errors = ObjectValidator.Validate(product);

            if(errors.Count() > 0)
            {
                message = errors.ElementAt(0).ErrorMessage;
                return null;
            };

            // TODO: Verify unique product

            //Add
           //var index = FindEmptyProductIndex();
           //if (index < 0)
           //{
           //    message = "Out of memory";
           //    return null;
           //};

            //clone the object
            product.Id = _nextId++;
            _products.Add(Clone(product));
            message = null;

            // Return a copy
            return product;
        }

        public Product Edit( Product product, out string message )
        {
            if (product == null)
            {
                message = "Product cannot be null.";
                return null;
            };

            //Validate product
            var error = product.Validate();
            if (!String.IsNullOrEmpty(error))
            {
                message = error;
                return null;
            };

            // TODO: Verify unique product except current product

            //Find existing
            var existing = GetById(product.Id);
            if (existing== null)
            {
                message = "Product not found.";
                return null;
            };

            //Clone the object
            //_products[existingIndex] = Clone(product);
            Copy(existing, product);
            message = null;

            //Return a copy
            return product;
        }

        public Product[] GetAll ()
        {
            //Return a copy so caller cannot change the underlying data
            var items = new List<Product>();
            //for (var index = 0; index < _products.Length; ++index)
            foreach ( var product in _products)
            {
                if (product != null)
                    items.Add(Clone(product));
            }

            return items.ToArray();
        }

        public void Remove (int id)
        {
            if (id > 0)
            {
                var existing = GetById(id);
                if (existing != null)
                    _products.Remove(existing);
            };
        }

        //Clone a product
        private Product Clone (Product item)
        {
            var newProduct = new Product();
            Copy(newProduct, item);

            return newProduct;
        }

        private void Copy (Product target, Product source)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            target.Description = source.Description;
            target.Price = source.Price;
            target.IsDiscontinued = source.IsDiscontinued;
        }

        //private int FindEmptyProductIndex()
        //{
        //    for (var index = 0; index < _products.Length; ++index)
        //    {
        //        if (_products[index] == null)
        //            return index;
        //    };
        //
        //    return -1;
        //}

        private Product GetById (int id)
        {
            //for (var index = 0; index < _products.Length; ++index)
            foreach ( var product in _products)
            {
                if (product.Id == id)
                    return product;
            };

            return null;
        }

        private readonly List<Product> _products = new List<Product>();
        private int _nextId = 1;
    }
}
