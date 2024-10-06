using E_Commerce_app.DataAccess.Data;
using E_Commerce_app.Models;
using E_Commerce_app.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.DbIntializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManger; 
        private readonly UserManager<IdentityUser> _userManger;
        private readonly ApplicationDbContext _db;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManger,RoleManager<IdentityRole> roleManger)
        {
            _db = db;
            _userManger = userManger;
            _roleManger = roleManger;
        }

        public void Intializer()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();  
                }
            }catch (Exception ex)
            {
            }

            if (!_roleManger.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManger.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well
                _userManger.CreateAsync(new AppUser
                {
                    UserName = "kiko",
                    Email = "admin@kiko.com",
                    Name = "kiko",
                    PhoneNumber = "1112223333",
                    StreetAddress = "test 123 Ave",
                    State = "IL",
                    PostalCode = "23422",
                    City = "Chicago"
                }, "Admin123*").GetAwaiter().GetResult();


                AppUser user = _db.AppUsers.FirstOrDefault(u => u.Email == "admin@kiko.com");
                _userManger.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
