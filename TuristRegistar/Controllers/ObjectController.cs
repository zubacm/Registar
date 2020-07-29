using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;
using TuristRegistar.Models;

namespace TuristRegistar.Controllers
{
    public class ObjectController : Controller
    {
        private readonly ITouristObject _touristObject;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUser _user;


        public ObjectController(ITouristObject touristObject, IUser user, IHostingEnvironment hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _touristObject = touristObject;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _user = user;
        }

       
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("createobject")] // /createobject
        public IActionResult CreateObject()

        {
            var model = new CreateObjectViewModel();
            model = FillSelectLists(model);
            DeleteAllTempImages();

            return View(model);
        }

        private CreateObjectViewModel FillSelectLists(CreateObjectViewModel model)
        {
            IEnumerable<Countries> countries = _touristObject.GetCountries();
            model.Countries = countries.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            if (model.SelectedCountry != null)
            {
                IEnumerable<Cities> cities = _touristObject.GetCitiesFromCountry(Convert.ToInt32(model.SelectedCountry));
                model.Cities = cities.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            }
            IEnumerable<ObjectTypes> objecttypes = _touristObject.GetObjectTypes();
            model.ObjectTypes = objecttypes.Select(ot => new SelectListItem() { Text = ot.Name, Value = ot.Id.ToString() }).ToList();

            //attributes are for attributes and specialoffers
            IEnumerable<ObjectAttributes> attributes = _touristObject.GetAllObjectAttributes();
            model.Offers = attributes.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            model.SpecialOffers = model.Offers;

            IEnumerable<CountableObjectAttributes> cntOffers = _touristObject.GetAllCountableObjectAttributes();
            model.CountableOffers = cntOffers.Select(co => new SelectListItem() { Text = co.Name, Value = co.Id.ToString() }).ToList();

            return model;
        }
        private EditObjectViewModel FillSelectLists(EditObjectViewModel model)
        {
            IEnumerable<Countries> countries = _touristObject.GetCountries();
            model.Countries = countries.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            if (!String.IsNullOrWhiteSpace(model.SelectedCountry))
            {
                IEnumerable<Cities> cities = _touristObject.GetCitiesFromCountry(Convert.ToInt32(model.SelectedCountry));
                model.Cities = cities.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            }
            IEnumerable<ObjectTypes> objecttypes = _touristObject.GetObjectTypes();
            model.ObjectTypes = objecttypes.Select(ot => new SelectListItem() { Text = ot.Name, Value = ot.Id.ToString() }).ToList();

            //attributes are for attributes and specialoffers
            IEnumerable<ObjectAttributes> attributes = _touristObject.GetAllObjectAttributes();
            model.Offers = attributes.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            model.SpecialOffers = model.Offers;

            IEnumerable<CountableObjectAttributes> cntOffers = _touristObject.GetAllCountableObjectAttributes();
            model.CountableOffers = cntOffers.Select(co => new SelectListItem() { Text = co.Name, Value = co.Id.ToString() }).ToList();

            return model;
        }


        public IActionResult GetObjectAttributes(string excludedAttributesId)
        {
            if (!string.IsNullOrWhiteSpace(excludedAttributesId))
            {
                var excludedAttributesIdList = _touristObject.ParseStringToIDsList(excludedAttributesId);
                IEnumerable<ObjectAttributes> attributes = _touristObject.GetObjectAttributes(excludedAttributesIdList);
                var objAttributes = attributes.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
                return Ok(objAttributes);
            }
            else
            {
                IEnumerable<ObjectAttributes> attributes = _touristObject.GetAllObjectAttributes();
                var objAttributes = attributes.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
                return Ok(objAttributes);
            }
        }

        public IActionResult GetObjectCntAttributes(string excludedAttributesId)
        {
            if (!string.IsNullOrWhiteSpace(excludedAttributesId))
            {
                var excludedAttributesIdList = _touristObject.ParseStringToIDsList(excludedAttributesId);
                IEnumerable<CountableObjectAttributes> attributes = _touristObject.GetCountableObjectAttributes(excludedAttributesIdList);
                var cntObjAttributes = attributes.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
                return Ok(cntObjAttributes);
            }
            else
            {
                IEnumerable<CountableObjectAttributes> cntAttributes = _touristObject.GetAllCountableObjectAttributes();
                var cntObjAttributes = cntAttributes.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
                return Ok(cntObjAttributes);
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("createobject")] // /createobject
        public async Task<IActionResult> CreateObject(CreateObjectViewModel model)
        {

            if(!string.IsNullOrWhiteSpace(model.AddedOffers))
                model.ListOfAddedOffers = (_touristObject.ParseStringToIDsList(model.AddedOffers))
                    .Select(item => new ObjectHasAttributes() { AttributeId = item}).ToList();
            if (!string.IsNullOrWhiteSpace(model.AddedCountableOffers))
                model.ListOfAddedCntOffers = (_touristObject.ParseStringToKeyValueList(model.AddedCountableOffers))
                    .Select(item => new CntObjAttributesCount() { CountableObjAttrId = item.Key, Count = item.Value }).ToList();
            if (!string.IsNullOrWhiteSpace(model.AddedSpecialOffers))
                model.ListOfAddedSpecialOffers = (_touristObject.ParseStringToKeyValue(model.AddedSpecialOffers))
                    .Select(item => new SpecialOffersPrices() { SpecialOfferId = item.Key, Price = item.Value }).ToList();
            if (model.OccupancyPricing && (!string.IsNullOrWhiteSpace(model.OccubancBasedPrices)))
                model.OccupancyBasedPricing.Prices = (_touristObject.ParseStringToKeyValue(model.OccubancBasedPrices))
                    .Select(item => new OccupancyBasedPrices() { Occupancy = item.Key, PricePerNight = item.Value }).ToList();
            if (!string.IsNullOrWhiteSpace(model.UnavailablePeriodsString))
                model.UnavailablePeriods = (_touristObject.ParseDates(model.UnavailablePeriodsString))
                    .Select(item => new UnavailablePeriods() { From = item.Key, To = item.Value }).ToList();

            if ((!ModelState.IsValid) || (!CheckPricing(model)))
            {
                TempData["Error-Notification"] = "Ispravno popunite neophodna polja!";
                model = FillSelectLists(model);
                model = GetAttributes(model);
                DeleteAllTempImages();
                return View(model);
            }

            Objects newobject = new Objects()
            {
                Name = model.Name,
                Address = model.Address,
                EmailContact = model.EmailContact,
                PhoneNumberContact = model.PhoneNumberContact,
                WebContact = model.WebContact,
                Description = model.Description,
                ObjectHasAttributes = model.ListOfAddedOffers == null ? new List<ObjectHasAttributes>() : model.ListOfAddedOffers.Select(item => new ObjectHasAttributes() { AttributeId = item.AttributeId }).ToList(),
                CntObjAttributesCount = model.ListOfAddedCntOffers == null ? new List<CntObjAttributesCount>() : model.ListOfAddedCntOffers,
                SpecialOffers = model.ListOfAddedSpecialOffers == null ? new List<SpecialOffersPrices>() : model.ListOfAddedSpecialOffers,
                CountryId = Convert.ToInt32(model.SelectedCountry) == 0 ? null : (int?)Convert.ToInt32(model.SelectedCountry),
                CityId = Convert.ToInt32(model.SelectedCity) == 0 ? null : (int?)Convert.ToInt32(model.SelectedCity),
                ObjectTypeId = Convert.ToInt32(model.SelectedObjectType) == 0 ? null : (int?)Convert.ToInt32(model.SelectedObjectType),
                Lat = model.Lat,
                Lng = model.Lng,
                UnavailablePeriods = model.UnavailablePeriods == null ? new List<UnavailablePeriods>() : model.UnavailablePeriods,
                OccupancyPricing = model.OccupancyPricing,
                StandardPricingModel = (!model.OccupancyPricing) ? model.StandardPricingModel : null,
                OccupancyBasedPricing = model.OccupancyPricing ? model.OccupancyBasedPricing : null,
                FullAddress = model.Address + (Convert.ToInt32(model.SelectedCity) == 0 ? "" : _touristObject.GetCityName(Convert.ToInt32(model.SelectedCity)))
                + (Convert.ToInt32(model.SelectedCity) == 0 ? "" : _touristObject.GetCoutnryName(Convert.ToInt32(model.SelectedCountry))),
                ////CreatorId = null,
                IdentUserId = _userManager.GetUserId(this.User),
                ObjectImages = CopyFiles(Path.Combine(_hostingEnvironment.WebRootPath, "Temp"), Path.Combine(_hostingEnvironment.WebRootPath, "UploadedImages")),
            };
            if (model.Surface != null)
                newobject.Surface = (float)model.Surface;

            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];
            await _touristObject.AddObject(newobject, currency);

            TempData["Notification"] = "Uspješno ste dodali objekt";
            return Redirect("createobject");
        }

        //[Authorize]
        [Route("editobject")] // /editobject
        public async Task<IActionResult> EditObject(int id)
        {
            //if user admin or owns object
            //Napravit servis da li korisik owns object
            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];
            var myobject = await _touristObject.GetObject(id, currency);

            var model = new EditObjectViewModel()
            {
                Id = myobject.Id,
                Name = myobject.Name,
                Lat = myobject.Lat,
                Lng = myobject.Lng,
                Address = myobject.Address,
                EmailContact = myobject.EmailContact,
                PhoneNumberContact = myobject.PhoneNumberContact,
                WebContact = myobject.WebContact,
                Description = myobject.Description,
                UnavailablePeriods = myobject.UnavailablePeriods.Where(up => DateTime.Compare(DateTime.Now, up.To) <= 0).ToList(),
                ListOfAddedOffers = myobject.ObjectHasAttributes.ToList(),
                ListOfAddedCntOffers = myobject.CntObjAttributesCount.ToList(),
                ListOfAddedSpecialOffers = myobject.SpecialOffers.ToList(),
                Surface = myobject.Surface,
                OccupancyPricing = myobject.OccupancyPricing,
                OccupancyBasedPricing = myobject.OccupancyBasedPricing,
                StandardPricingModel = myobject.StandardPricingModel,
                IdentUserId = myobject.IdentUserId,
                SelectedCity = myobject.CityId == null ? null : myobject.CityId.ToString(),
                SelectedCountry = myobject.CountryId == null ? null : myobject.CountryId.ToString(),
                SelectedObjectType = myobject.ObjectTypeId == null ? null : myobject.ObjectTypeId.ToString(),
                ImgsSrc = myobject.ObjectImages.ToList(),
            };
            model = FillSelectLists(model);

            return View(model);
        }


        //try private
        public IActionResult GetCitiesInCountry(int countryId)
        {
            var cities = _touristObject.GetCitiesFromCountry(countryId);
            var citiesSelectList = cities.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            return Ok(citiesSelectList);
        }


        public async Task<JsonResult> ImageUploadTemp(CreateObjectViewModel model)
        {

            var file = model.ImageFile;

            if (file != null)
            {

                var fileName = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamewithoutextension = Path.GetFileNameWithoutExtension(file.FileName);

                var path  = Path.Combine(_hostingEnvironment.WebRootPath, "Temp" , file.FileName);

                //file.SaveAs(path);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return Json(file.FileName);

        }

        public async Task<JsonResult> AddNewImage(EditObjectViewModel model)
        {

            var file = model.ImageFile;
            var filename = file.FileName;

            if (file != null)
            {
                try
                {
                    if (System.IO.File.Exists(Path.Combine(_hostingEnvironment.WebRootPath, "UploadedImages", filename)))
                    {
                        var extension = Path.GetExtension(filename);
                        //unique file name
                        filename = string.Format(@"{0}" + extension, Guid.NewGuid());

                    }
                    var path = Path.Combine(_hostingEnvironment.WebRootPath, "UploadedImages", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var img = new ObjectImages()
                    {
                        Path = "/UploadedImages/" + filename,
                       
                    };

                    var imgid = _touristObject.AddImage(img, model.Id);
                    var response = @"{imgname:'" + filename + "',imgid:" + imgid+"}";
                    JObject json = JObject.Parse(response);
                    var oks = Json(json);

                    return Json(json);
                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.WriteLine("Error while copying");
                }
            }


            return Json("Not found");
        }

        public IActionResult DeleteImage(EditObjectViewModel model)
        {
            _touristObject.DeleteImage(model.DeleteImageId);
            var relativelocation = model.DeleteImagePath.Remove(0, 1);
            var path = Path.Combine(_hostingEnvironment.WebRootPath, relativelocation);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            return Ok();
        }

        public IActionResult DeleteImageTemp(CreateObjectViewModel model)
        {
            var relativelocation = model.DeleteImagePath.Remove(0, 1);
            var path = Path.Combine(_hostingEnvironment.WebRootPath, relativelocation);
            if(System.IO.File.Exists(path))
                 System.IO.File.Delete(path);

            return Ok();
        }



        private bool CheckPricing(CreateObjectViewModel model)
        {
            if ((!model.OccupancyPricing) &&
                ((model.StandardPricingModel.StandardOccupancy == null || model.StandardPricingModel.OffsetPercentage == null
                || model.StandardPricingModel.MinOccupancy == null || model.StandardPricingModel.MaxOccupancy == null) 
                || model.StandardPricingModel.StandardOccupancy < model.StandardPricingModel.MinOccupancy || model.StandardPricingModel.StandardOccupancy > model.StandardPricingModel.MaxOccupancy))
            {
                return false;
            }
            if (model.OccupancyPricing && (model.OccupancyBasedPricing.MaxOccupancy == null || model.OccupancyBasedPricing.MinOccupancy == null || model.OccubancBasedPrices == null))
            {
                return false;
            }
            return true;
        }
        private bool CheckPricing(EditObjectViewModel model)
        {
            if ((!model.OccupancyPricing) &&
                ((model.StandardPricingModel.StandardOccupancy == null || model.StandardPricingModel.OffsetPercentage == null
                || model.StandardPricingModel.MinOccupancy == null || model.StandardPricingModel.MaxOccupancy == null)
                || model.StandardPricingModel.StandardOccupancy < model.StandardPricingModel.MinOccupancy || model.StandardPricingModel.StandardOccupancy > model.StandardPricingModel.MaxOccupancy))
            {
                return false;
            }
            if (model.OccupancyPricing && (model.OccupancyBasedPricing.MaxOccupancy == null || model.OccupancyBasedPricing.MinOccupancy == null || model.OccubancBasedPrices == null))
            {
                return false;
            }
            return true;
        }


        private CreateObjectViewModel GetAttributes(CreateObjectViewModel model)
        {
            if (model.ListOfAddedOffers != null)
            {
                foreach (var item in model.ListOfAddedOffers)
                {
                    item.Attribute = _touristObject.GetObjectAttribute(item.AttributeId);
                }
            }
            if (model.ListOfAddedCntOffers != null)
            {
                foreach (var item in model.ListOfAddedCntOffers)
                {
                    item.CountableObjAttr = _touristObject.GetCountableObjectAttribute(item.CountableObjAttrId);
                }
            }
            if (model.ListOfAddedSpecialOffers != null)
            {
                foreach (var item in model.ListOfAddedSpecialOffers)
                {
                    item.SpecialOffer = _touristObject.GetObjectAttribute(item.SpecialOfferId);
                }
            }

            return model;
        }

        //Bili su u temp folderu i na save se sačuvavaju 
        public List<ObjectImages> CopyFiles(string sourcePath, string destinationPath)
        {
            var imgs = new List<ObjectImages>();
            
            //System.IO.Directory.CreateDirectory(destinationPath);
            try
            {
                foreach (var file in new DirectoryInfo(sourcePath).GetFiles())
                {
                    var filename = file.Name;
                    if (System.IO.File.Exists(Path.Combine(destinationPath, file.Name)))
                    {
                        var extension = Path.GetExtension(file.Name);
                        //unique file name
                        filename = string.Format(@"{0}." + extension, Guid.NewGuid());

                    }
                    file.CopyTo(Path.Combine(destinationPath, filename));
                    var img = new ObjectImages()
                    {
                        Path = "/UploadedImages/" + filename
                    };
                    imgs.Add(img);
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Error while copying");
            }


            DeleteAllTempImages();


            return imgs;
        }


        public void DeleteAllTempImages()
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Temp");
            try
            {
                foreach (var file in new DirectoryInfo(path).GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Error while deleteing");
            }
        }

        [Authorize]
        public IActionResult DeletePeriod(EditObjectViewModel model)
        {
            //Admin ili owner
            _touristObject.DeletePeriod(model.DeletePeriodId);

            return Ok();
        }

        [Authorize]
        public IActionResult AddPeriod(EditObjectViewModel model)
        {
            //admin ili owner
            if (!string.IsNullOrWhiteSpace(model.UnavailablePeriodsString))
                model.UnavailablePeriods = (_touristObject.ParseDates(model.UnavailablePeriodsString))
                    .Select(item => new UnavailablePeriods() { From = item.Key, To = item.Value }).ToList();
            var id = _touristObject.AddPeriod(model.UnavailablePeriods[0], model.Id);

            return Json(id);
        }


        [HttpPost]
        [Authorize]//admin ili owner
        public IActionResult EditOccupancyBased(EditObjectViewModel model)
        {
            model.OccupancyPricing = true;
            if ( (!string.IsNullOrWhiteSpace(model.OccubancBasedPrices)))
                model.OccupancyBasedPricing.Prices = (_touristObject.ParseStringToKeyValue(model.OccubancBasedPrices))
                    .Select(item => new OccupancyBasedPrices() { Occupancy = item.Key, PricePerNight = item.Value }).ToList();

            if (!CheckPricing(model))
            {
                TempData["Error-Notification"] = "Ispravno popunite neophodna polja!";
                //redirect to edit umjesto ovog
                return View(model);
            }
            //var occupancyBasedPricing = model.OccupancyBasedPricing;
            //ili možda null
            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];

            if (model.OccupancyBasedPricing.Id != 0)
            {//i ovdje currency
                _touristObject.NewOccupancyBasedPricing(model.OccupancyBasedPricing, model.Id, currency);
            }
            else
            {
                _touristObject.DeleteStandardModel(model.Id);//ovdje treba currency converting
                _touristObject.AddOccupancyBasedModel(model.OccupancyBasedPricing, model.Id, currency);
                //add occupancy
            }
            ViewData["Notification"] = "Uspješno sačuvane izmjene";
            return null;
            //return RedirectToAction("Index", "Object");
        }

        [Authorize]//admin ili owner ili jednostavno stavit httppost
        public IActionResult EditStandardModel(EditObjectViewModel model)
        {
            model.OccupancyPricing = false;

            if (!CheckPricing(model))
            {
                TempData["Error-Notification"] = "Ispravno popunite neophodna polja!";
                //redirect to edit umjesto ovog
                return View(model);
            }

            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];
            //test test when done
            if (model.StandardPricingModel.Id == 0)
            {
                _touristObject.DeleteOccupancyBasedModel(model.Id);//isto currency
                _touristObject.AddStandardModel(model.StandardPricingModel, model.Id, currency);
            }
            else
            {
                _touristObject.EditStandardModel(model.StandardPricingModel, currency);
            }

            //ove nalove
            return null;
        }

        [Authorize]//isto
        public IActionResult DeleteObjectHasAttribute(EditObjectViewModel model)
        {
            _touristObject.DeleteObjectHasAttributes(model.Id, model.DeleteAttributeId);
            return Ok();
        }

        [Authorize]
        public IActionResult AddObjectHasAttribute(EditObjectViewModel model)
        {
            var objHasAttribute = new ObjectHasAttributes()
            {
                ObjectId = model.Id,
                AttributeId = model.AddAttributeId,
            };
            _touristObject.AddObjectHasAttribute(objHasAttribute);
            return Ok();
        }

        [Authorize]//Isto
        public IActionResult AddCntAttribute(EditObjectViewModel model)
        {
            var cntObjAttribute = new CntObjAttributesCount()
            {
                ObjectId = model.Id,
                CountableObjAttrId = model.AddCntAttributeId,
                Count = model.AddCntAttributeValue,
            };
            _touristObject.AddCntAttributeCount(cntObjAttribute);
            return Ok();
        }
        [Authorize]//isto
        public IActionResult DeleteCntAttribute(EditObjectViewModel model)
        {
            _touristObject.DeleteCntAttributeCount(model.Id, model.DeleteCntAttributeId);
            return Ok();
        }

        [Authorize]//isto
        public IActionResult AddSpecialOffer(EditObjectViewModel model)
        {
            var specialoffer = new SpecialOffersPrices()
            {
                ObjectId = model.Id,
                SpecialOfferId = model.AddSpecialOfferId,
                Price = model.AddSpecialOfferValue,
            };
            _touristObject.AddSpecialOffer(specialoffer);
            return Ok();
        }
        [Authorize]//isto
        public IActionResult DeleteSpecialOffer(EditObjectViewModel model)
        {
            _touristObject.DeleteSpecialOffer(model.Id, model.DeleteSpecialOfferId);
            return Ok();
        }

        [Authorize]//isto
        public IActionResult EditObjectBasic(EditObjectViewModel model)
        {
            Objects myobject = new Objects()
            {
                Id = model.Id,
                Name = model.Name,
                Lat = model.Lat,
                Lng = model.Lng,
                Address = model.Address,
                EmailContact = model.EmailContact,
                PhoneNumberContact = model.PhoneNumberContact,
                WebContact = model.WebContact,
                Description = model.Description,
                Surface = model.Surface,
                CountryId = Convert.ToInt32(model.SelectedCountry) == 0 ? null : (int?)Convert.ToInt32(model.SelectedCountry),
                CityId = Convert.ToInt32(model.SelectedCity) == 0 ? null : (int?)Convert.ToInt32(model.SelectedCity),
                ObjectTypeId = Convert.ToInt32(model.SelectedObjectType) == 0 ? null : (int?)Convert.ToInt32(model.SelectedObjectType),
                FullAddress = model.Address + (Convert.ToInt32(model.SelectedCity) == 0 ? "" : _touristObject.GetCityName(Convert.ToInt32(model.SelectedCity)))
                + (Convert.ToInt32(model.SelectedCity) == 0 ? "" : _touristObject.GetCoutnryName(Convert.ToInt32(model.SelectedCountry))),
            };
            _touristObject.EditObjectBasic(myobject);
            ViewData["Notification"] = "Uspješno sačuvane izmjene";
            return RedirectToAction("EditObject", "Object", new { id = model.Id });
        }

        public async Task<IActionResult> ObjectDetails(int id, int occupancy, DateTime checkin, DateTime checkout)
        {
            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];
            var myobject = await _touristObject.GetObject(id, currency);


            var model = new ObjectDetailsViewModel()
            {
                Id = myobject.Id,
                Name = myobject.Name,
                Lat = myobject.Lat,
                Lng = myobject.Lng,
                Address = myobject.Address,
                EmailContact = myobject.EmailContact,
                PhoneNumberContact = myobject.PhoneNumberContact,
                WebContact = myobject.WebContact,
                Description = myobject.Description,
                Offers = myobject.ObjectHasAttributes.ToList(),
                CntOffers = myobject.CntObjAttributesCount.ToList(),
                SpecialOffers = myobject.SpecialOffers.ToList(),
                Surface = myobject.Surface,
                OccupancyPricing = myobject.OccupancyPricing,
                OccupancyBasedPricing = myobject.OccupancyBasedPricing,
                StandardPricingModel = myobject.StandardPricingModel,
                StandardPricingModelId = myobject.StandardPricingModelId,
                OccupancyBasedPricingId = myobject.OccupancyBasedPricingId,
                IdentUserId = myobject.IdentUserId,
                City = myobject.CityId == null ? null : myobject.CityId.ToString(),
                Country = myobject.CountryId == null ? null : myobject.CountryId.ToString(),
                ObjectType = myobject.ObjectType == null ? null : myobject.ObjectType.Name,
                ImgsSrc = myobject.ObjectImages.ToList(),
                CheckIn = checkin,
                CheckOut = checkout,
                CheckInString = checkin == DateTime.MinValue ? "" : checkin.Year + "-"+checkin.Month+"-"+checkin.Day,
                CheckOutString = checkout == DateTime.MinValue ? "" : checkout.Year+"-"+checkout.Month+"-"+checkout.Day,
                SelectedOccupancy = occupancy,
                MinOccupancy = myobject.StandardPricingModel == null ? (myobject.OccupancyBasedPricing != null && myobject.OccupancyBasedPricing.MinOccupancy != null ? (int)myobject.OccupancyBasedPricing.MinOccupancy : 1) : (myobject.StandardPricingModel.MinOccupancy != null ? (int)myobject.StandardPricingModel.MinOccupancy : 1),
                MaxOccupancy = myobject.StandardPricingModel == null ? (myobject.OccupancyBasedPricing != null && myobject.OccupancyBasedPricing.MaxOccupancy != null ? (int)myobject.OccupancyBasedPricing.MaxOccupancy : 30) : (myobject.StandardPricingModel.MaxOccupancy != null ? (int)myobject.StandardPricingModel.MaxOccupancy : 30),
                MinDaysOffer = myobject.StandardPricingModel == null ? (myobject.OccupancyBasedPricing != null && myobject.OccupancyBasedPricing.MinDaysOffer != null ? (int)myobject.OccupancyBasedPricing.MinDaysOffer : 1) : (myobject.StandardPricingModel.MinDaysOffer != null ? (int)myobject.StandardPricingModel.MinDaysOffer : 1),
                MaxDaysOffer = myobject.StandardPricingModel == null ? (myobject.OccupancyBasedPricing != null && myobject.OccupancyBasedPricing.MaxDaysOffer != null ? (int)myobject.OccupancyBasedPricing.MaxDaysOffer : 355) : (myobject.StandardPricingModel.MaxDaysOffer != null ? (int)myobject.StandardPricingModel.MaxDaysOffer : 355),
                UnavailablePeriods = myobject.UnavailablePeriods,
                NumberOfRatings = _touristObject.GetNumberOfRatings(myobject.Id),
                Rating = Math.Round(_touristObject.GetAvarageRating(myobject.Id), 2),
                CreatorIdentUserId = myobject.IdentUserId,
            };
            Users myuser = _user.GetUserFromIdentUser(myobject.IdentUserId);
            model.CreatorId = myuser.Id;
            model.CreatorName = myuser.Name + " " + myuser.LastName;


            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var bookmarks = _user.GetAllUserBookmarksId(user.Id).ToList();
                model.IsBookmark = bookmarks.Contains(model.Id) ? true : false;
            }


            model.Occupancy = Enumerable.Range(model.MinOccupancy, model.MaxOccupancy).Select(item => new SelectListItem()
            { Text = item.ToString(), Value = item.ToString(),
                Selected = item == occupancy ? true: false,
            });
            model.UnavailablePeriodsModel = new List<DateTime>();
            foreach (var item in model.UnavailablePeriods)
            {
                for (DateTime i = item.From; i <= item.To; i=i.AddDays(1))
                {
                    model.UnavailablePeriodsModel.Add(i);
                }
            }

            if (occupancy != 0 && model.CheckIn != DateTime.MinValue && model.CheckOut != DateTime.MinValue)
            {
                if (model.StandardPricingModel != null)
                    model.Price = await _touristObject.GetPriceForStandardModel(occupancy, currency, model.CheckIn, model.CheckOut, model.StandardPricingModel);
                else if (model.OccupancyBasedPricing != null)
                    model.Price = await _touristObject.GetPriceForOccupancyBasedModel(occupancy, currency, model.CheckIn, model.CheckOut, model.OccupancyBasedPricing);
            }

            model.CurrPage = 1;
            model.Pager = new Pager(_touristObject.CountRatingsAndReviews(model.Id), 1);
            model.Reviews = _touristObject.GetRatingsAndReviews(1, model.Pager.PageSize, model.Id)
                .Select(rr => new Review() {
                    Id = rr.Id,
                    Rating = rr.Rating,
                    Text = rr.Review,
                    User = rr.User,
                }).ToList();

            return View(model);
        }

        [Authorize]
        public IActionResult AddReview(ObjectDetailsViewModel model)
        {
           // var user = await _userManager.GetUserAsync(User);
            var identUserId = _userManager.GetUserId(this.User);
            var currentUser = _user.GetUserFromIdentUser(identUserId);

            var ratingAndReview = new RatingsAndReviews()
            {
                Rating = model.Review.Rating,
                Review = model.Review.Text,
                User = currentUser,
                ObjectId = model.Id,
            };

            _touristObject.AddRatingAndReview(ratingAndReview);

            //this will be first page
            var ratingsAndReviews = _touristObject.GetRatingsAndReviews(1,5,model.Id);

            var ratingsAndReviwsModel = ratingsAndReviews.Select(rr => new Review()
            {
                Id = rr.Id,
                Text = rr.Review,
                Rating = rr.Rating,
                UserId = rr.UserId,
                User = rr.User
            }).ToList();

           // TempData["Notification"] = "Uspješno ste dodali utisak i ocjenu.";
            return Ok(ratingsAndReviwsModel);
        }

        public IActionResult ChangeReviewsPage(ObjectDetailsViewModel model)
        {
            model.Pager = new Pager(_touristObject.CountRatingsAndReviews(model.Id), model.CurrPage);
            if (model.CurrPage == 0)
                model.Pager.CurrentPage = 1; 

            model.Reviews = _touristObject.GetRatingsAndReviews(model.Pager.CurrentPage, model.Pager.PageSize, model.Id)
                .Select(rr => new Review()
                {
                    Id = rr.Id,
                    Rating = rr.Rating,
                    Text = rr.Review,
                    User = rr.User,
                }).ToList();

            return PartialView("_Reviews", model);
        }

        public async Task<IActionResult> FindPrice(ObjectDetailsViewModel model)
        {
            model.CheckIn = model.CheckInString == null ? DateTime.MinValue : DateTime.Parse(model.CheckInString);
            model.CheckOut = model.CheckOutString == null ? DateTime.MinValue : DateTime.Parse(model.CheckOutString);
            if (!(model.CheckIn != DateTime.MinValue && model.CheckIn != DateTime.MinValue && model.SelectedOccupancy > 0))
            {
                ViewData["Error-Notification"] = "Unesite ispravne podatke";
                //da li radi?
                return View(model);
            }
            var currency = Request.Cookies["Currency"] == null ? "BAM" : Request.Cookies["Currency"];
            if (model.OccupancyPricing == false)
            {
                model.StandardPricingModel = _touristObject.GetStandardPricingModelWithId((int)model.StandardPricingModelId);
                model.Price = await _touristObject.GetPriceForStandardModel(model.SelectedOccupancy, currency, model.CheckIn, model.CheckOut, model.StandardPricingModel);
            }
            else if (model.OccupancyPricing == true)
            {
                model.OccupancyBasedPricing = _touristObject.GetOccupancyBasedPricingWithId((int)model.OccupancyBasedPricingId);
                model.Price = await _touristObject.GetPriceForOccupancyBasedModel(model.SelectedOccupancy, currency, model.CheckIn, model.CheckOut, model.OccupancyBasedPricing);
            }

            return PartialView("_PricePartial", model);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult RemoveReview(int id)
        {
            _touristObject.DeleteReview(id);
            return Ok();
        }
        //još delete

    }
}