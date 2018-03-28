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
using JonathanByrne.MovieLib.Data.Memory;

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

            RefreshUI();
        }

        //Opens new MovieDetails form and saves user given info to a new movie
        private void OnMovieAdd (object sender, EventArgs e)
        {
            var button = sender as ToolStripMenuItem;

            var form = new MovieDetails("Add Movie");

            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;

            _database.Add(form.Movie, out var message);
            if (!String.IsNullOrEmpty(message))
                MessageBox.Show(message);

            RefreshUI();
        }

        //Deletes info saved in movie
        private void OnMovieDelete( object sender, EventArgs e)
        {
            var movie = GetSelectedMovie();
            if (movie == null)
            {
                MessageBox.Show(this, "No movie selected", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };

            DeleteMovie(movie);
        }

        private void DeleteMovie( Movie movie )
        {
            if (!ShowConfirmation("Are you sure?", "Remove Movie"))
                return;

            //Delete movie
            _database.Remove(movie.Id);

            RefreshUI();
        }

        //Edits info saved in movie
        private void OnMovieEdit(object sender, EventArgs e)
        {
            var movie = GetSelectedMovie();
            if (movie == null)
            {
                MessageBox.Show(this, "No movie selected", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };

            EditMovie(movie);
        }

        private void EditMovie( Movie movie )
        {
            var form = new MovieDetails(movie);
            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;

            form.Movie.Id = movie.Id;
            _database.Update(form.Movie, out var message);
            if (!String.IsNullOrEmpty(message))
                MessageBox.Show(message);

            RefreshUI();
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

        //Gets movie in selected row on grid
        private Movie GetSelectedMovie ()
        {
            if (dataGridView1.SelectedRows.Count > 0)
                return dataGridView1.SelectedRows[0].DataBoundItem as Movie;

            return null;
        }

        private void RefreshUI ()
        {
            var movies = _database.GetAll();

            movieBindingSource.DataSource = movies.ToList();
        }

        //Dialog box to confirm user wants to do something
        private bool ShowConfirmation (string message, string title)
        {
            return MessageBox.Show(this, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes;
        }

        private IMovieDatabase _database = new MemoryMovieDatabase();

        private void OnCellDoubleClick( object sender, DataGridViewCellEventArgs e )
        {
            var movie = GetSelectedMovie();
            if (movie == null)
                return;

            EditMovie(movie);
        }

        private void OnCellKeyDown( object sender, KeyEventArgs e)
        {
            var movie = GetSelectedMovie();
            if (movie == null)
                return;

            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                DeleteMovie(movie);
            } else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                EditMovie(movie);
            };
        }
    }
}
