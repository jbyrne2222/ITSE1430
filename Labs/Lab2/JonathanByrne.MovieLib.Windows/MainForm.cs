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
    /// <summary> Declares and initializes MainForm</summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        //Opens new MovieDetails form and saves user given info to a new movie
        private void OnMovieAdd (object sender, EventArgs e)
        {
            var button = sender as ToolStripMenuItem;

            var form = new MovieDetails();
            form.Text = "Add Movie";

            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;

            _movie = form.Movie;
        }

        //Deletes info saved in movie
        private void OnMovieDelete(object sender, EventArgs e)
        {
            if (!ShowConfirmation("Are you sure?", "Remove Movie"))
                return;

            //Delete movie
            _movie = null;
        }

        //Edits info saved in movie
        private void OnMovieEdit(object sender, EventArgs e)
        {
            //Error message if nothing is stored in movie
            if (_movie == null)
            {
                MessageBox.Show("There are no movies to edit");
                return;
            }

            var form = new MovieDetails();
            form.Text = "Edit Movie";
            form.Movie = _movie;

            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;

            //Editing the movie
            _movie = form.Movie;
        }

        //Closes MainForm
        private void OnFileExit(object sender, EventArgs e)
        {
            Close();
        }

        //Provides form with about info
        private void OnHelpAbout(object sender, EventArgs e)
        {
            var form = new AboutMovieLib();

            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;
        }

        //Dialog box to confirm user wants to do something
        private bool ShowConfirmation (string message, string title)
        {
            return MessageBox.Show(this, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes;
        }

        private Movie _movie;

    }
}
