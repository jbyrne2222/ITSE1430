using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Data
{
    /// <summary>Provides an in-memory product database.</summary>
    public abstract class ProductDatabase : IProductDatabase
    {

        public Product Add ( Product product )
        {
            //Check for null
            //if (product == null)
                //throw new ArgumentNullException(nameof(product));
            product = product ?? throw new ArgumentNullException(nameof(product));

            //Validate product
            product.Validate();
            //var errors = product.TryValidate();
            //var error = errors.FirstOrDefault();
            //if (error != null)
            //{
            //    message = error.ErrorMessage;
            //    return null;
            //};

            // Verify unique product
            var existing = GetProductByNameCore(product.Name);
            if (existing != null)
                throw new Exception("Product already exists");
            //{
            //    message = "Product already exists";
            //    return null;
            //};

            return AddCore(product);
        }

        public Product Update( Product product )
        {

            //Check for null
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            //Validate product using IValidateableObject
            product.Validate();
            //var errors = ObjectValidator.TryValidate(product);
            //if (errors.Count() > 0)
            //{
            //    message = errors.ElementAt(0).ErrorMessage;
            //    return null;
            //};

            // Verify unique product
            var existing = GetProductByNameCore(product.Name);
            if (existing != null && existing.Id != product.Id)
                throw new Exception("Product already exists");
            //{
            //    message = "Product already exists";
            //    return null;
            //}

            //Find existing
            existing = existing ?? GetCore(product.Id);
            if (existing == null)
                throw new ArgumentException("Product not found", nameof(product));
            //{
            //    message = "Product not found.";
            //    return null;
            //};

            return UpdateCore(product);
        }

        /// <summary>Gets all the products.</summary>
        /// <returns>The list of products.</returns>
        public IEnumerable<Product> GetAll()
        {
            // Option 2 - extension
            //return GetAllCore()
            //    .OrderBy(p => p.Name)
            //    .ThenByDescending(p => p.Id)
            //    .Select(p => p);

            // Option 1 - LINQ
            return from p in GetAllCore()
                   orderby p.Name, p.Id descending
                   select p;
        }

        //public IEnumerable<Product> GetAll ()
        //{
        //    //Return a copy so caller cannot change the underlying data
        //    var items = new List<Product>();
        //
        //    //for (var index = 0; index < _products.Length; ++index)
        //    foreach ( var product in _products)
        //    {
        //        if (product != null)
        //            items.Add(Clone(product));
        //    };
        //
        //    return items;
        //}
        //

        /// <summary>Removes a product. </summary>
        /// <param name="id">The product ID.</param>
        public void Remove (int id)
        {
            //Return an error if id <= 0
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be > 0");

            //if (id > 0)
            //{
                RemoveCore(id);
            //};
        }

        protected abstract Product AddCore( Product product );
        protected abstract IEnumerable<Product> GetAllCore();
        protected abstract Product GetCore( int id );
        protected abstract Product UpdateCore( Product product );
        protected abstract void RemoveCore( int id );
        protected abstract Product GetProductByNameCore( string name );
    }
}
