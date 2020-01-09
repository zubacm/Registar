using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
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


        public ObjectController(ITouristObject touristObject, IHostingEnvironment hostingEnvironment)
        {
            _touristObject = touristObject;
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Route("createobject")] // /createobject
        public IActionResult CreateObject()

        {
            var model = new CreateObjectViewModel();
            IEnumerable<Countries> countries = _touristObject.GetCountries();
            model.Countries = countries.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();

            IEnumerable<ObjectAttributes> attributes = _touristObject.GetAllObjectAttributes();
            model.Offers = attributes.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            model.SpecialOffers = model.Offers;

            IEnumerable<CountableObjectAttributes> cntOffers = _touristObject.GetAllCountableObjectAttributes();
            model.CountableOffers = cntOffers.Select(co => new SelectListItem() { Text = co.Name, Value = co.Id.ToString() }).ToList();

            return View(model);
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

        [HttpPost]
        [Route("createobject")] // /createobject
        public IActionResult CreateObject(CreateObjectViewModel model)
        {
            if ((!ModelState.IsValid) || (!CheckPricing(model)))
            {
                TempData["Error-Notification"] = "Ispravno popunite neophodna polja!";
                return View(model);
            }

            if(!string.IsNullOrWhiteSpace(model.AddedOffers))
                model.ListOfAddedOffersId = _touristObject.ParseStringToIDsList(model.AddedOffers);
            if (!string.IsNullOrWhiteSpace(model.AddedCountableOffers))
                model.ListOfAddedCntOffers = (_touristObject.ParseStringToKeyValueList(model.AddedCountableOffers))
                    .Select(item => new CountableOffers() { Id = item.Key, Value = item.Value }).ToList();
            if (!string.IsNullOrWhiteSpace(model.AddedSpecialOffers))
                model.ListOfAddedSpecialOffersId = (_touristObject.ParseStringToKeyValue(model.AddedSpecialOffers))
                    .Select(item => new SpecialOffers() { Id = item.Key, Price = item.Value }).ToList();
            if ((!model.StandardPricing) && (!string.IsNullOrWhiteSpace(model.OccubancBasedPrices)))
                model.OccupancyBasedPricing.Prices = (_touristObject.ParseStringToKeyValue(model.OccubancBasedPrices))
                    .Select(item => new OccupancyBasedPrices() { Occupancy = item.Key, PricePerNight = item.Value }).ToList();
            //check if this up and down works

            //if(!string.IsNullOrWhiteSpace(model.UnavailablePeriodsString))
            //    model.UnavailablePeriods = (_touristObject)

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
            if (model.StandardPricing &&
                (model.StandardPricingModel.StandardOccupancy == null || model.StandardPricingModel.OffsetPercentage == null
                || model.StandardPricingModel.StandardOccupancy == null || model.StandardPricingModel.MinOccupancy == null || model.StandardPricingModel.MaxOccupancy == null))
            {
                return false;
            }
            if ((!model.StandardPricing) && (model.OccupancyBasedPricing.MaxOccupancy == null || model.OccupancyBasedPricing.MinOccupancy == null || model.OccubancBasedPrices == null))
            {
                return false;
            }
            return true;
        }
    }
}