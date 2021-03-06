﻿/*
 * Jonathan Byrne
 * ITSE 1430
 * Lab3
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonathanByrne.MovieLib
{
    /// <summary>Provides support for validating data</summary>
    public static class ObjectValidator
    {
        /// <summary>Validates an object and all properties</summary>
        /// <param name="source">The object to validate.</param>
        /// <returns>The validation results.</returns>
        public static IEnumerable<ValidationResult> Validate ( this IValidatableObject source)
        {
            var context = new ValidationContext(source);
            var errors = new Collection<ValidationResult>();
            var results = Validator.TryValidateObject(source, context, errors, true);

            return errors;

        }
    }
}
