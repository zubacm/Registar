using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;
using TuristRegistar.Models;


namespace TuristRegistar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITouristObject _touristObject;
        private readonly IUser _user;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ITouristObject touristObject,  UserManager<IdentityUser> userManager, IUser user)
        {
            _touristObject = touristObject;
            _userManager = userManager;
            _user = user;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Chat()
        {
            ViewData["Message"] = "Chat page.";
            return View();
        }

        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue)
            {
                    var model = new ErrorViewModel() { RequestId = statusCode.ToString(), };
                    return View("~/Views/Shared/Error.cshtml", model);
            }
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }

        [Route("objectlist")] // /objectlist
        public async Task<IActionResult> ObjectList()
        {
            
                var model = new ObjectsViewModel();
                model.Search = new Search()
                {
                    ObjectAttributes = _touristObject.GetAllObjectAttributes()
                                .Select(atr => new ObjectAttributesModel() { Id = atr.Id, Name = atr.Name, Selected = false }).ToList(),
                    ObjectTypes = _touristObject.GetObjectTypes()
                            .Select(type => new ObjectTypeModel() { Id = type.Id, Name = type.Name, Selected = false }).ToList(),
                };


            var user = await _userManager.GetUserAsync(User);
            var bookmarks = new List<int>();
            if (user != null)
            {
                bookmarks = _user.GetAllUserBookmarksId(user.Id).ToList();
                model.IdentUserId = user.Id;
            }

            var pager = new Pager(_touristObject.CountObjects(), 1);
           
            model.Pager = pager;
            
            model.ObjectsList = _touristObject.GetObjects(pager.CurrentPage, pager.PageSize)
                               .Select(ob => new ObjectItemModel()
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
                                   IsBookmark = bookmarks.Contains(ob.Id) ? true : false,
                               }).ToList();




            return View(model);
        }


        public async Task<IActionResult> FilterObjectList(ObjectsViewModel model)
        {
            
            model.Search.CheckIn = model.Search.CheckInString == null ? DateTime.MinValue : DateTime.Parse(model.Search.CheckInString);
            model.Search.CheckOut = model.Search.CheckOutString == null ? DateTime.MinValue : DateTime.Parse(model.Search.CheckOutString);

        


            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];
            var pager = new Pager((await _touristObject.CountFilteredObjects(model.Search, currency)), 1);
            model.Pager = pager;

            var user = await _userManager.GetUserAsync(User);
            var bookmarks = new List<int>();
            if (user != null)
            {
                bookmarks = _user.GetAllUserBookmarksId(user.Id).ToList();
                model.IdentUserId = user.Id;

            }

            model.ObjectsList = (await _touristObject.GetFilteredObjects(pager.CurrentPage, pager.PageSize, model.Search, currency))
                .Select(ob => new ObjectItemModel()
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
                    IsBookmark = bookmarks.Contains(ob.Id) ? true : false,
                }).ToList();

            return PartialView("_ObjectsListed", model);
        }


        public async Task<IActionResult> ChangePage(ObjectsViewModel model)
        {
            model.Search.CheckIn = model.Search.CheckInString == null ? DateTime.MinValue : DateTime.Parse(model.Search.CheckInString);
            model.Search.CheckOut = model.Search.CheckOutString == null ? DateTime.MinValue : DateTime.Parse(model.Search.CheckOutString);

            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];
            var pager = new Pager((await _touristObject.CountFilteredObjects(model.Search, currency)), model.CurrPage);
            if (model.CurrPage == 0)
                pager.CurrentPage = 1;

            model.Pager = pager;

            var user = await _userManager.GetUserAsync(User);
            var bookmarks = new List<int>();
            if (user != null)
            {
                bookmarks = _user.GetAllUserBookmarksId(user.Id).ToList();
                model.IdentUserId = user.Id;
            }

            model.ObjectsList = (await _touristObject.GetFilteredObjects(pager.CurrentPage, pager.PageSize, model.Search, currency))
               .Select(ob => new ObjectItemModel()
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
                   IsBookmark = bookmarks.Contains(ob.Id) ? true : false,                  
               }).ToList();

            return PartialView("_ObjectsListed", model);
        }

       


        ////[Route("objectsmap")] // /objectlist
        public IActionResult Map()
        {

            var model = new ObjectsMapViewModel();
            model.Search = new Search()
            {
                ObjectAttributes = _touristObject.GetAllObjectAttributes()
                            .Select(atr => new ObjectAttributesModel() { Id = atr.Id, Name = atr.Name, Selected = false }).ToList(),
                ObjectTypes = _touristObject.GetObjectTypes()
                        .Select(type => new ObjectTypeModel() { Id = type.Id, Name = type.Name, Selected = false }).ToList(),
            };



            model.ObjectsList = _touristObject.GetAllObjects()
                               .Select(ob => new ObjectItemModel()
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



            return View(model);
        }

        //test this the last for map
        public async Task<IActionResult> FilterObjectsMap(ObjectsMapViewModel model)
        {
            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];


            model.ObjectsList = (await _touristObject.GetAllFilteredObjects(model.Search, currency))
                .Select(ob => new ObjectItemModel()
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

            if(!string.IsNullOrWhiteSpace(model.Search.SearchString))
            {
                var coordinates = _touristObject.GetCityCoordinates(model.Search.SearchString);
                if (coordinates == null)
                    coordinates = _touristObject.GetCountryCoordinates(model.Search.SearchString);

                if (coordinates != null)
                {
                    model.CenterLat = coordinates.Lat;
                    model.CenterLng = coordinates.Lng;
                }
            }

            return PartialView("ObjectsMap", model);
        }

        [Authorize(Roles = "USER")]
        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Conversation(String withIdentUserId)
        {

            //String withIdentUserId = "0f1c48be-70a8-475e-b028-b1c1204a8edd";
            var currentUserId = _userManager.GetUserId(this.User);
            var conversation = _user.GetConversationBetweenUsers(currentUserId, withIdentUserId);
            var currentUser = await _userManager.GetUserAsync(User);
            var model = new ConversationViewModel()
            {
                SenderId = currentUserId,
                ReceiverId = withIdentUserId,
                SenderUsername = (await _userManager.GetUserAsync(User)).UserName,
                ReceiverUsername = (await _userManager.FindByIdAsync(withIdentUserId)).UserName,                
            };
            var conv = _user.GetConversationBetweenUsers(model.SenderId, model.ReceiverId);
            model.ConversationId = conv != null ? conv.Id : (int?)null;
            model.Messages = conv != null ? _user.GetConversationMessages(conv.Id, 1, 8) : null;

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> LoadMessages(int conversationid, int pagenumber, int pagesize)
        {
            var messages = _user.GetConversationMessages(conversationid, pagenumber, pagesize);
            var json = JsonConvert.SerializeObject(messages);
            return Ok(json);
        }


        [Authorize]
        public async Task<IActionResult> AddMessageAsync(ConversationViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.ConversationId == null)
                {
                    var conv = new Conversations()
                    {
                        IdentUser1Id = model.SenderId,
                        IdentUser2Id = model.ReceiverId,
                    };

                    model.ConversationId = await _user.AddInitialConversationAsync(conv);
                }

                var message = new Messages()
                {
                    ConversationId = (int)model.ConversationId,
                    SendingIdentUserId = model.SenderId,
                    Message = model.Text,
                    DateTime = DateTime.Now,
                };

                //Check if initial conversation has conv.id 
                _user.AddMessageAsync(message);

                return Ok();
            }

            return Error(500);
        }

        public void Hey()
        {
            GetExchangeRate("BAM","EUR");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}


        public void SetCookieForCurrency(string currency)
        {
            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("Currency", currency, option);
        }


        //public async Task<KeyValuePair<string,string>> GetExchangeRate(string from, string to)
        public async Task<Decimal> GetExchangeRate(string from, string to)
        {
            //Examples:
            //from = "EUR"
            //to = "USD"
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://free.currencyconverterapi.com");
                    //f49a41b74f3e1f052200
                    var response = await client.GetAsync($"/api/v6/convert?q={from}_{to}&compact=y&apiKey=f49a41b74f3e1f052200");
                    //var stringResult = await response.Content.ReadAsStringAsync();
                    //dynamic data = JObject.Parse(stringResult);

                    //data = {"EUR_USD":{"val":1.140661}}
                    //I want to return 1.140661
                    //EUR_USD is dynamic depending on what from/to is
                    var stringResult = await response.Content.ReadAsStringAsync();
                    var dictResult = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(stringResult);

                    //key
                    var mykey = dictResult.ElementAt(0).Key;
                    var myval = dictResult.ElementAt(0).Value.ElementAt(0).Value;
                    //value
                    return Convert.ToDecimal(dictResult.ElementAt(0).Value.ElementAt(0).Value);
                    //return dictResult[$"{from}_{to}"]["val"];
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    
                    return 0;
                    //ovdje neki error page no connection to server
                    //return new KeyValuePair<string, string> ( "Error", "Error calling API. Please do manual lookup." );
                    //return "Error calling API. Please do manual lookup.";
                }
            }
        }
    }
}
