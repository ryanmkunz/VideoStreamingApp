using RyanTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace RyanTube
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
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
            
            if (currentUser.AdminApproved)
            {
                authorize = true;
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult("You have not yet been approved by Ryan");
        }
    }
}