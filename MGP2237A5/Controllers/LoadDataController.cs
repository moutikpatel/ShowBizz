using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGP2237A5.Controllers
{
    public class LoadDataController : Controller
    {
        // Reference to the manager object
        Manager m = new Manager();
        // GET: LoadData/Roles
        [AllowAnonymous]
        public ActionResult Roles()
        {
            ViewBag.Result = m.LoadRoles() ? "Roles data was loaded" : "(done)";
            return View("result");
        }

     
        // GET: LoadData/Genres
        public ActionResult Genres()
        {
            ViewBag.Result = m.LoadGenres() ? "Genres data was loaded" : "(done)";
            return View("result");
        }
        // GET: LoadData/Actors
        public ActionResult Actors()
        {
            ViewBag.Result = m.LoadActors() ? "Actors data was loaded" : "(done)";
            return View("result");
        }
        // GET: LoadData/Shows
        public ActionResult Shows()
        {
            ViewBag.Result = m.LoadShows() ? "Shows data was loaded" : "(done)";
            return View("result");
        }
        // GET: LoadData/Episodes
        public ActionResult Episodes()
        {
            ViewBag.Result = m.LoadEpisodes() ? "Episodes data was loaded" : "(done)";
            return View("result");
        }


        public ActionResult RemoveRoles()
        {
            if (m.RemoveRoles())
            {
                return Content("Roles has been removed");
            }
            else
            {
                return Content("could not remove Roles");
            }
        }

        // GET: LoadData/RemoveGenres
        public ActionResult RemoveGenres()
        {
            if (m.RemoveGenres())
            {
                return Content("Genres has been removed");
            }
            else
            {
                return Content("could not remove Genres");
            }
        }

        // GET: LoadData/RemoveActors
        public ActionResult RemoveActors()
        {
            if (m.RemoveActors())
            {
                return Content("Actors has been removed");
            }
            else
            {
                return Content("could not remove Actors");
            }
        }

        // GET: LoadData/RemoveShows
        public ActionResult RemoveShows()
        {
            if (m.RemoveShows())
            {
                return Content("Shows has been removed");
            }
            else
            {
                return Content("could not remove Shows");
            }
        }

        // GET: LoadData/RemoveEpisodes
        public ActionResult RemoveEpisodes()
        {
            if (m.RemoveEpisodes())
            {
                return Content("Episodes has been removed");
            }
            else
            {
                return Content("could not remove Episodes");
            }
        }

        // GET: LoadData/RemoveDatabase
        public ActionResult RemoveDatabase()
        {
            if (m.RemoveDatabase())
            {
                return Content("database has been removed");
            }
            else
            {
                return Content("could not remove database");
            }
        }
    }
}