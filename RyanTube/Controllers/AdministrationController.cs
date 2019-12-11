using RyanTube.Models;
using System.Linq;
using System.Web.Mvc;

namespace RyanTube.Controllers
{
    [CustomAdminAuthorize]
    public class AdministrationController : AccountController
    {
        ApplicationDbContext db = new ApplicationDbContext();  
                
        public ActionResult ListUsers()
        {
            var users = db.Users.ToList();
            return View(users);
        }   
        
        public ActionResult ApproveUser(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id).AdminApproved = true;
            db.SaveChanges();            

            return RedirectToAction("ListUsers", "Administration");
        }

        public ActionResult MakeAdmin(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id).UserRole = "Admin";
            var result = db.SaveChanges();            

            return View("ListUsers");
        }

        public ActionResult Details(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);

            return View(user);
        } 
        
        public ActionResult Delete(string id)
        {                        
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            db.Users.Remove(user);
            db.SaveChanges();
            
            return RedirectToAction("ListUsers");            
        }
    }
}