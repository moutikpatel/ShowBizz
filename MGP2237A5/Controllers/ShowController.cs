using MGP2237A5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGP2237A5.Controllers
{
    public class ShowController : Controller
    {
        private Manager m = new Manager();
        public ActionResult Index()
        {
            return View(m.ShowGetAll());
        }
        public ActionResult Details(int? id)
        {
            var theShow = m.ShowGetByIdWithInfo(id.GetValueOrDefault());

            if (theShow == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(theShow);
            }
        }
        [Route("Shows/{id}/AddEpisode"), Authorize(Roles = "Clerk")]
        public ActionResult AddEpisode(int id)
        {
            var show = m.ShowGetByIdWithInfo(id);
            var genres = m.GenreGetAll();
            var preSelectedGenre = genres.ElementAt(0).Id;
            if (show == null)
            {
                return HttpNotFound();
            }
            else
            {
                var formModel = new EpisodeAddFormViewModel();
                formModel.ShowId = show.Id;
                formModel.ShowName = show.Name;
                formModel.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name", selectedValue: preSelectedGenre);
                return View(formModel);
            }
        }


        [Route("Shows/{id}/AddEpisode"), Authorize(Roles = "Clerk")]
        [HttpPost]
        public ActionResult AddEpisode(EpisodeAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            var addedItem = m.EpisodeAdd(newItem);
            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", "Episode", new { id = addedItem.Id });
            }
        }
    }
}
