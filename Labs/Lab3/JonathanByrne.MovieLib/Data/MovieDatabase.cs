/*
 * Jonathan Byrne
 * ITSE 1430
 * Lab3
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonathanByrne.MovieLib
{
    /// <summary>Provides an in-memory movie database.</summary>
    public abstract class MovieDatabase : IMovieDatabase
    {
        /// <summary>Adds a movie to the database.</summary>
        /// <param name="movie">The movie being added.</param>
        /// <param name="message">The message out to the user.</param>
        /// <returns>The newly added movie. If the method fails, then return null.</returns>
        public Movie Add( Movie movie, out string message )
        {
            if (movie == null)
            {
                message = "Movie cannot be null";
                return null;
            };

            //Validate movie
            var errors = movie.Validate();

            var error = errors.FirstOrDefault();
            if (error != null)
            {
                message = error.ErrorMessage;
                return null;
            }

            //Verify unique movie
            var existing = GetMovieByNameCore(movie.Title);
            if(existing != null)
            {
                message = "Movie is already in database";
                return null;
            };

            message = null;
            return AddCore(movie);
        }

        /// <summary>Gets a specific movie from the database.</summary>
        /// <param name="id">Unique Id of the movie.</param>
        /// <param name="message"></param>
        /// <returns>The movie with the given Id. If no movie exists, return null.</returns>
        public Movie Get( int id )
        {
            return GetCore(id);
        }

        /// <summary>Gets all movies from database.</summary>
        /// <returns>An enumerable list of movies</returns>
        public IEnumerable<Movie> GetAll()
        {
            return GetAllCore();
        }

        /// <summary>Removes the specified movie from the database.</summary>
        /// <param name="id">The unique Id of the movie.</param>
        public void Remove( int id )
        {
            if(id > 0)
            {
                RemoveCore(id);
            }
        }

        /// <summary>Updates movies in database</summary>
        /// <param name="movie">The updated movie information.</param>
        /// <param name="message">The message out to the user.</param>
        /// <returns>The updated movie.</returns>
        public Movie Update( Movie movie, out string message )
        {
            message = "";

            //Check for null
            if (movie == null)
            {
                message = "Product cannot be null.";
                return null;
            };

            //Validate movie using IValidateableObject
            var errors = ObjectValidator.Validate(movie);
            if (errors.Count() > 0)
            {
                message = errors.ElementAt(0).ErrorMessage;
                return null;
            };

            // Verify unique movie
            var existing = GetMovieByNameCore(movie.Title);
            if (existing != null && existing.Id != movie.Id)
            {
                message = "Movie already exists in database";
                return null;
            }

            //Find existing
            existing = existing ?? GetCore(movie.Id);
            if (existing == null)
            {
                message = "Product not found.";
                return null;
            };

            return UpdateCore(movie);
        }

        protected abstract Movie AddCore( Movie movie );
        protected abstract Movie GetCore( int id );
        protected abstract Movie UpdateCore( Movie movie );
        protected abstract Movie GetMovieByNameCore( string name );
        protected abstract void RemoveCore( int id );
        protected abstract IEnumerable<Movie> GetAllCore();
    }
}
