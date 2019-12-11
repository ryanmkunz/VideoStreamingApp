using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RyanTube.Models
{
    public class UserTag
    {
        [Key]
        public int Id { get; set; }        
        public string UserName { get; set; }

        [ForeignKey("Video")]
        public int VideoId { get; set; }
        public virtual Video Video { get; set; }
    }
}