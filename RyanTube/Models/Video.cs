using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RyanTube.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<UserTag> UserTags { get; set; }
    }
}