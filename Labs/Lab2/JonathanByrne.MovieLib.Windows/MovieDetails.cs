/*
 * Jonathan Byrne
 * ITSE 1430
 * Lab2
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JonathanByrne.MovieLib.Windows
{
    /// <summary>Declares and Initializes MovieDetails Form</summary>
    public partial class MovieDetails : Form
    {
        public MovieDetails()
        {
            InitializeComponent();
        }

        /// <summary>Gets and sets a new movie</summary>
        public Movie Movie { get; set; }

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad(e);

            //Load a movie
            if (Movie != null)
            {
                _txtName.Text = Movie.Name;
                _txtDescription.Text = Movie.Description;
                _txtLength.Text = Movie.Length.ToString();
                _chkIsOwned.Checked = Movie.IsOwned;
            };
        }

        //Cancel
        private void OnCancel( object sender, EventArgs e)
        {

        }

        //Saves user given info
        private void OnSave (object sender, EventArgs e)
        {
            var movie = new Movie();
            movie.Name = _txtName.Text;
            //Displays an error and sets a default name value if user does not provide one
            if (String.IsNullOrEmpty(movie.Name))
            {
                MessageBox.Show("Name cannot be empty");
                movie.Name = "Unknown movie title";
            }               
            movie.Description = _txtDescription.Text;
            movie.Length = ConvertToLength(_txtLength);
            //Displays an error and sets a default length value if user provides illegal length
            if (movie.Length < 0)
            {
                MessageBox.Show("Length must be either empty or a value >= 0");
                movie.Length = 0;
            }
            movie.IsOwned = _chkIsOwned.Checked;


            Movie = movie;
            DialogResult = DialogResult.OK;
            Close();
        }

        //Converts string from length textbox to int length in movie class
        private int ConvertToLength (TextBox control)
        {
            if (Int32.TryParse(control.Text, out var length))
                return length;

            return 0;
        }


    }
}
