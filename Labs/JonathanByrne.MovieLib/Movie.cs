/*
 * Jonathan Byrne
 * ITSE 1430
 * Lab3
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonathanByrne.MovieLib
{
    /// <summary>Provides information about a movie</summary>
    public class Movie : IValidatableObject
    {
        /// <summary>Gets or sets the movie ID. </summary>
        public int Id { get; set;}

        /// <summary>Gets or sets the movie title</summary>
        public string Title
        {
            get { return _title ?? ""; }
            set { _title = value; }
        }

        /// <summary>Gets or sets the movie description (optional)</summary>
        public string Description
        {
            get { return _description ?? ""; }
            set { _description = value ?? ""; }
        }

        /// <summary>Gets or sets the movie length in minutes (defaults to 0)</summary>
        public int Length
        {
            get; set;
        } = 0;

        /// <summary>Gets or sets if the movie is owned</summary>
        public bool IsOwned
        {
            get; set;
        }

        /// <summary>Validates the movie.</summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation results.</returns>
        public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            var errors = new List<ValidationResult>();

            // Name is required
            if (String.IsNullOrEmpty(_title))
                errors.Add(new ValidationResult("Title cannot be empty",
                             new[] { nameof(Title) }));

            //Length >= 0
            if (Length < 0)
                errors.Add(new ValidationResult("Length must be >= 0",
                             new[] { nameof(Length) }));

            return errors;
        }

        private string _title;
        private string _description;
    }
}
