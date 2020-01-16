using System;
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


        public ObjectController(ITouristObject touristObject, IHostingEnvironment hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _touristObject = touristObject;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

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
                ObejectTypeId = Convert.ToInt32(model.SelectedObjectType) == 0 ? null : (int?)Convert.ToInt32(model.SelectedObjectType),
                Lat = model.Lat,
                Lng = model.Lng,
                UnavailablePeriods = model.UnavailablePeriods == null ? new List<UnavailablePeriods>() : model.UnavailablePeriods,
                OccupancyPricing = model.OccupancyPricing,
                StandardPricingModel = (!model.OccupancyPricing) ? model.StandardPricingModel : null,
                OccupancyBasedPricing = model.OccupancyPricing ? model.OccupancyBasedPricing : null,
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
        [Route("editobject")] // /createobject
        public async Task<IActionResult> EditObject(int id)
        {
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
                UnavailablePeriods = myobject.UnavailablePeriods.ToList(),
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
                SelectedObjectType = myobject.ObejectTypeId == null ? null : myobject.ObejectTypeId.ToString(),
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

        public IActionResult DeletePeriod(EditObjectViewModel model)
        {
            _touristObject.DeletePeriod(model.DeletePeriodId);

            return Ok();
        }

        public IActionResult AddPeriod(EditObjectViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.UnavailablePeriodsString))
                model.UnavailablePeriods = (_touristObject.ParseDates(model.UnavailablePeriodsString))
                    .Select(item => new UnavailablePeriods() { From = item.Key, To = item.Value }).ToList();
            var id = _touristObject.AddPeriod(model.UnavailablePeriods[0], model.Id);

            return Json(id);
        }


        [HttpPost]
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
            if (model.OccupancyBasedPricing.Id != 0)
            {
                _touristObject.NewOccupancyBasedPricing(model.OccupancyBasedPricing, model.Id);
            }
            else
            {
                _touristObject.DeleteStandardModel(model.Id);
                _touristObject.AddOccupancyBasedModel(model.OccupancyBasedPricing, model.Id);
                //add occupancy
            }
            ViewData["Notification"] = "Uspješno sačuvane izmjene";
            return null;
            //return RedirectToAction("Index", "Object");
        }

        public IActionResult EditStandardModel(EditObjectViewModel model)
        {
            model.OccupancyPricing = false;

            if (!CheckPricing(model))
            {
                TempData["Error-Notification"] = "Ispravno popunite neophodna polja!";
                //redirect to edit umjesto ovog
                return View(model);
            }

            if (model.StandardPricingModel.Id == 0)
            {
                _touristObject.DeleteOccupancyBasedModel(model.Id);
                _touristObject.AddStandardModel(model.StandardPricingModel, model.Id);
            }
            else
            {
                _touristObject.EditStandardModel(model.StandardPricingModel);
            }

            //ove nalove
            return null;
        }

        public IActionResult DeleteObjectHasAttribute(EditObjectViewModel model)
        {
            _touristObject.DeleteObjectHasAttributes(model.Id, model.DeleteAttributeId);
            return Ok();
        }

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
        public IActionResult DeleteCntAttribute(EditObjectViewModel model)
        {
            _touristObject.DeleteCntAttributeCount(model.Id, model.DeleteCntAttributeId);
            return Ok();
        }

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
        public IActionResult DeleteSpecialOffer(EditObjectViewModel model)
        {
            _touristObject.DeleteSpecialOffer(model.Id, model.DeleteSpecialOfferId);
            return Ok();
        }

    }
}