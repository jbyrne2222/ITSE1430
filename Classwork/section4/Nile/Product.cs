/*
 * Jonathan Byrne
 * ITSE 1430
 * Section 2 classwork
 */

 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile
{
    /// <summary>Provides information about a product.</summary>
    public class Product : IValidatableObject
    {
        /// <summary>Gets or sets the product ID.</summary>
        public int Id { get; set; }

        internal decimal DiscountPercentage = 0.10M;

        /// <summary>Gets or sets the name.</summary>
        /// <value></value>
        public string Name
        {
            get { return _name ?? ""; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description ?? ""; }
            set { _description = value?? ""; }
        }

        //Using auto property here
        public decimal Price
        {
            //get { return _price; }
            //set { _price = value; }
            get; set;
        } = 0;

        //public int ShowingOffAcessibility
        //{
        //    get { }
        //    internal set { }
        //}

        public decimal ActualPrice
        {
            get 
            {
                if (IsDiscontinued)
                    return Price - (Price * DiscountPercentage);

                return Price;
            }

            //set { }
        }

        public bool IsDiscontinued { get; set; }
        //{
        //    get { return _isDiscontinued; }
        //    set { _isDiscontinued = value; }
        //}

        /// <summary>Get the product name.</summary>
        /// <returns>The name.</returns>
        //public string GetName ()
        //{
        //    return _name ?? "";
        //}

        /// <summary>Sets the product name.</summary>
        /// <param name="value">The name.</param>
        //public void SetName (string value)
        //{
        //    _name = value ?? "";
        //}

        /// <summary>Validates the product.</summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation results.</returns>
        public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            var errors = new List<ValidationResult>();

            // Name is required
            if (String.IsNullOrEmpty(_name))
                errors.Add(new ValidationResult("Name cannot be empty",
                             new[] { nameof(Name) }));

            //Price >= 0
            if (Price < 0)
                errors.Add(new ValidationResult("Price must be >= 0",
                             new[] { nameof(Price) }));

            return errors;
        }

        private string _name;
        private string _description;
        //private decimal _price;
        //private bool _isDiscontinued;
    }
}
