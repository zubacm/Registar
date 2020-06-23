using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Services
{
    public class UserAdministrationService : IUserAdministration
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserAdministrationService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return _context.Roles.ToList();   
        }

        public void BanUser(String userId)
        {
            var lockoutEndDate = new DateTime(2999, 01, 01);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            _userManager.SetLockoutEnabledAsync(user, true);
            _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);
            _userManager.UpdateAsync(user);

        }

        public void CancelBan(String userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            _userManager.SetLockoutEnabledAsync(user, true);
            _userManager.SetLockoutEndDateAsync(user, null);
            _context.SaveChanges();
        }

        public bool CheckIfUserIsBanned(String userId)
        {
            var le = _context.Users.FirstOrDefault(u => u.Id == userId).LockoutEnd;
            return _context.Users.FirstOrDefault(u => u.Id == userId).LockoutEnd != null ? true : false;
        }

        public String GetUserRole(string userId)
        {
            if (_context.UserRoles.Where(ur => ur.UserId == userId).FirstOrDefault() != null)
            {
                var roleId = _context.UserRoles.Where(ur => ur.UserId == userId).FirstOrDefault().RoleId;
                return _context.Roles.Where(r => r.Id == roleId).FirstOrDefault().Name;
            }
            return "No role";
        }
        public String GetUserRoleId(string userId)
        {
            if (_context.UserRoles.Where(ur => ur.UserId == userId).FirstOrDefault() != null)
            {
                return _context.UserRoles.Where(ur => ur.UserId == userId).FirstOrDefault().RoleId;
            }
            return "No role";
        }


        public async Task GiveUserRoleAsync(string userId, string role)
        {
            string roleId = _context.Roles.Where(r => r.Name == role).FirstOrDefault().Id;
            String query = "Insert into AspNetUserRoles (UserId, RoleId) Values('" + userId + "', '" + roleId + "')";
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            _context.Database.ExecuteSqlCommand(query);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
            await _context.SaveChangesAsync();
        }

        public async Task ChangeUserRoleAsync(String roleId, String identUserId)
        {
            var userrole = _context.UserRoles.Where(ur => ur.UserId == identUserId).FirstOrDefault();
            _context.UserRoles.Remove(userrole);
            _context.SaveChanges();

            String query = "Insert into AspNetUserRoles (UserId, RoleId) Values('" + identUserId + "', '" + roleId + "')";
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            _context.Database.ExecuteSqlCommand(query);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
            await _context.SaveChangesAsync();
        }

        public int CountUsers()
        {
            return _context.Userss
                .Count();
        }

        public int CountUsers(string searchString, string roleId)
        {
            if (!string.IsNullOrWhiteSpace(roleId))
            {
                var users = _context.Userss
               .Where(u => u.UserName.ToLower().Contains(searchString.ToLower()) || u.Name.ToLower().Contains(searchString.ToLower()) || u.LastName.ToLower().Contains(searchString.ToLower()))
               //.Where(u => GetUserRoleId(u.IdentUserId) == roleId)
               .ToList();

               return users.Where(u => GetUserRoleId(u.IdentUserId) == roleId)
               .Count();
            }
            return _context.Userss
                .Where(u => u.UserName.ToLower().Contains(searchString.ToLower()) || u.Name.ToLower().Contains(searchString.ToLower()) || u.LastName.ToLower().Contains(searchString.ToLower()))
                .Count();
        }

        public List<Users> GetUsers(int pagenumber, int pagesize, string searchString, string roleId)
        {
            if (!string.IsNullOrWhiteSpace(roleId))
            {
                var users = _context.Userss
                .Include(u => u.IdentUser)
                .Where(u => u.UserName.ToLower().Contains(searchString.ToLower()) || u.Name.ToLower().Contains(searchString.ToLower()) || u.LastName.ToLower().Contains(searchString.ToLower()))
                .ToList();

                return users.Where(u => GetUserRoleId(u.IdentUserId) == roleId)
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();

            }
            return _context.Userss
                .Include(u => u.IdentUser)
                .Where(u => u.UserName.ToLower().Contains(searchString.ToLower()) || u.Name.ToLower().Contains(searchString.ToLower()) || u.LastName.ToLower().Contains(searchString.ToLower()))
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
        }
    }
}
