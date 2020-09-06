using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly ITouristObject _touristObject;
        private readonly IUserAdministration _userAdministration;


        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, IUser user, ITouristObject touristObject, IUserAdministration userAdministration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _user = user;
            _touristObject = touristObject;
            _userAdministration = userAdministration;
        }

        [Route("register")] // /register
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")] // /register
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    Email = model.Username,
                    UserName = model.Username,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "USER");
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
                TempData["Notification"] = "Uspješno ste se registrovali";
                return RedirectToAction("Index", "Home");
            }
            return View(model);

        }


        [Route("login")] // /login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [Route("login")] // /login
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false/*Input.RememberMe*/, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    TempData["Notification"] = "Uspješno ste se prijavili";
                    return RedirectToAction("Index", "Home", new { login = true });
                }
                if (result.IsLockedOut)
                {
                    var unsucessfulmodel = new ErrorViewModel() { RequestId = 403.ToString(), };
                    return View("~/Views/Shared/Error.cshtml", unsucessfulmodel);
                }
            }
            var errormodel = new ErrorViewModel() { RequestId = 403.ToString(), };
            return View("~/Views/Shared/Error.cshtml", errormodel);
        }


        //[Route("signout")] // /logout
        //[HttpPost]
        public async Task<ActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            TempData["Notification"] = "Uspješno ste se odjavili";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [Route("settings")] // /settings
        public IActionResult Settings()
        {
            return View();
        }


        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UserSettingsAdmin(String identUserId)
        {
            var user = await _userManager.FindByIdAsync(identUserId);
            var roles = _userAdministration.GetAllRoles();
            var model = new UserSettingsAdminModel()
            {
                IdentUserId = identUserId,
                Username = user.UserName,
                Roles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Id.ToString() }).ToList(),
                SelectedRoleId = _userAdministration.GetUserRoleId(user.Id),
            };
            return View(model);
        }

        [Authorize]
        public IActionResult _ChangePassword()
        {

            return PartialView();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> _ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ne valja.");
                TempData["Error-Notification"] = "Greška prilikom mijenjanja lozinka.";
                return View("Settings");
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

        [Authorize]
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


        [Authorize]
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

        

        [Authorize]
        public async Task<IActionResult> _UpdateProfile(String identUserId)
        {
            IdentityUser user;
            if(identUserId == null)
                user = await _userManager.GetUserAsync(User);
            else
                user = await _userManager.FindByIdAsync(identUserId.ToString());

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
                AdminAction = identUserId == null ? false : true,
            };

            return View(model);
        }


        [HttpPost]
        [Authorize]
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

            if (model.AdminAction)
                return RedirectToAction("UserSettingsAdmin", "Auth", new { identUserId = model.UserId});
            return RedirectToAction("Settings", "Auth");
        }

        [Authorize]
        public IActionResult UserAccount()
        {
            return View();
        }

       // [Authorize]
        public async Task<IActionResult> _UserObjects(String identUserId)
        {
            IdentityUser user;
            if(identUserId == null)
              user = await _userManager.GetUserAsync(User);
            else
              user = await _userManager.FindByIdAsync(identUserId);

            if (user == null)
            {
                return NotFound($"Nemoguće je pronaći korisnika koji ima ID '{_userManager.GetUserId(User)}'.");
            }

                
            var model = new UserObjectsModel()
            {
                IdentUserId = user.Id,             
            };
            var pager = new Pager(_user.CountUserObjects(user.Id), 1);
            model.Pager = pager;

            model.ObjectsList = _user.GetUserObjects(user.Id, pager.CurrentPage, pager.PageSize).Select(ob => new ObjectItemModel()
            {
                Id = ob.Id,
                Name = ob.Name,
                Location = ob.City != null ? ob.Address + ", " + ob.City.Name : ob.Address,
                ImgSrc = ob.ObjectImages.Count > 0 ? ob.ObjectImages.ElementAt(0).Path : "/pink.png",
                Lat = ob.Lat,
                Lng = ob.Lng,
                Description = ob.Description,
                WebContact = ob.WebContact,
                EmailContact = ob.EmailContact,
                PhoneNumberContact = ob.PhoneNumberContact,
                Type = ob.ObjectType == null ? "" : ob.ObjectType.Name,
                NumberOfRatings = _touristObject.GetNumberOfRatings(ob.Id),
                Rating = Math.Round(_touristObject.GetAvarageRating(ob.Id), 2),
            }).ToList();
            return PartialView("_UserObjects", model);
        }

        [Authorize]
        public IActionResult UserObjectsChangePage(UserObjectsModel model)
        {

            var pager = new Pager(_user.CountUserObjects(model.IdentUserId), model.CurrPage);

            if (model.CurrPage == 0)
                pager.CurrentPage = 1;
            model.Pager = pager;



            model.ObjectsList = _user.GetUserObjects(model.IdentUserId, model.Pager.CurrentPage, model.Pager.PageSize).Select(ob => new ObjectItemModel()
            {
                Id = ob.Id,
                Name = ob.Name,
                Location = ob.City != null ? ob.Address + ", " + ob.City.Name : ob.Address,
                ImgSrc = ob.ObjectImages.Count > 0 ? ob.ObjectImages.ElementAt(0).Path : "/pink.png",
                Lat = ob.Lat,
                Lng = ob.Lng,
                Description = ob.Description,
                WebContact = ob.WebContact,
                EmailContact = ob.EmailContact,
                PhoneNumberContact = ob.PhoneNumberContact,
                Type = ob.ObjectType == null ? "" : ob.ObjectType.Name,
                NumberOfRatings = _touristObject.GetNumberOfRatings(ob.Id),
                Rating = Math.Round(_touristObject.GetAvarageRating(ob.Id), 2),
            }).ToList();

            return PartialView("_UserObjects", model);

        }

        [Authorize]
        public async Task<IActionResult> _UserBookmarks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nemoguće je pronaći korisnika koji ima ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new UserBookmarksModel()
            {
                IdentUserId = user.Id,
            };
            var pager = new Pager(_user.CountUserBookmarks(user.Id), 1);
            model.Pager = pager;
            model.Bookmarks = _user.GetUserBookmarks(user.Id, pager.CurrentPage, pager.PageSize).Select(ob => new ObjectItemModel()
            {
                Id = ob.Id,
                Name = ob.Name,
                Location = ob.City != null ? ob.Address + ", " + ob.City.Name : ob.Address,
                ImgSrc = ob.ObjectImages.Count > 0 ? ob.ObjectImages.ElementAt(0).Path : "/pink.png",
                Lat = ob.Lat,
                Lng = ob.Lng,
                Description = ob.Description,
                WebContact = ob.WebContact,
                EmailContact = ob.EmailContact,
                PhoneNumberContact = ob.PhoneNumberContact,
                Type = ob.ObjectType == null ? "" : ob.ObjectType.Name,
                NumberOfRatings = _touristObject.GetNumberOfRatings(ob.Id),
                Rating = Math.Round(_touristObject.GetAvarageRating(ob.Id), 2),

            }).ToList();

            //napravi view
            return PartialView("_UserBookmarks", model);
        }

        [Authorize]
        public async Task<IActionResult> UserBookmarksChangePage(UserBookmarksModel model)
        {


            var pager = new Pager(_user.CountUserBookmarks(model.IdentUserId), model.CurrPage);

            if (model.CurrPage == 0)
                pager.CurrentPage = 1;
            model.Pager = pager;


            model.Bookmarks = _user.GetUserBookmarks(model.IdentUserId, pager.CurrentPage, pager.PageSize).Select(ob => new ObjectItemModel()
            {
                Id = ob.Id,
                Name = ob.Name,
                Location = ob.City != null ? ob.Address + ", " + ob.City.Name : ob.Address,
                ImgSrc = ob.ObjectImages.Count > 0 ? ob.ObjectImages.ElementAt(0).Path : "/pink.png",
                Lat = ob.Lat,
                Lng = ob.Lng,
                Description = ob.Description,
                WebContact = ob.WebContact,
                EmailContact = ob.EmailContact,
                PhoneNumberContact = ob.PhoneNumberContact,
                Type = ob.ObjectType == null ? "" : ob.ObjectType.Name,
                NumberOfRatings = _touristObject.GetNumberOfRatings(ob.Id),
                Rating = Math.Round(_touristObject.GetAvarageRating(ob.Id), 2),

            }).ToList();

            return PartialView("_UserBookmarks", model);

        }


        [Authorize]
        public IActionResult AddBookmark(String identUserId, int objectId)
        {
            _user.AddBookmark(identUserId, objectId);

            return Ok();
        }

        [Authorize]
        public IActionResult RemoveBookmark(String identUserId, int objectId)
        {
            _user.RemoveBookmark(identUserId, objectId);

            return Ok();
        }

        public async Task<IActionResult> UserDetails(String identUserId)
        {
            var user = await _userManager.FindByIdAsync(identUserId.ToString());

            if (user == null)
            {
                return NotFound($"Nemoguće je pronaći korisnika koji ima ID '{_userManager.GetUserId(User)}'.");
            }
            var current_user = _user.GetUser(user.Id);

            var model = new UserDetails() {
                IdentUserId = identUserId,
                KorisnikId = current_user.Id,
                //UserId = current_user.IdentUserId,
                Username = current_user.UserName,
                Name = current_user.Name,
                LegalPerson = current_user.LegalPerson,
                LastName = current_user.LastName,
                Email = current_user.Email,
                ContactAddress = current_user.ContactAddress,
                Phone = current_user.PhoneNumber,
                AdminAction = identUserId == null ? false : true,
            };

            return View(model);
        }

        [Authorize]
        public IActionResult GetCurrentUserIdentId()
        {
            var userid = _userManager.GetUserId(User);

            return Ok(userid);
        }

    }
}