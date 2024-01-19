using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MGP2237A5.Data
{
    public class Actor

    {
        public Actor()
        {
            Shows = new HashSet<Show>();
            ActorMediaItems = new HashSet<ActorMediaItem>();
        }
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string AlternateName { get; set; }

        public DateTime? BirthDate { get; set; }

        public double? Height { get; set; }

        [Required, MaxLength(250)]
        public string ImageUrl { get; set; }

        [Required, MaxLength(250)]
        public string Executive { get; set; }

        public ICollection<Show> Shows { get; set; } = new HashSet<Show>();

        public string Biography { get; set; }

        public ICollection<ActorMediaItem> ActorMediaItems { get; set; }
    }

}