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
    /// <summary>The interface for our movie database.</summary>
    public interface IMovieDatabase
    {
        Movie Add( Movie movie, out string message );
        Movie Get( int id );
        IEnumerable<Movie> GetAll();
        void Remove( int id );
        Movie Update( Movie movie, out string message );
    }
}
