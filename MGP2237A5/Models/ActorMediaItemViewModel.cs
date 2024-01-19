using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MGP2237A5.Models
{
    public class ActorMediaItemBaseViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Caption { get; set; }

        public string ContentType { get; set; }
    }

    public class ActorMediaItemAddViewModel
    {
        public int ActorId { get; set; }
        [Required, StringLength(100)]
        public string Caption { get; set; }
        [Required]
        public HttpPostedFileBase ContentUpload { get; set; }
    }
    public class ActorMediaItemAddFormViewModel
    {
        public int ActorId { get; set; }

        [Display(Name = "Actor Name")]
        public string ActorName { get; set; }
        [Required, StringLength(100)]
        public string Caption { get; set; }
        [Required]
        [Display(Name = "Upload Content")]
        [DataType(DataType.Upload)]
        public string ContentUpload { get; set; }
    }
    public class ActorMediaItemWithContentViewModel : ActorMediaItemBaseViewModel
    {
        public byte[] Content { get; set; }
    }

}