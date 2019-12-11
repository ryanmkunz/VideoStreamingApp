using RyanTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RyanTube.Controllers
{
    public class BaseController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUser GetCurrentUser()
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var currentApplicationUser = db.Users.FirstOrDefault(u => u.UserName == userName);

            return currentApplicationUser;
        }
    }
}