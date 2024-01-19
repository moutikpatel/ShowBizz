using MGP2237A5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGP2237A5.Controllers
{
    public class ActorController : Controller
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: Actor
        public ActionResult Index()
        {
            return View(m.ActorGetAll());
        }

        // GET: Actor/Details/5
        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var theActor = m.ActorGetByIdWithShowInfo(id.GetValueOrDefault());

            if (theActor == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Sort by Caption
                theActor.Photos = theActor.ActorMediaItems.Where(p => p.ContentType.StartsWith("image/")).OrderBy(p => p.Caption);
                theActor.Documents = theActor.ActorMediaItems.Where(p => p.ContentType.StartsWith("application/pdf")).OrderBy(p => p.Caption);
                theActor.AudioClips = theActor.ActorMediaItems.Where(p => p.ContentType.StartsWith("audio/")).OrderBy(p => p.Caption);
                theActor.VideoClips = theActor.ActorMediaItems.Where(p => p.ContentType.StartsWith("video/")).OrderBy(p => p.Caption);


                // Pass the object to the view
                return View(theActor);
            }
        }

        // GET: Actor/Create
        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            var model = new ActorAddViewModel();
            return View(model);
        }

        // POST: Actor/Create
        [HttpPost, Authorize(Roles = "Executive"), ValidateInput(false)]
        public ActionResult Create(ActorAddViewModel newItem)
        {

            // Validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.ActorAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Id });
            }
        }

        // GET: Actors/5/AddShow
        // TODO: 17 - Used "attribute routing" for a custom URL segment (resource)
        [Route("Actors/{id}/AddShow"), Authorize(Roles = "Coordinator")]
        public ActionResult AddShow(int id)
        {
            // Attempt to get the associated object
            var actor = m.ActorGetByIdWithShowInfo(id);

            var genres = m.GenreGetAll();
            var preSelectedGenre = genres.ElementAt(0).Id;

            if (actor == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Create and configure a form object
                var formModel = new ShowAddFormViewModel();
                formModel.ActorId = actor.Id;
                formModel.ActorName = actor.Name;

                // Get all actors and preselect the known actor
                var selectedValues = new List<int> { actor.Id };

                formModel.ActorList = new MultiSelectList
                    (items: m.ActorGetAll(),
                    dataValueField: "Id",
                    dataTextField: "Name",
                    selectedValues: selectedValues);

                // formModel.GenreId = preSelectedGenre;
                formModel.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name", selectedValue: preSelectedGenre);
                // IEnumerable<string> genres1 = m.GenreGetAll().Select(m => m.Name);
                // formModel.GenreList = new SelectList(genres1);

                return View(formModel);
            }
        }

        // POST: Actors/5/AddShow
        // TODO: 19 - Used "attribute routing" for a custom URL segment (resource)
        [Route("Actors/{id}/AddShow"), Authorize(Roles = "Coordinator"), ValidateInput(false)]
        [HttpPost]
        public ActionResult AddShow(ShowAddViewModel newItem)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.ShowAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                // TODO: 20 - Must redirect to the Vehicles controller
                return RedirectToAction("details", "show", new { id = addedItem.Id });
            }
        }

        // Method uses attribute routing
        // GET: actors/5/addcontent
        [Route("actors/{id}/addcontent"), Authorize(Roles = "Executive")]
        public ActionResult AddContent(int? id)
        {
            // Attempt to get the matching object
            var o = m.ActorGetByIdWithShowInfo(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Create a form
                var form = new ActorMediaItemAddFormViewModel();
                // Configure its property values
                form.ActorId = o.Id;
                form.ActorName = o.Name;

                // Pass the object to the view
                return View(form);
            }
        }

        // POST: actors/5/addcontent
        [HttpPost]
        [Route("actors/{id}/addcontent"), Authorize(Roles = "Executive")]
        public ActionResult AddContent(int? id, ActorMediaItemAddViewModel newItem)
        {
            // Validate the input
            // Two conditions must be checked
            if (!ModelState.IsValid && id.GetValueOrDefault() == newItem.ActorId)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.ActorMediaItemAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", new { id = addedItem.Id });
            }
        }

        // GET: Actors/MediaItem/5
        // TODO 8 - Uses attribute routing
        [Route("Actors/MediaItem/{id}")]
        public ActionResult ShowMediaItem(int? id)
        {
            // Attempt to get the matching object
            var o = m.ActorMediaItemGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                // TODO 9 - Return a file content result
                // Set the Content-Type header, and return the photo bytes
                return File(o.Content, o.ContentType);
            }
        }

        // GET: Actor/MediaItem/5/Download
        // Dedicated media item DOWNLOAD method, uses attribute routing
        [Route("actors/mediaitem/{id}/download")]
        public ActionResult MediaItemDownload(int? id)
        {
            // Attempt to get the matching object
            var mediaItem = m.ActorMediaItemGetById(id.GetValueOrDefault());

            if (mediaItem == null || mediaItem.ContentType != "application/pdf")
            {
                return HttpNotFound();
            }
            else
            {
                // Create a new Content-Disposition header
                var cd = new System.Net.Mime.ContentDisposition
                {
                    // Assemble the file name + extension
                    FileName = $"document-{id}.pdf",
                    // Force the media item to be saved (not viewed)
                    Inline = false
                };
                // Add the header to the response
                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(mediaItem.Content, mediaItem.ContentType);
            }
        }

    }
}

