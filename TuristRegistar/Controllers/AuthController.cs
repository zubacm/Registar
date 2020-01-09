using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;
using TuristRegistar.Models;

namespace TuristRegistar.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUser _user;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, IUser user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _user = user;
        }

        [Route("register")] // /register
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")] // /register
        [HttpPost]
        public async Task<ActionResult> Register(/*[FromBody]*/ RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    Email = model.Username,
                    UserName = model.Username,//change to username
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                var userLPPP = new TuristRegistar.Data.Models.Users()
                {
                    LegalPerson = model.LegalPerson,
                    UserName = model.Username,
                    Name = model.Name,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    ContactAddress = model.ContactAddress,
                    IdentUserId = user.Id,
                };
                if (model.LegalPerson)
                    await _user.AddLegalPerson(userLPPP);
                else
                    await _user.AddLegalPerson(userLPPP);
                return Ok(new { Username = user.UserName });
            }
            return View(model);

        }


        [Route("login")] // /login
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            return View();
        }

        [Route("login")] // /login
        [HttpPost]
        public async Task<ActionResult> Login(/*[FromBody] */LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false/*Input.RememberMe*/, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    TempData["Notification"] = "Uspješno ste se prijavili";
                    return RedirectToAction("Index", "Home");
                }
                //if (result.IsLockedOut)
                //{
                //    return RedirectToPage("./Lockout");
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Neuspješan pokušaj prijave.");
                //    return Page();
                //}
            }
            return Unauthorized();
        }

        //[Route("signout")] // /logout
        //[HttpPost]
        public async Task<ActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            TempData["Notification"] = "Uspješno ste se odjavili";
            return RedirectToAction("Index", "Home");
        }

        //Authorize user
        [Route("settings")] // /login
        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult _ChangePassword()
        {

            return PartialView();
        }

        public async Task<IActionResult> _UpdateUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nemoguće je pronaći korisnika koji ima ID '{_userManager.GetUserId(User)}'.");
            }
            var model = new UserViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
            };
            return PartialView(model);
        }

        public IActionResult UpdateUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error-Notification"] = "Greška prilikom ažuriranja profila.";
                return View("Settings");
            }

            _user.ChangeUsername(model.Id, model.Username);
            TempData["Notification"] = "Ažurirali ste profil.";

            return RedirectToAction("Settings", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> _ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ne valja.");
                TempData["Error-Notification"] = "Greška prilikom mijenjanja lozinka.";
                return View("Settings");
                //return PartialView(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nemoguće je pronaći korisnika koji ima ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["Error-Notification"] = "Molimo vas da navedete ispravnu staru lozinku.";
                return View("Settings");
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["Notification"] = "Promijenili ste lozinku.";

            return RedirectToAction("Settings", "Auth");
        }


        public async Task<IActionResult> _UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nemoguće je pronaći korisnika koji ima ID '{_userManager.GetUserId(User)}'.");
            }

            var current_user = _user.GetUser(user.Id);

            UpdateProfileViewModel model = new UpdateProfileViewModel()
            {
                KorisnikId = current_user.Id,
                UserId = current_user.IdentUserId,
                Username = current_user.UserName,
                Name = current_user.Name,
                LegalPerson = current_user.LegalPerson,
                LastName = current_user.LastName,
                Email = current_user.Email,
                ContactAddress = current_user.ContactAddress,
                Phone = current_user.PhoneNumber,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error-Notification"] = "Greška prilikom ažuriranja profila.";
                return View("Settings");
            }
            Users updateUser = new Users()
            {
                Id = model.KorisnikId,
                IdentUserId = model.UserId,
                Name = model.Name,
                LastName = model.LegalPerson == true ? "" : model.LastName,
                LegalPerson = model.LegalPerson,
                PhoneNumber = model.Phone,
                ContactAddress = model.ContactAddress,
                Email = model.Email,
                UserName = model.Username,
            };

            _user.UpdateUser(updateUser);
            TempData["Notification"] = "Ažurirali ste profil.";

            return RedirectToAction("Settings", "Auth");
        }


    }
}