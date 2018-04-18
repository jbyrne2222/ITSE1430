/*
 * Jonathan Byrne
 * ITSE 1430
 * Lab3
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JonathanByrne.MovieLib.Data.Memory;
using JonathanByrne.MovieLib.Data.Sql;

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

            var connString = ConfigurationManager.ConnectionStrings["MovieDatabase"];
            _database = new SqlMovieDatabase(connString.ConnectionString);

            RefreshUI();
        }

        //Opens new MovieDetails form and saves user given info to a new movie
        private void OnMovieAdd (object sender, EventArgs e)
        {
            var button = sender as ToolStripMenuItem;

            var form = new MovieDetails("Add Movie");

            //Show form modally
            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;

            //Add to database
            try
            {
                _database.Add(form.Movie);
            } catch (NotImplementedException)
            {
                MessageBox.Show("Not Implemented yet.");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            };

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

        //Helper method to handle deleting movies
        private void DeleteMovie( Movie movie )
        {
            if (!ShowConfirmation("Are you sure?", "Remove Movie"))
                return;

            //Delete movie
            try
            {
                _database.Remove(movie.Id);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            };

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

        //Helper method to handle editing movies
        private void EditMovie( Movie movie )
        {
            var form = new MovieDetails(movie);
            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;

            //Update the movie
            form.Movie.Id = movie.Id;        

            try
            {
                _database.Update(form.Movie);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            };

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
            var items = (from r in dataGridView1.SelectedRows.OfType<DataGridViewRow>()
                          select new {
                              Index = r.Index,
                              Movie = r.DataBoundItem as Movie
                          }).FirstOrDefault();

            return items.Movie;
        }

        private void RefreshUI ()
        {
            IEnumerable<Movie> movies = null;
            try
            {
                movies = _database.GetAll();
            } catch (Exception)
            {
                MessageBox.Show("Error loading movies");
            };

            movieBindingSource.DataSource = movies?.ToList();
        }

        //Dialog box to confirm user wants to do something
        private bool ShowConfirmation (string message, string title)
        {
            return MessageBox.Show(this, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes;
        }

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

        private IMovieDatabase _database;
    }
}
