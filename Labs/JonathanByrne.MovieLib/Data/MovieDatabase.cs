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

namespace JonathanByrne.MovieLib.Data
{
    /// <summary>Provides an in-memory movie database.</summary>
    public abstract class MovieDatabase : IMovieDatabase
    {
        /// <summary>Adds a movie to the database.</summary>
        /// <param name="movie">The movie being added.</param>
        /// <param name="message">The message out to the user.</param>
        /// <returns>The newly added movie. If the method fails, then return null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="movie"/> is null.</exception>
        /// <exception cref="ValidationException"><paramref name="movie"/> is invalid.</exception>
        /// <exception cref="Exception">A movie with the same name already exists.</exception>
        /// <remarks>
        /// Returns an error if the movie is null, invalid or if a movie
        /// with the same title already exists.
        /// </remarks>
        public Movie Add( Movie movie )
        {
            movie = movie ?? throw new ArgumentNullException(nameof(movie));

            //Validate movie
            movie.Validate();           

            //Verify unique movie
            var existing = GetMovieByNameCore(movie.Title);
            if (existing != null)
                throw new Exception("Movie already exists.");

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
            return from m in GetAllCore()
                   orderby m.Title, m.Id descending
                   select m;
        }

        /// <summary>Removes the specified movie from the database.</summary>
        /// <param name="id">The unique Id of the movie.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than or equal to zero.</exception>
        public void Remove( int id )
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be > 0");

            RemoveCore(id);
        }

        /// <summary>Updates movies in database</summary>
        /// <param name="movie">The updated movie information.</param>
        /// <param name="message">The message out to the user.</param>
        /// <returns>The updated movie.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="movie"/> is null.</exception>
        /// <exception cref="ValidationException"><paramref name="movie"/> is invalid.</exception>
        /// <exception cref="Exception">A movie with the same name already exists.</exception>
        /// <exception cref="ArgumentException">The movie does not exist.</exception>
        /// <remarks>
        /// Returns an error if movie is null, invalid, movie title
        /// already exists or if the movie cannot be found.
        /// </remarks>
        public Movie Update( Movie movie )
        {
            //Check for null
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            //Verify movie
            movie.Validate();

            //Verify unique movie
            var existing = GetMovieByNameCore(movie.Title);
            if (existing != null && existing.Id != movie.Id)
                throw new Exception("Movie already exists");

            //Find existing
            existing = existing ?? GetCore(movie.Id);
            if (existing == null)
                throw new ArgumentException("Movie not found", nameof(movie));

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
