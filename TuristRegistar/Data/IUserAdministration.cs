using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Data
{
    public interface IUserAdministration
    {
        IEnumerable<IdentityRole> GetAllRoles();
        bool CheckIfUserIsBanned(String userId);
        void BanUser(String userId);
        void CancelBan(String userId);
        String GetUserRoleId(string userId);
        String GetUserRole(string userId);
        Task GiveUserRoleAsync(string userId, string role);
        Task ChangeUserRoleAsync(String roleId, String identUserId);
        int CountUsers();
        int CountUsers(string searchString, string roleId);
        List<Users> GetUsers(int pagenumber, int pagesize, string searchString, string roleId);
    }
}
