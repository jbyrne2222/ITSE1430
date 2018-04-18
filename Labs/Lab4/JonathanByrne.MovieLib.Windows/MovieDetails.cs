/*
 * Jonathan Byrne
 * ITSE 1430
 * Lab3
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
        #region Constructors
        /// <summary>Default constructor</summary>
        public MovieDetails()
        {
            InitializeComponent();
        }

         /// <summary>Constructor passing in movie title</summary>
         /// <param name="title">Movie title</param>
        public MovieDetails( string title ) : this()
        {
            Text = title;
        }

        /// <summary>Constructor passing in a movie</summary>
        /// <param name="movie">Movie</param>
        public MovieDetails( Movie movie ) : this("Edit Movie")
        {
            Movie = movie;
        }
        #endregion

        /// <summary>Gets and sets a new movie</summary>
        public Movie Movie { get; set; }

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad(e);

            //Load a movie
            if (Movie != null)
            {
                _txtName.Text = Movie.Title;
                _txtDescription.Text = Movie.Description;
                _txtLength.Text = Movie.Length.ToString();
                _chkIsOwned.Checked = Movie.IsOwned;
            };

            ValidateChildren();
        }

        //Cancel
        private void OnCancel( object sender, EventArgs e)
        {
        }

        //Saves user given info
        private void OnSave (object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            var movie = new Movie() {
                Title = _txtName.Text,
                Description = _txtDescription.Text,
                Length = ConvertToLength(_txtLength),
                IsOwned = _chkIsOwned.Checked,
            };

            var errors = ObjectValidator.Validate(movie);
            if (errors.Count() > 0)
            {
                DisplayError(errors.ElementAt(0).ErrorMessage);
                return;
            };

            Movie = movie;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void DisplayError ( string message )
        {
            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Converts string from length textbox to int length in movie class
        private int ConvertToLength (TextBox control)
        {
            if (Int32.TryParse(control.Text, out var length))
                return length;

            return -1;
        }

        private void _txtName_validating( object sender, System.ComponentModel.CancelEventArgs e)
        {
            var textbox = sender as TextBox;

            if (String.IsNullOrEmpty(textbox.Text))
            {
                _errorProvider.SetError(textbox, "Title is required");
                e.Cancel = true;
            } else
                _errorProvider.SetError(textbox, "");
        }

        private void _txtLength_Validating( object sender, System.ComponentModel.CancelEventArgs e )
        {
            var textbox = sender as TextBox;

            var price = ConvertToLength(textbox);
            if (price < 0)
            {
                _errorProvider.SetError(textbox, "Length must be >= 0");
                e.Cancel = true;
            } else
                _errorProvider.SetError(textbox, "");
        }
    }
}