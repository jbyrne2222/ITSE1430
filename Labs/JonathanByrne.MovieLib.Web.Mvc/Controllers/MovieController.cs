using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JonathanByrne.MovieLib.Data;
using JonathanByrne.MovieLib.Data.Sql;
using JonathanByrne.MovieLib.Web.Mvc.Models;

namespace JonathanByrne.MovieLib.Web.Mvc.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        [HttpGet]
        public ActionResult List()
        {
            var movies = _database.GetAll();

            return View(movies.Select(p => p.ToModel()));
        }

        public MovieController()
        {
            var connString = ConfigurationManager.ConnectionStrings["MovieDatabase"];
            _database = new SqlMovieDatabase(connString.ConnectionString);
        }
        private readonly IMovieDatabase _database;

        //[HttpGet]
        //public ActionResult Index()
        //{
        //    var movies = _database.GetAll();

        //    return View(movies.Select(p => p.ToModel()));
        //}

        [HttpGet]
        public ActionResult Add()
        {
            return View(new MovieViewModel());
        }

        [HttpPost]
        public ActionResult Add( MovieViewModel model )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var movie = model.ToDomain();

                    movie = _database.Add(movie);

                    return RedirectToAction(nameof(List));
                }
            } catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit( int id )
        {
            var movie = _database.GetAll().FirstOrDefault(p => p.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie.ToModel());
        }

        [HttpPost]
        public ActionResult Edit( MovieViewModel model )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var movie = model.ToDomain();

                    _database.Update(movie);

                    return RedirectToAction("List");
                }
            } catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            };

            return View(model);
        }

        [HttpGet]
        [Route("movies/delete/{id}")]
        public ActionResult Delete( int id )
        {
            var movie = _database.GetAll().FirstOrDefault(p => p.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie.ToModel());
        }

        [HttpPost]
        public ActionResult Delete( MovieViewModel model )
        {
            try
            {
                var movie = _database.GetAll()
                                         .FirstOrDefault(p => p.Id == model.Id);

                if (movie == null)
                    return HttpNotFound();

                _database.Remove(model.Id);

                return RedirectToAction(nameof(List));
            } catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            };

            return View(model);
        }
    }
}
