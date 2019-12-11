using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RyanTube.Models;

[assembly: OwinStartupAttribute(typeof(RyanTube.Startup))]
namespace RyanTube
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }
        private void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new Microsoft.AspNet.Identity.RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Visitor"))
            {
                var role = new IdentityRole();
                role.Name = "Visitor";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }            
        }
    }
}
