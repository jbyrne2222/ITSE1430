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

namespace JonathanByrne.MovieLib.Data.Memory
{
    /// <summary>Provides an in-memory movie database.</summary>
    public class MemoryMovieDatabase : MovieDatabase
    {
        /// <summary>Adds a movie to the list</summary>
        /// <param name="movie">The movie being added</param>
        /// <returns>The movie.</returns>
        protected override Movie AddCore ( Movie movie )
        {
            //clone the movie
            movie.Id = _nextId++;
            _movies.Add(Clone(movie));

            // Return a copy
            return movie;
        }

        /// <summary>Updates existing movie in list</summary>
        /// <param name="movie">The movie being updated.</param>
        /// <returns>The updated movie.</returns>
        protected override Movie UpdateCore ( Movie movie )
        {
            var existing = GetCore(movie.Id);

            //Clone the movie
            Copy(existing, movie);

            //return a copy
            return movie;
        }

        /// <summary>Gets all the movies</summary>
        /// <returns>A list of movies.</returns>
        protected override IEnumerable<Movie> GetAllCore()
        {
            foreach (var movie in _movies)
            {
                if (movie != null)
                    yield return Clone(movie);
            };
        }

        /// <summary>Gets movie by ID </summary>
        /// <param name="id">Id</param>
        /// <returns>null</returns>
        protected override Movie GetCore( int id )
        {
            foreach (var movie in _movies)
            {
                if (movie.Id == id)
                    return movie;
            };

            return null;
        }

        /// <summary>Gets movie by title.</summary>
        /// <param name="name">movie title</param>
        /// <returns>null</returns>
        protected override Movie GetMovieByNameCore( string name )
        {
            foreach (var movie in _movies)
            {
                if (String.Compare(movie.Title, name, true) == 0)
                    return movie;
            };

            return null;
        }

        /// <summary>Removes a movie.</summary>
        /// <param name="id">The movie Id.</param>
        protected override void RemoveCore (int id)
        {
            var existing = GetCore(id);
            if (existing != null)
                _movies.Remove(existing);
        }

        /// <summary>Creates a cloned copy of a movie.</summary>
        /// <param name="item">The movie being cloned.</param>
        /// <returns>A new copy of a movie.</returns>
        private Movie Clone ( Movie item )
        {
            var newMovie = new Movie();
            Copy(newMovie, item);

            return newMovie;
        }

        /// <summary>Makes a copy of the movie.</summary>
        /// <param name="target">New copy of movie.</param>
        /// <param name="source">Movie being copied.</param>
        private void Copy (Movie target, Movie source)
        {
            target.Id = source.Id;
            target.Title = source.Title;
            target.Description = source.Description;
            target.Length = source.Length;
            target.IsOwned = source.IsOwned;
        }

        private readonly List<Movie> _movies = new List<Movie>();
        private int _nextId = 1;
    }
}
