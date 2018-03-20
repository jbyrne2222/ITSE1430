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

            // Verify unique product
            var existing = GetProductByNameCore(product.Name);
            if(existing != null)
            {
                message = "Product already exists";
                return null;
            };

            message = null;
            return AddCore(product);
        }

        public Product Update( Product product, out string message )
        {
            message = "";

            //Check for null
            if (product == null)
            {
                message = "Product cannot be null.";
                return null;
            };

            //Validate product using IValidateableObject
            var errors = ObjectValidator.Validate(product);
            if (errors.Count() > 0)
            {
                message = errors.ElementAt(0).ErrorMessage;
                return null;
            };

            // Verify unique product
            var existing = GetProductByNameCore(product.Name);
            if (existing != null && existing.Id != product.Id)
            {
                message = "Product already exists";
                return null;
            }

            //Find existing
            existing = existing ?? GetCore(product.Id);
            if (existing== null)
            {
                message = "Product not found.";
                return null;
            };

            return UpdateCore(product);
        }

        /// <summary>Gets all the products.</summary>
        /// <returns>The list of products.</returns>
        public IEnumerable<Product> GetAll()
        {
            return GetAllCore();
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
            if (id > 0)
            {
                RemoveCore(id);
            };
        }

        protected abstract Product AddCore( Product product );
        protected abstract IEnumerable<Product> GetAllCore();
        protected abstract Product GetCore( int id );
        protected abstract Product UpdateCore( Product product );
        protected abstract void RemoveCore( int id );
        protected abstract Product GetProductByNameCore( string name );
    }
}
