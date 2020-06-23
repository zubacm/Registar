using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuristRegistar.Data;
using TuristRegistar.Models;

namespace TuristRegistar.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IUserAdministration _userAdministration;

        public AdministrationController(IUserAdministration userAdministration)
        {
            _userAdministration = userAdministration;
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

    }
}