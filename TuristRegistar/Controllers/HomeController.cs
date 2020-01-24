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
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ITouristObject touristObject,  UserManager<IdentityUser> userManager)
        {
            _touristObject = touristObject;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("objectlist")] // /objectlist
        public IActionResult ObjectList()
        {
            
                var model = new ObjectsViewModel();
                model.Search = new Search()
                {
                    ObjectAttributes = _touristObject.GetAllObjectAttributes()
                                .Select(atr => new ObjectAttributesModel() { Id = atr.Id, Name = atr.Name, Selected = false }).ToList(),
                    ObjectTypes = _touristObject.GetObjectTypes()
                            .Select(type => new ObjectTypeModel() { Id = type.Id, Name = type.Name, Selected = false }).ToList(),
                };

            
            var pager = new Pager(_touristObject.CountObjects(), 1);
            //if (model.CurrPage == 0)
            //    pager.CurrentPage = pager.EndPage;


            //nekretnine = _nekretnina.GetNekretnine(pager.CurrentPage, pager.PageSize);
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
                                   Rating = _touristObject.GetAvarageRating(ob.Id),
                                   }).ToList();



            return View(model);
        }

        [HttpPost]//not used...
        public IActionResult ObjectListPost(ObjectsViewModel model)
        {

            model.Search = new Search()
            {
                ObjectAttributes = _touristObject.GetAllObjectAttributes()
                            .Select(atr => new ObjectAttributesModel() { Id = atr.Id, Name = atr.Name, Selected = false }).ToList(),
                ObjectTypes = _touristObject.GetObjectTypes()
                        .Select(type => new ObjectTypeModel() { Id = type.Id, Name = type.Name, Selected = false }).ToList(),
            };

            //model = FilterObjectList(model);

            var pager = new Pager(_touristObject.CountObjects(), model.CurrPage);
            if (model.CurrPage == 0)
                pager.CurrentPage = pager.EndPage;

            //nekretnine = _nekretnina.GetNekretnine(pager.CurrentPage, pager.PageSize);
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
                                   Rating = _touristObject.GetAvarageRating(ob.Id),
                               }).ToList();

            return View("ObjectList", model);
        }

        public async Task<IActionResult> FilterObjectList(ObjectsViewModel model)
        {
            
            model.Search.CheckIn = model.Search.CheckInString == null ? DateTime.MinValue : DateTime.Parse(model.Search.CheckInString);
            model.Search.CheckOut = model.Search.CheckOutString == null ? DateTime.MinValue : DateTime.Parse(model.Search.CheckOutString);

            //if ((model.Search.PriceBelow != 0 && (model.Search.CheckIn == DateTime.MinValue || model.Search.CheckOut == DateTime.MinValue)) || (model.Search.PriceBelow != 0 && model.Search.Occupancy == 0))
            //{
            //    //reset
            //    model.Search.PriceBelow = 0;
            //}
            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];
            var pager = new Pager((await _touristObject.CountFilteredObjects(model.Search, currency)), 1);
            model.Pager = pager;

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
                    Rating = _touristObject.GetAvarageRating(ob.Id),
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
                   Rating = _touristObject.GetAvarageRating(ob.Id),
               }).ToList();

            return PartialView("_ObjectsListed", model);
        }

        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";

            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


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
                    //return new KeyValuePair<string, string> ( "Error", "Error calling API. Please do manual lookup." );
                    //return "Error calling API. Please do manual lookup.";
                }
            }
        }
    }
}
