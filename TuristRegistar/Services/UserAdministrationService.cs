using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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



        public IEnumerable<Currencies> GetAllCurrenciesFromDataBase()
        {
            return _context.Currencies.ToList();
        }


        public void AddCountableObjectAttribute(CountableObjectAttributes attribute)
        {
            _context.CountableObjectAttributes.Add(attribute);
            _context.SaveChanges();
        }
        public void RemoveCountableObjectAttribute(int id)
        {
            var cntObjAttribute = _context.CountableObjectAttributes.FirstOrDefault(coa => coa.Id == id);
            _context.RemoveRange(_context.CntObjAttributesCount.Where(c => c.CountableObjAttrId == id));
            _context.CountableObjectAttributes.Remove(cntObjAttribute);
            _context.SaveChanges();
        }
        public void EditCountableObjectAttribute(CountableObjectAttributes edittedAttribute)
        {
            var attribute = _context.CountableObjectAttributes.FirstOrDefault(c => c.Id == edittedAttribute.Id);
            attribute.Name = edittedAttribute.Name;
            _context.CountableObjectAttributes.Attach(attribute);
            _context.SaveChanges();
        }
        public void AddObjectAttributes(ObjectAttributes attribute)
        {
            _context.ObjectAttributes.Add(attribute);
            _context.SaveChanges();
        }
        public void RemoveObjectAttribute(int id)
        {
            var objAttribute = _context.ObjectAttributes.FirstOrDefault(oa => oa.Id == id);
            _context.RemoveRange(_context.ObjectHasAttributes.Where(oha => oha.AttributeId == id));
            _context.ObjectAttributes.Remove(objAttribute);
            _context.SaveChanges();
        }
        public void EditObjectAttribute(ObjectAttributes edittedAttribute)
        {
            var attribute = _context.ObjectAttributes.FirstOrDefault(a => a.Id == edittedAttribute.Id);
            attribute.Name = edittedAttribute.Name;
            _context.ObjectAttributes.Attach(attribute);
            _context.SaveChanges();
        }
        public void AddObjectType(ObjectTypes type)
        {
            _context.ObjectTypes.Add(type);
            _context.SaveChanges();
        }
        public void EditObjectTypes(ObjectTypes editedType)
        {
            var type = _context.ObjectTypes.FirstOrDefault(t => t.Id == editedType.Id);
            type.Name = editedType.Name;
            _context.ObjectTypes.Attach(type);
            _context.SaveChanges();
        }
        //check if woeks
        public void RemoveObjectType(int typeId)
        {
            var type = _context.ObjectTypes.FirstOrDefault(t => t.Id == typeId);
            var objects = _context.Objects.Where(o => o.ObjectTypeId == typeId).ToList();
            objects.ForEach(o => { o.ObjectTypeId = null; });
            _context.ObjectTypes.Remove(type);
            _context.SaveChanges();
        }
        public void AddCountry(Countries country)
        {
            _context.Counries.Add(country);
            _context.SaveChanges();
        }
        public void RemoveCountry(int countryId)
        {
            var coutnry = new Countries() { Id = countryId };
            _context.Counries.Remove(coutnry);
            _context.RemoveRange(_context.Cities.Where(c => c.CountriesId == countryId));
            _context.RemoveRange(_context.Objects.Where(c => c.CountryId == countryId));
            _context.SaveChanges();
        }
        public void EditCountry(Countries editedCountries)
        {
            var country = _context.Counries.FirstOrDefault(c => c.Id == editedCountries.Id);
            country.Name = editedCountries.Name;
            _context.Counries.Attach(country);
            _context.SaveChanges();
        }
        public void AddCity(Cities city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
        }
        public void EditCity(Cities editedCity)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == editedCity.Id);
            city.Name = editedCity.Name;
            city.Lat = editedCity.Lat;
            city.Lng = editedCity.Lng;
            city.CountriesId = editedCity.CountriesId;
            _context.Cities.Attach(city);
            _context.SaveChanges();

        }
        public void RemoveCity(int id)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == id);
            _context.Cities.Remove(city);
            _context.RemoveRange(_context.Objects.Where(c => c.CityId == id));
            _context.SaveChanges();
        }
        
        public void AddCurrency(Currencies currency)
        {
            _context.Currencies.Add(currency);
            _context.SaveChanges();
        }
        public void RemoveCurrency(int currencyId)
        {
            var currency = new Currencies() { Id = currencyId };
            _context.Currencies.Remove(currency);
            _context.SaveChanges();
        }




        public async Task<IEnumerable<String>> GetCurrenciesFromApi()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://free.currencyconverterapi.com");
                    //f49a41b74f3e1f052200 => this is my api key
                    var response = await client.GetAsync($"/api/v6/currencies?apiKey=f49a41b74f3e1f052200");

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var JSONobject = JObject.Parse(stringResult);

                    var JSONlist = JSONobject["results"].ToList();
                    var currenciesId = new List<String>();
                    foreach (var item in JSONlist)
                    {
                        string stringBeforeChar = (item.ToString()).Substring(0, (item.ToString()).IndexOf(":"));
                        
                        int firstStringPosition = (item.ToString()).IndexOf("\"")+1;
                        int secondStringPosition = (item.ToString()).IndexOf("\":");
                        string stringBetweenTwoStrings = (item.ToString()).Substring(firstStringPosition,
                            secondStringPosition - firstStringPosition);
                        currenciesId.Add(stringBetweenTwoStrings);
                    }


                    return currenciesId;
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    return null;
                    //return new KeyValuePair<string, string> ( "Error", "Error calling API. Please do manual lookup." );
                    //return "Error calling API. Please do manual lookup.";
                }
            }
        }
    }
}
