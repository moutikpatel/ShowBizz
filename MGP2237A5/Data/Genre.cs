using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MGP2237A5.Data
{
    public class Genre
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}