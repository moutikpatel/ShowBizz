using AutoMapper;
using MGP2237A5.Data;
using MGP2237A5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Web;

// ************************************************************************************
// WEB524 Project Template V2 == 2237-f353a210-fe34-48a1-a666-15d62ce093b4
//
// By submitting this assignment you agree to the following statement.
// I declare that this assignment is my own work in accordance with the Seneca Academic
// Policy. No part of this assignment has been copied manually or electronically from
// any other source (including web sites) or distributed to other students.
// ************************************************************************************


namespace MGP2237A5.Controllers
{
    public class Manager
    {

        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();
                cfg.CreateMap<Genre, GenreBaseViewModel>();
                cfg.CreateMap<Actor, ActorBaseViewModel>();
                cfg.CreateMap<Actor, ActorAddViewModel>();
                cfg.CreateMap<Actor, ActorWithShowInfoViewModel>();
                cfg.CreateMap<ActorAddViewModel, Actor>();
                cfg.CreateMap<Show, ShowBaseViewModel>();
                cfg.CreateMap<ShowAddViewModel, Show>();
                cfg.CreateMap<Show, ShowWithInfoViewModel>();
                cfg.CreateMap<Episode, EpisodeBaseViewModel>();
                cfg.CreateMap<Episode, EpisodeWithShowNameViewModel>();
                cfg.CreateMap<EpisodeAddViewModel, Episode>();
                cfg.CreateMap<Episode, EpisodeVideoViewModel>();
                cfg.CreateMap<ActorMediaItem, ActorMediaItemWithContentViewModel>();
                cfg.CreateMap<ActorMediaItem, ActorMediaItemBaseViewModel>();


            });

            mapper = config.CreateMapper();
            ds.Configuration.ProxyCreationEnabled = false;
            ds.Configuration.LazyLoadingEnabled = false;
        }



     

        // Actor
        public IEnumerable<ActorBaseViewModel> ActorGetAll()
        {
            var views = ds.Actors.OrderBy(a => a.Name);
            return mapper.Map<IEnumerable<Actor>, IEnumerable<ActorBaseViewModel>>(views);
        }

        public ActorWithShowInfoViewModel ActorAdd(ActorAddViewModel newItem)
        {
            var user = HttpContext.Current.User.Identity.Name;
            if (user == null)
            {
                return null;
            }
            else
            {
                var views = ds.Actors.Add(mapper.Map<ActorAddViewModel, Actor>(newItem));
                views.Executive = user;
                ds.SaveChanges();
                return (views == null) ? null : mapper.Map<Actor, ActorWithShowInfoViewModel>(views);
            }
        }

        public ActorWithShowInfoViewModel ActorGetByIdWithShowInfo(int id)
        {
            var views = ds.Actors
                             .Include("Shows")
                             .Include("ActorMediaItems")
                             .SingleOrDefault(a => a.Id == id);
            return (views == null) ? null : mapper.Map<Actor, ActorWithShowInfoViewModel>(views);
        }

        public ActorMediaItemWithContentViewModel ActorMediaItemGetById(int id)
        {
            var views = ds.ActorMediaItems.SingleOrDefault(p => p.Id == id);
            return (views == null) ? null : mapper.Map<ActorMediaItem, ActorMediaItemWithContentViewModel>(views);
        }

        public ActorBaseViewModel ActorMediaItemAdd(ActorMediaItemAddViewModel newItem)
        {
            var views = ds.Actors.Find(newItem.ActorId);
            if (views == null)
            {
                return null;
            }
            else
            {
                var addedItem = new ActorMediaItem();
                ds.ActorMediaItems.Add(addedItem);
                addedItem.Caption = newItem.Caption;
                addedItem.Actor = views;
                byte[] contentBytes = new byte[newItem.ContentUpload.ContentLength];
                newItem.ContentUpload.InputStream.Read(contentBytes, 0, newItem.ContentUpload.ContentLength);
                addedItem.Content = contentBytes;
                addedItem.ContentType = newItem.ContentUpload.ContentType;
                ds.SaveChanges();
                return (addedItem == null) ? null : mapper.Map<Actor, ActorBaseViewModel>(views);
            }
        }

        // Episode
        public IEnumerable<EpisodeBaseViewModel> EpisodeGetAll()
        {
            var views = ds.Episodes
                                .Include("Show")
                                .OrderBy(a => a.Name)
                                .ThenBy(a => a.SeasonNumber)
                                .ThenBy(a => a.EpisodeNumber);

            return mapper.Map<IEnumerable<Episode>, IEnumerable<EpisodeBaseViewModel>>(views);
        }

        public EpisodeWithShowNameViewModel EpisodeAdd(EpisodeAddViewModel newItem)
        {
            var user = HttpContext.Current.User.Identity.Name;
            var show = ds.Shows.Find(newItem.ShowId);
            if (user == null || show == null)
            {
                return null;
            }
            else
            {
                var views = ds.Episodes.Add(mapper.Map<EpisodeAddViewModel, Episode>(newItem));
                byte[] videoBytes = new byte[newItem.VideoUpload.ContentLength];
                newItem.VideoUpload.InputStream.Read(videoBytes, 0, newItem.VideoUpload.ContentLength);
                views.Clerk = user;
                views.Show = show;
                views.Video = videoBytes;
                views.VideoContentType = newItem.VideoUpload.ContentType;
                ds.SaveChanges();
                return (views == null) ? null : mapper.Map<Episode, EpisodeWithShowNameViewModel>(views);
            }
        }

        public EpisodeWithShowNameViewModel EpisodeGetByIdWithShowName(int id)
        {
            var views = ds.Episodes
                .Include("Show")
                .SingleOrDefault(a => a.Id == id);
            return (views == null) ? null : mapper.Map<Episode, EpisodeWithShowNameViewModel>(views);
        }

        public EpisodeVideoViewModel EpisodeVideoGetById(int id)
        {
            var views = ds.Episodes.Find(id);

            return (views == null) ? null : mapper.Map<Episode, EpisodeVideoViewModel>(views);
        }


        // Genre
        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            var views = ds.Genres.OrderBy(a => a.Name);
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(views);
        }

        // Show
        public IEnumerable<ShowBaseViewModel> ShowGetAll()
        {
            var views = ds.Shows.OrderBy(a => a.Name);
            return mapper.Map<IEnumerable<Show>, IEnumerable<ShowBaseViewModel>>(views);
        }

        public ShowWithInfoViewModel ShowAdd(ShowAddViewModel newItem)
        {
            var user = HttpContext.Current.User.Identity.Name;
            if (user == null)
            {
                return null;
            }
            else
            {
                var views = ds.Shows.Add(mapper.Map<ShowAddViewModel, Show>(newItem));
                views.Coordinator = user;
                var showActors = new List<Actor>();
                foreach (var actorId in newItem.ActorIds)
                {
                    var actor = ds.Actors.Find(actorId);
                    if (actor != null)
                    {
                        showActors.Add(actor);
                    }
                }
                views.Actors = showActors;
                ds.SaveChanges();
                return (views == null) ? null : mapper.Map<Show, ShowWithInfoViewModel>(views);
            }

        }

        public ShowWithInfoViewModel ShowGetByIdWithInfo(int id)
        {
            var views = ds.Shows
                .Include("Actors")
                .Include("Episodes")
                .SingleOrDefault(a => a.Id == id);
            return (views == null) ? null : mapper.Map<Show, ShowWithInfoViewModel>(views);
        }

       

        #region Role Claims

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        #endregion

        #region Load Data Methods

        public bool LoadRoles()
        {
            var user = HttpContext.Current.User.Identity.Name;
            bool done = false;
            ds.RoleClaims.Add(new RoleClaim() { Name = "Admin" });
            ds.RoleClaims.Add(new RoleClaim() { Name = "Executive" });
            ds.RoleClaims.Add(new RoleClaim() { Name = "Coordinator" });
            ds.RoleClaims.Add(new RoleClaim() { Name = "Clerk" });
            ds.SaveChanges();
            done = true;
            return done;
        }

        public bool LoadGenres()
        {
            bool done = false;

            if (ds.Genres.Count() == 0)
            {
                ds.Genres.Add(new Genre { Name = "Action" });
                ds.Genres.Add(new Genre { Name = "Adventure" });
                ds.Genres.Add(new Genre { Name = "Comedy" });
                ds.Genres.Add(new Genre { Name = "Crime" });
                ds.Genres.Add(new Genre { Name = "Drama" });
                ds.Genres.Add(new Genre { Name = "Fantasy" });
                ds.Genres.Add(new Genre { Name = "Horror" });
                ds.Genres.Add(new Genre { Name = "Mystery" });
                ds.Genres.Add(new Genre { Name = "Romance" });
                ds.Genres.Add(new Genre { Name = "Sci-Fi" });

                ds.SaveChanges();
                done = true;
            }
            return done;
        }

        public bool LoadActors()
        {

            var user = HttpContext.Current.User.Identity.Name;
            if (ds.Actors.Count() > 0) { return false; }

            ds.Actors.Add(new Actor
            {
                Name = "Benedict Cumberbatch",
                AlternateName = "Sherlock Holmes",
                BirthDate = new DateTime(1976, 7, 19),
                Executive = user,
                Height = 1.83,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/BCumberbatch_Comic-Con_2019.jpg/220px-BCumberbatch_Comic-Con_2019.jpg"
            });


            ds.Actors.Add(new Actor
            {
                Name = "Robert Downey Jr.",
                AlternateName = "Iron Man",
                BirthDate = new DateTime(1965, 4, 4),
                Executive = user,
                Height = 1.74,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/94/Robert_Downey_Jr_2014_Comic_Con_%28cropped%29.jpg/220px-Robert_Downey_Jr_2014_Comic_Con_%28cropped%29.jpg"
            });

            ds.Actors.Add(new Actor
            {
                Name = "Scarlett Johansson",
                AlternateName = "Black Widow",
                BirthDate = new DateTime(1984, 11, 22),
                Executive = user,
                Height = 1.6,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2a/Scarlett_Johansson_by_Gage_Skidmore_2_%28cropped%2C_2%29.jpg/220px-Scarlett_Johansson_by_Gage_Skidmore_2_%28cropped%2C_2%29.jpg"
            });

            ds.Actors.Add(new Actor
            {
                Name = "Leonardo DiCaprio",
                AlternateName = "Leo",
                BirthDate = new DateTime(1974, 11, 11),
                Executive = user,
                Height = 1.83,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Leonardo_Dicaprio_Cannes_2019.jpg/220px-Leonardo_Dicaprio_Cannes_2019.jpg"
            });

            // Save changes
            ds.SaveChanges();
            return true;
        }

        public bool LoadShows()
        {

            var user = HttpContext.Current.User.Identity.Name;
            if (ds.Shows.Count() > 0) { return false; }
            var sherlock = ds.Actors.SingleOrDefault(a => a.Name == "Benedict Cumberbatch");
            if (sherlock == null) { return false; }

            ds.Shows.Add(new Show
            {
                Actors = new Actor[] { sherlock },
                Name = "Sherlock",
                Coordinator = user,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/4d/Sherlock_titlecard.jpg",
                Genre = "Mystery",
                ReleaseDate = new DateTime(2010, 7, 25)
            });

            ds.Shows.Add(new Show
            {
                Actors = new Actor[] { sherlock },
                Name = "Good Omens",
                Coordinator = user,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/57/Good_Omens.svg",
                Genre = "Comedy, Fantasy",
                ReleaseDate = new DateTime(2019, 5, 31)
            });
            ds.SaveChanges();
            return true;
        }


        public bool LoadEpisodes()
        {

            var user = HttpContext.Current.User.Identity.Name;
            if (ds.Episodes.Count() > 0) { return false; }
            var sherlock = ds.Shows.SingleOrDefault(a => a.Name == "Sherlock");
            var goodOmens = ds.Shows.SingleOrDefault(a => a.Name == "Good Omens");

            if (sherlock == null || goodOmens == null) { return false; }
            ds.Episodes.Add(new Episode
            {
                Name = "S1E1",
                EpisodeNumber = 1,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2010, 7, 25),
                Genre = "Mystery",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/4d/Sherlock_titlecard.jpg",
                Show = sherlock
            });

            ds.Episodes.Add(new Episode
            {
                Name = "S1E2",
                EpisodeNumber = 2,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2010, 8, 1),
                Genre = "Mystery",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/4d/Sherlock_titlecard.jpg",
                Show = sherlock
            });

            ds.Episodes.Add(new Episode
            {
                Name = "S1E3",
                EpisodeNumber = 3,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2010, 8, 8),
                Genre = "Mystery",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/4d/Sherlock_titlecard.jpg",
                Show = sherlock
            });


            ds.Episodes.Add(new Episode
            {
                Name = "S1E1",
                EpisodeNumber = 1,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2019, 5, 31),
                Genre = "Comedy, Fantasy",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/57/Good_Omens.svg",
                Show = goodOmens
            });

            ds.Episodes.Add(new Episode
            {
                Name = "S1E2",
                EpisodeNumber = 2,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2019, 6, 7),
                Genre = "Comedy, Fantasy",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/57/Good_Omens.svg",
                Show = goodOmens
            });

            ds.Episodes.Add(new Episode
            {
                Name = "S1E3",
                EpisodeNumber = 3,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2019, 6, 14),
                Genre = "Comedy, Fantasy",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/57/Good_Omens.svg",
                Show = goodOmens
            });


            ds.SaveChanges();
            return true;
        }


        public bool RemoveRoles()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Removing Genres
        public bool RemoveGenres()
        {
            try
            {
                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Removing Actors
        public bool RemoveActors()
        {
            try
            {
                foreach (var e in ds.Actors)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Removing Shows
        public bool RemoveShows()
        {
            try
            {
                foreach (var e in ds.Shows)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Removing Episodes
        public bool RemoveEpisodes()
        {
            try
            {
                foreach (var e in ds.Episodes)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region RequestUser Class

        // This "RequestUser" class includes many convenient members that make it
        // easier work with the authenticated user and render user account info.
        // Study the properties and methods, and think about how you could use this class.

        // How to use...
        // In the Manager class, declare a new property named User:
        //    public RequestUser User { get; private set; }

        // Then in the constructor of the Manager class, initialize its value:
        //    User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

        public class RequestUser
        {
            // Constructor, pass in the security principal
            public RequestUser(ClaimsPrincipal user)
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    Principal = user;

                    // Extract the role claims
                    RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                    // User name
                    Name = user.Identity.Name;

                    // Extract the given name(s); if null or empty, then set an initial value
                    string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                    if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                    GivenName = gn;

                    // Extract the surname; if null or empty, then set an initial value
                    string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                    if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                    Surname = sn;

                    IsAuthenticated = true;
                    // You can change the string value in your app to match your app domain logic
                    IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
                }
                else
                {
                    RoleClaims = new List<string>();
                    Name = "anonymous";
                    GivenName = "Unauthenticated";
                    Surname = "Anonymous";
                    IsAuthenticated = false;
                    IsAdmin = false;
                }

                // Compose the nicely-formatted full names
                NamesFirstLast = $"{GivenName} {Surname}";
                NamesLastFirst = $"{Surname}, {GivenName}";
            }

            // Public properties
            public ClaimsPrincipal Principal { get; private set; }

            public IEnumerable<string> RoleClaims { get; private set; }

            public string Name { get; set; }

            public string GivenName { get; private set; }

            public string Surname { get; private set; }

            public string NamesFirstLast { get; private set; }

            public string NamesLastFirst { get; private set; }

            public bool IsAuthenticated { get; private set; }

            public bool IsAdmin { get; private set; }

            public bool HasRoleClaim(string value)
            {
                if (!IsAuthenticated) { return false; }
                return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
            }

            public bool HasClaim(string type, string value)
            {
                if (!IsAuthenticated) { return false; }
                return Principal.HasClaim(type, value) ? true : false;
            }
        }

        #endregion
    }
}