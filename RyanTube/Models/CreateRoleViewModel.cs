using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RyanTube.Models
{
    public class CreateRoleViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}