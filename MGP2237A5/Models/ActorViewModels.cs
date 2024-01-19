using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MGP2237A5.Models
{

    public class ActorAddViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(150), Display(Name = "Alternate Name")]
        public string AlternateName { get; set; }

        [Display(Name = "Birth Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Height (m)"), DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double? Height { get; set; }

        [Required, StringLength(250), Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Biography")]
        public string Biography { get; set; }
    }

    public class ActorBaseViewModel : ActorAddViewModel
    {
        
        [Required, StringLength(250)]
        public string Executive { get; set; }
    }


    public class ActorWithShowInfoViewModel : ActorBaseViewModel
    {
        public ActorWithShowInfoViewModel()
        {
            Shows = new List<ShowBaseViewModel>();
            ActorMediaItems = new List<ActorMediaItemBaseViewModel>();
            Photos = new List<ActorMediaItemBaseViewModel>();
            Documents = new List<ActorMediaItemBaseViewModel>();
            AudioClips = new List<ActorMediaItemBaseViewModel>();
            VideoClips = new List<ActorMediaItemBaseViewModel>();
        }

        [Display(Name = "Appeared In")]
        public IEnumerable<ShowBaseViewModel> Shows { get; set; }
        public IEnumerable<ActorMediaItemBaseViewModel> ActorMediaItems { get; set; }
        public IEnumerable<ActorMediaItemBaseViewModel> Photos { get; set; }
        public IEnumerable<ActorMediaItemBaseViewModel> Documents { get; set; }

        [Display(Name = "Audio Clips")]
        public IEnumerable<ActorMediaItemBaseViewModel> AudioClips { get; set; }

        [Display(Name = "Video Clips")]
        public IEnumerable<ActorMediaItemBaseViewModel> VideoClips { get; set; }
    }
}