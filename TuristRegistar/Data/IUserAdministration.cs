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
        Task BanUser(String userId);
        Task CancelBan(String userId);
        String GetUserRoleId(string userId);
        String GetUserRole(string userId);
        Task GiveUserRoleAsync(string userId, string role);
        void ChangeUserRole(String roleId, String identUserId);
        int CountUsers();
        int CountUsers(string searchString, string roleId);
        List<Users> GetUsers(int pagenumber, int pagesize, string searchString, string roleId);

        void AddCountableObjectAttribute(CountableObjectAttributes attribute);
        void RemoveCountableObjectAttribute(int id);
        void EditCountableObjectAttribute(CountableObjectAttributes edittedAttribute);
        void AddObjectAttributes(ObjectAttributes attribute);
        void RemoveObjectAttribute(int id);
        void EditObjectAttribute(ObjectAttributes edittedAttribute);
        void AddObjectType(ObjectTypes type);
        void EditObjectTypes(ObjectTypes editedType);
        void RemoveObjectType(int typeId);
        void AddCountry(Countries country);
        void RemoveCountry(int countryId);
        void EditCountry(Countries editedCountries);
        void AddCity(Cities city);
        void EditCity(Cities editedCity);
        void RemoveCity(int id);
        void AddCurrency(Currencies currency);
        void RemoveCurrency(int currencyId);
        IEnumerable<Currencies> GetAllCurrenciesFromDataBase();
        Task<IEnumerable<String>> GetCurrenciesFromApi();
    }
}
