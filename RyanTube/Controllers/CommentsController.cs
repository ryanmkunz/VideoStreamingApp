using RyanTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RyanTube.Controllers
{
    [CustomAuthorize]
    public class CommentsController : VideosController
    {        
        [HttpGet]
        public ActionResult CreateComment(int videoId)
        {            
            var comments = db.Comments.ToList();
            var lastComment = comments.LastOrDefault();
            var currentUser = GetCurrentUser();
            var userName = currentUser.UserName;

            Comment comment = new Comment
            {                
                VideoId = videoId,
                User = currentUser,
                UserName = userName
            };
            if (lastComment != null)
            {
                comment.Id = lastComment.Id + 1;
            }
            else
            {
                comment.Id = 0;
            }            
            db.Comments.Add(comment);
            db.SaveChanges();
            
            return RedirectToAction("EditComment", new { id = comment.Id });
        }  
        
        public bool HasPermission(int id)
        {
            var comment = db.Comments.FirstOrDefault(c => c.Id == id);
            var currentUser = GetCurrentUser();

            if (comment.User == currentUser || currentUser.UserRole == "Admin")
            {
                return true;
            }

            return false;
        }

        [HttpGet]
        public ActionResult EditComment(int id)
        {            
            if (HasPermission(id) == true)
            {
                var comment = db.Comments.FirstOrDefault(c => c.Id == id);
                return View(comment);
            }

            return View("LackingPermission");
        }

        [HttpPost]
        public ActionResult EditComment(Comment comment)
        {
            db.Comments.FirstOrDefault(c => c.Id == comment.Id).Content = comment.Content;
            db.SaveChanges();            
            
            return RedirectToAction("Watch", "Videos", new { id = comment.VideoId });
        }
        
        public ActionResult DeleteComment(int id)
        {            
            if (HasPermission(id) == true)
            {
                var comment = db.Comments.FirstOrDefault(c => c.Id == id);
                db.Comments.Remove(comment);
                db.SaveChanges();
            }

            return View("LackingPermission");
        }
    }
}