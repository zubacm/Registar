using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;
using TuristRegistar.Models;

namespace TuristRegistar.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IUserAdministration _userAdministration;
        private readonly ITouristObject _touristObject;

        public AdministrationController(IUserAdministration userAdministration, ITouristObject touristObject)
        {
            _userAdministration = userAdministration;
            _touristObject = touristObject;
        }


        public IActionResult UsersList(AdministrationModel model)
        {
            //Ovo ne treba jer će sve biti asinhrono filteri i mijenjanje strana
            model.SearchString = model.SearchString == null ? "" : model.SearchString;
            model.CurrPage = model.CurrPage == 0 ? 1 : model.CurrPage;

            model.Pager = new Pager(_userAdministration.CountUsers(), 1);
            var myusers = _userAdministration.GetUsers(model.Pager.CurrentPage, model.Pager.PageSize, model.SearchString, "");

            var roles = _userAdministration.GetAllRoles();
            model.UsersList = new List<UserAdministrationModel>();
            foreach (var item in myusers)
            {
                model.UsersList.Add(
                    new UserAdministrationModel()
                    {                      
                        Id = item.Id,
                        IdentUserId = item.IdentUserId,
                        UserName = item.UserName,
                        EmailAddress = item.Email,
                        ContactAddress = item.ContactAddress,
                        PhoneContact = item.PhoneNumber,
                        RoleName = _userAdministration.GetUserRole(item.IdentUserId),
                        RoleId = _userAdministration.GetUserRoleId(item.IdentUserId),
                        TypeOfUser = item.LegalPerson ? "Poslovno lice" : "Fizičko lice",  
                        Name = item.Name,
                        LastName = item.LastName,
                        Roles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Id.ToString() }).ToList(),
                    }
                );
            }


            model.Roles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Id }).ToList();
            model.Roles.Add(new SelectListItem() { Text = "Sve", Value = "" });

            //napravi model
            return View(model);
        }


        public IActionResult FilterUsersList(AdministrationModel model)
        {
            model.SearchString = model.SearchString == null ? "" : model.SearchString;
          //  model.CurrPage = model.CurrPage == 0 ? 1 : model.CurrPage;
          //Ustvari ovdje nek vraća na prvu stranu
            //Check what is selected roleid, is it roleid or role????
            model.Pager = new Pager(_userAdministration.CountUsers(model.SearchString, model.SelectedRole), 1);
            var myusers = _userAdministration.GetUsers(model.Pager.CurrentPage, model.Pager.PageSize, model.SearchString, model.SelectedRole);

            var roles = _userAdministration.GetAllRoles();

            model.UsersList = new List<UserAdministrationModel>();
            foreach (var item in myusers)
            {
                model.UsersList.Add(
                    new UserAdministrationModel()
                    {
                        Id = item.Id,
                        IdentUserId = item.IdentUserId,
                        UserName = item.UserName,
                        EmailAddress = item.Email,
                        ContactAddress = item.ContactAddress,
                        PhoneContact = item.PhoneNumber,
                        RoleName = _userAdministration.GetUserRole(item.IdentUserId),
                        RoleId = _userAdministration.GetUserRoleId(item.IdentUserId),
                        TypeOfUser = item.LegalPerson ? "Poslovno lice" : "Fizičko lice",
                        Name = item.Name,
                        LastName = item.LastName,
                        Roles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Id.ToString() }).ToList(),
                    }
                );
            }

            //Ovo ovdje vjerovatno ne treba,, check!!!!
            //var roles = _userAdministration.GetAllRoles();
            //model.Roles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Id }).ToList();
            //model.Roles.Add(new SelectListItem() { Text = "Sve", Value = "" });

            //Napravi partial
            return PartialView("_UsersListed", model);
        }

        //test this
        public IActionResult ChangePageUsersList(AdministrationModel model)
        {
            var pager =  new Pager((_userAdministration.CountUsers(model.SearchString, model.SelectedRole)), model.CurrPage);
            if (model.CurrPage == 0)
                pager.CurrentPage = 1;

            model.Pager = pager;

            var myusers = _userAdministration.GetUsers(model.Pager.CurrentPage, model.Pager.PageSize, model.SearchString, model.SelectedRole);

            var roles = _userAdministration.GetAllRoles();

            model.UsersList = new List<UserAdministrationModel>();
            foreach (var item in myusers)
            {
                model.UsersList.Add(
                    new UserAdministrationModel()
                    {
                        Id = item.Id,
                        IdentUserId = item.IdentUserId,
                        UserName = item.UserName,
                        EmailAddress = item.Email,
                        ContactAddress = item.ContactAddress,
                        PhoneContact = item.PhoneNumber,
                        RoleName = _userAdministration.GetUserRole(item.IdentUserId),
                        RoleId = _userAdministration.GetUserRoleId(item.IdentUserId),
                        TypeOfUser = item.LegalPerson ? "Poslovno lice" : "Fizičko lice",
                        Name = item.Name,
                        LastName = item.LastName,
                        Roles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Id.ToString() }).ToList(),
                    }
                );
            }

            //Ovo ovdje vjerovatno ne treba,, check!!!!
            //var roles = _userAdministration.GetAllRoles();
            //model.Roles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Id }).ToList();
            //model.Roles.Add(new SelectListItem() { Text = "Sve", Value = "" });

            //Napravi partial
            return PartialView("_UsersListed", model);
        }

        public IActionResult ChangeUserRole(String roleId, String identUserId)
        {
            //ovdje ako je banovan da ga fino banuje
            if (_userAdministration.CheckIfUserIsBanned(identUserId) && roleId != "3")
            {
                _userAdministration.CancelBan(identUserId);
            }
            if (roleId == "3")
            {
                _userAdministration.BanUser(identUserId);
            }
            //check if lockedout to remove it
            _userAdministration.ChangeUserRoleAsync(roleId, identUserId);
            return Ok();
        }


        public IActionResult Parameters()
        {
            return View();
        }


        public IActionResult _EditCountries()
        {
            var model = new CountriesEditModel()
            {
                AvailableCountries = _touristObject.GetCountries(),
            };

            return PartialView(model);
        }
        public IActionResult AddCountry(CountriesEditModel model)
        {
            var country = new Countries() { Name = model.NewCountry};
            _userAdministration.AddCountry(country);
            model.AvailableCountries = _touristObject.GetCountries();

            return PartialView("_EditCountries", model);
        }
        public IActionResult RemoveCountry(int countryId)
        {
            _userAdministration.RemoveCountry(countryId);
            //see more
            return Ok();
        }

        public IActionResult _EditCities()
        {
            var model = new CitiesEditModel()
            {
                Countries = (_touristObject.GetCountries())
                                .Select(item => new SelectListItem() { Text = item.Name, Value = item.Id.ToString() }).ToList(),
            };

            return PartialView(model);
        }
        public IActionResult GetCitiesInCountry(int countryId)
        {
            var model = new CitiesEditModel()
            {
                Countries = (_touristObject.GetCountries())
                               .Select(item => new SelectListItem() { Text = item.Name, Value = item.Id.ToString() }).ToList(),
                SelectedCountry = countryId,
            };
            model.CitiesForCountry = _touristObject.GetCitiesFromCountry(countryId);

            return PartialView("_EditCities", model);
        }
        public IActionResult CityAction(CitiesEditModel model)
        {
            switch (model.SubmitButton)
            {
                case "Dodaj":
                    return AddCity(model);
                case "Sačuvaj":
                    return EditCity(model);
            }
            return null;
        }
        public IActionResult AddCity(CitiesEditModel model)
        {
            Cities city = new Cities()
            {
                Name = model.NewCity,
                CountriesId = Convert.ToInt32(model.SelectedCountryAddModal),
                Lat = model.LatAddModal,
                Lng = model.LngAddModal,
            };
            _userAdministration.AddCity(city);
            //model.Countries = (_touristObject.GetCountries())
            //                    .Select(item => new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });

            return View("Parameters");
        }
        public IActionResult EditCity(CitiesEditModel model)
        {
            Cities city = new Cities()
            {
                Id = model.EditCityId,
                Name = model.EditCity,
                Lat = model.LatEditModal,
                Lng = model.LngEditModal,
                CountriesId = model.SelectedCountryEditModal,
            };
            _userAdministration.EditCity(city);
            return View("Parameters");
        }
        public IActionResult DeleteCity(int cityId)
        {
            _userAdministration.RemoveCity(cityId);

            return Ok();
        }
        //public IActionResult EditCity(CitiesEditModel model)
        //{

        //}

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> _EditCurrencies()
        {
            var model = new CurrencyEditModel()
            {
              AvailableCurrencies =_userAdministration.GetAllCurrenciesFromDataBase(),
              
            };
            var currenciesFromAPI = (await _userAdministration.GetCurrenciesFromApi());
            currenciesFromAPI = currenciesFromAPI.Where(item => !model.AvailableCurrencies.Any(x => x.Key == item)).ToList(); 
            model.AllCurrenciesFromAPI = currenciesFromAPI
                .Select(item => new SelectListItem() { Text = item, Value = item }).ToList(); ;
            return PartialView(model);
        }

        public async Task<IActionResult> AddCurrency(CurrencyEditModel model)
        {
            var newCurrency = new Data.Models.Currencies() { Key = model.NewCurrency };
            _userAdministration.AddCurrency(newCurrency);

            model.AvailableCurrencies = _userAdministration.GetAllCurrenciesFromDataBase();
            var currenciesFromAPI = (await _userAdministration.GetCurrenciesFromApi());
            currenciesFromAPI = currenciesFromAPI.Where(item => !model.AvailableCurrencies.Any(x => x.Key == item)).ToList();
            model.AllCurrenciesFromAPI = currenciesFromAPI
                .Select(item => new SelectListItem() { Text = item, Value = item }).ToList(); 

            return PartialView("_EditCurrencies", model);
        }
        public IActionResult RemoveCurrency(int currencyId)
        {
            _userAdministration.RemoveCurrency(currencyId);

            return Ok();
        }
    }
}