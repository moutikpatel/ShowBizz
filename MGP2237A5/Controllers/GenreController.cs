using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGP2237A5.Controllers
{
    public class GenreController : Controller
    {
        private Manager m = new Manager();
        public ActionResult Index()
        {
            return View(m.GenreGetAll());
        }
    }
}
