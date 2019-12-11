using RyanTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RyanTube.Controllers
{
    public class HomeController : BaseController
    {        
        public ActionResult Index()
        {            
            var currentUserName = User.Identity.Name;

            if ( currentUserName == "")
            {
                return RedirectToAction("LogIn", "Account");
            }

            var currentUser = db.Users.FirstOrDefault(u => u.Email == currentUserName);
            var isAdminApproved = currentUser.AdminApproved;

            if (!isAdminApproved)
            {
                return RedirectToAction("AwaitApproval", currentUser);
            }
            return RedirectToAction("ListVideos", "Videos");
        }

        public ActionResult About()
        {            
            return View();
        }

        public ActionResult Contact()
        {            
            return View();
        }

        public ActionResult AwaitApproval(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);

            return View(user);
        }
    }
}