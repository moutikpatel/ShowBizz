using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MGP2237A5.Data
{
    public class Episode
    {

        public Episode()
        {
            AirDate = DateTime.Now;
        }

        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        public int SeasonNumber { get; set; }

        public int EpisodeNumber { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AirDate { get; set; }

        [Required, StringLength(250)]
        public string ImageUrl { get; set; }

        [Required, MaxLength(250)]
        public string Clerk { get; set; }

        [Required]
        public Show Show { get; set; }

        public string Premise { get; set; }

        public byte[] Video { get; set; }
        [StringLength(250)]
        public string VideoContentType { get; set; }
    }
}