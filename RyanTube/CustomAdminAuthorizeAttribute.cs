using RyanTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RyanTube
{
    public class CustomAdminAuthorizeAttribute : AuthorizeAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var currentUserName = HttpContext.Current.User.Identity.Name;

            if (currentUserName == "")
            {
                return authorize;
            }
            var currentUserId = db.Users.FirstOrDefault(u => u.Email == currentUserName).Id;
            var currentUser = db.Users.FirstOrDefault(u => u.Id == currentUserId);
            
            if (currentUser.UserRole == "Admin")
            {
                authorize = true;
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult("Admin status required");
        }
    }
}