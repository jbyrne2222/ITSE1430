/*
 * Jonathan Byrne
 * ITSE 1430
 * Lab2
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonathanByrne.MovieLib
{
    /// <summary>Provides information about a movie</summary>
    public class Movie
    {
        /// <summary>Gets or sets the movie title</summary>
        public string Name
        {
            get { return _name ?? ""; }
            set { _name = value; }
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

        private string _name;
        private string _description;
    }
}
