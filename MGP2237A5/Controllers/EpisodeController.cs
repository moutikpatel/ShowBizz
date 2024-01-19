using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGP2237A5.Controllers
{
    public class EpisodeController : Controller
    {
        private Manager m = new Manager();

        public ActionResult Index()
        {
            return View(m.EpisodeGetAll());
        }

        public ActionResult Details(int? id)
        {
            var views = m.EpisodeGetByIdWithShowName(id.GetValueOrDefault());

            if (views == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(views);
            }
        }
        [Route("Episodes/Video/{id}")]
        public ActionResult ShowVideo(int? id)
        {
            var views = m.EpisodeVideoGetById(id.GetValueOrDefault());

            if (views == null)
            {
                return HttpNotFound();
            }
            else
            {
                return File(views.Video, views.VideoContentType);
            }
        }
    }
}