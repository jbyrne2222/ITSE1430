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
    /// <summary>The interface for our movie database.</summary>
    public interface IMovieDatabase
    {
        /// <summary>Adds a movie to the database</summary>
        /// <param name="movie">The movie to add.</param>
        /// <param name="message">The message out to the user.</param>
        /// <returns>The added movie.</returns>
        Movie Add( Movie movie );

        /// <summary>Gets a movie from the database by the ID</summary>
        /// <param name="id">The movie ID</param>
        /// <returns>A movie</returns>
        Movie Get( int id );

        /// <summary>Gets all the movies.</summary>
        /// <returns>The list of movies.</returns>
        IEnumerable<Movie> GetAll();

        /// <summary>Removes a movie from the database.</summary>
        /// <param name="id">The ID of the movie.</param>
        void Remove( int id );

        /// <summary>Updates an existing movie in the database.</summary>
        /// <param name="movie">The movie to update.</param>
        /// <param name="message">The message out to the user.</param>
        /// <returns>The updated movie.</returns>
        Movie Update( Movie movie );
    }
}
