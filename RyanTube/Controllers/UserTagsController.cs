using RyanTube.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RyanTube.Controllers
{
    [CustomAuthorize]
    public class UserTagsController : VideosController
    {
        [HttpGet]
        public ActionResult CreateUserTag(int videoId)
        {
            var userTags = db.UserTags.ToList();
            var lastUserTag = userTags.LastOrDefault();            

            UserTag userTag = new UserTag
            {
                VideoId = videoId
            };
            if (lastUserTag != null)
            {
                userTag.Id = lastUserTag.Id + 1;
            }
            else
            {
                userTag.Id = 0;
            }
            db.UserTags.Add(userTag);
            db.SaveChanges();

            return RedirectToAction("EditUserTag", new { id = userTag.Id });
        }

        [HttpGet]
        public ActionResult EditUserTag(int id)
        {
            var userTag = db.UserTags.FirstOrDefault(u => u.Id == id);
            var users = db.Users.ToList();
            List<SelectListItem> userNames = new List<SelectListItem>();

            foreach (var item in users)
            {
                userNames.Add(new SelectListItem { Text = item.UserName, Value = item.UserName });
            }            
            ViewBag.Users = userNames;

            return View(userTag);
        }

        [HttpPost]
        public ActionResult EditUserTag(UserTag userTag)
        {
            db.UserTags.FirstOrDefault(u => u.Id == userTag.Id).UserName = userTag.UserName;
            db.SaveChanges();

            return RedirectToAction("Watch", "Videos", new { id = userTag.VideoId });
        }

        public ActionResult DeleteUserTag(int id)
        {
            var userTag = db.UserTags.FirstOrDefault(u => u.Id == id);
            db.UserTags.Remove(userTag);
            db.SaveChanges();

            return RedirectToAction("Watch", "Videos", new { id = userTag.VideoId });
        }
    }
}