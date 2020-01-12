using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult CreateObject(CreateObjectViewModel model)
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
                CountryId = Convert.ToInt32(model.SelectedCountry),
                CityId = Convert.ToInt32(model.SelectedCity),
                ObejectTypeId = Convert.ToInt32(model.SelectedObjectType),
                Lat = model.Lat,
                Lng = model.Lng,
                UnavailablePeriods = model.UnavailablePeriods == null ? new List<UnavailablePeriods>() : model.UnavailablePeriods,
                OccupancyPricing = model.OccupancyPricing,
                StandardPricingModel = (!model.OccupancyPricing) ? model.StandardPricingModel : null,
                OccupancyBasedPricing = model.OccupancyPricing ? model.OccupancyBasedPricing : null,
                ////CreatorId = null,
                IdentUserId = _userManager.GetUserId(this.User),
                ObjectImages = CopyFiles(Path.Combine(_hostingEnvironment.WebRootPath, "Temp"), Path.Combine(_hostingEnvironment.WebRootPath, _userManager.GetUserId(this.User))),
            };
            if (model.Surface != null)
                newobject.Surface = (float)model.Surface;

            return Redirect("createobject");
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

                //string webRootPath = _hostingEnvironment.WebRootPath;
                //string contentRootPath = _hostingEnvironment.ContentRootPath;

                //Content(webRootPath + "\n" + contentRootPath);
                var path  = Path.Combine(_hostingEnvironment.WebRootPath, "Temp" , file.FileName);

                //file.SaveAs(path);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return Json(file.FileName);

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
            string[] files = System.IO.Directory.GetFiles(sourcePath);

            foreach (string file in files)
            {
                System.IO.File.Copy(sourcePath, destinationPath);
                var img = new ObjectImages()
                {
                    //try it!
                    Path = Path.Combine(destinationPath, file)
                };
                imgs.Add(img);
            }

            return imgs;
        }


        public void DeleteAllTempImages()
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Temp");
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

    }
}