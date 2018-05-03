/*
 * Jonathan Byrne
 * ITSE 1430
 * Lab3
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonathanByrne.MovieLib.Data.Memory
{
    /// <summary>Provides an in-memory movie database.</summary>
    public class MemoryMovieDatabase : MovieDatabase
    {
        #region Construction

        public MemoryMovieDatabase (string filename)
        {
            _filename = filename;
        }
        #endregion

        /// <summary>Adds a movie to the list</summary>
        /// <param name="movie">The movie being added</param>
        /// <returns>The movie.</returns>
        protected override Movie AddCore ( Movie movie )
        {
            EnsureInitialized();

            movie.Id = _id++;
            _movies.Add(movie);

            SaveData();

            return movie;
        }

        /// <summary>Updates existing movie in list</summary>
        /// <param name="movie">The movie being updated.</param>
        /// <returns>The updated movie.</returns>
        protected override Movie UpdateCore ( Movie movie )
        {
            EnsureInitialized();

            var existing = GetCore(movie.Id);

            _movies.Remove(existing);
            _movies.Add(existing);

            SaveData();

            return movie;
        }

        /// <summary>Gets all the movies</summary>
        /// <returns>A list of movies.</returns>
        protected override IEnumerable<Movie> GetAllCore()
        {
            EnsureInitialized();

            return _movies;
        }

        /// <summary>Gets movie by ID </summary>
        /// <param name="id">Id</param>
        /// <returns>null</returns>
        protected override Movie GetCore( int id )
        {
            EnsureInitialized();

            return _movies.FirstOrDefault(i => i.Id == id);
        }

        /// <summary>Gets movie by title.</summary>
        /// <param name="name">movie title</param>
        /// <returns>null</returns>
        protected override Movie GetMovieByNameCore( string name )
        {
            EnsureInitialized();

            return _movies.FirstOrDefault(
                i => String.Compare(i.Title, name, true) == 0);
        }

        /// <summary>Removes a movie.</summary>
        /// <param name="id">The movie Id.</param>
        protected override void RemoveCore (int id)
        {
            EnsureInitialized();

            var existing = GetCore(id);
            if (existing != null)
            {
                _movies.Remove(existing);
                SaveData();
            };
        }

        #region Private Members
        //Ensure file is loaded
        private void EnsureInitialized()
        {
            if(_movies == null)
            {
                _movies = LoadData();

                if (_movies.Any())
                {
                    _id = _movies.Max(i => i.Id);
                    ++_id;
                };
            };
        }

        private List<Movie> LoadData()
        {
            var movies = new List<Movie>();

            try
            {
                // Make sure the file exists
                if (!File.Exists(_filename))
                    return movies;

                var lines = File.ReadAllLines(_filename);

                foreach (var line in lines)
                {
                    var fields = line.Split(',');

                    var movie = new Movie() {
                        Id = ParseInt32(fields[0]),
                        Title = fields[1],
                        Description = fields[2],
                        Length = ParseInt32(fields[3]),
                        IsOwned = ParseInt32(fields[4]) != 0
                    };
                    movies.Add(movie);
                };

                return movies;
            } catch (Exception e)
            {
                throw new Exception("Failure loading data", e);
            };
        }

        private void SaveData()
        {
            using (var stream = File.OpenWrite(_filename))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var movie in _movies)
                {
                    var line = $"{movie.Id},{movie.Title},{movie.Description}," +
                               $"{movie.Length},{(movie.IsOwned ? 1 : 0)}";

                    writer.WriteLine(line);
                };
            };
        }

        private int ParseInt32( string value )
        {
            if (Int32.TryParse(value, out var result))
                return result;

            return -1;
        }

        private List<Movie> _movies;
        private int _id = 1;
        private readonly string _filename;

        #endregion
    }
}
