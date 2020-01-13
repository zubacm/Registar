using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Services
{
    public class TouristObjectService : ITouristObject
    {
        private readonly ApplicationDbContext _context;

        public TouristObjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Countries> GetCountries()
        {
            return _context.Counries.ToList();
        }

        public IEnumerable<Cities> GetCitiesFromCountry(int countryId)
        {
            return _context.Counries.Include(c => c.Cities)
                .FirstOrDefault(c => c.Id == countryId).Cities.ToList();
        }

        public IEnumerable<ObjectTypes> GetObjectTypes()
        {
            return _context.ObjectTypes;
        }


        //parsing string ex. [1,2,9,3]
        public List<int> ParseStringToIDsList(string text)
        {
            string[] input = text.Trim('[', ']').Split(',');

            int i = 0;
            return input.Where(s => int.TryParse(s, out i)) 
                             .Select(s => i)
                             .ToList();
        }

        //parsing string ex [1:2,3:12,8:2]
        public List<KeyValuePair<int, int>> ParseStringToKeyValueList(string text) {
            var listKeyValue = new List<KeyValuePair<int, int>>();
            string[] input = text.Trim('[', ']').Split(',');

            foreach (var item in input)
            {
                var pair = item.ToString().Split(':');
                listKeyValue.Add(new KeyValuePair<int, int>(Convert.ToInt32(pair[0]), Convert.ToInt32(pair[1]) ));
            }
            return listKeyValue; 
        }

        public List<KeyValuePair<int, float>> ParseStringToKeyValue(string text)
        {
            var listKeyValue = new List<KeyValuePair<int, float>>();
            string[] input = text.Trim('[', ']').Split(',');

            foreach (var item in input)
            {
                var pair = item.ToString().Split(':');
                listKeyValue.Add(new KeyValuePair<int, float>(Convert.ToInt32(pair[0]), (float)(Convert.ToDecimal(pair[1]))));
            }
            return listKeyValue;
        }

        public List<KeyValuePair<DateTime, DateTime>> ParseDates(string text)
        {
            var listKeyValue = new List<KeyValuePair<DateTime, DateTime>>();
            string[] input = text.Trim('[', ']').Split(',');

            foreach (var item in input)
            {
                var pair = item.ToString().Split(':');
                listKeyValue.Add(new KeyValuePair<DateTime, DateTime>(DateTime.Parse(pair[0]), DateTime.Parse(pair[1])));
            }
            return listKeyValue;
        }


        public IEnumerable<ObjectAttributes> GetAllObjectAttributes()
        {
            return _context.ObjectAttributes.ToList();
        }

        public IEnumerable<ObjectAttributes> GetObjectAttributes(List<int> excludedAttributesId)
        {
            return _context.ObjectAttributes.Where(oa => !excludedAttributesId.Contains(oa.Id))
                .ToList();
        }

        public IEnumerable<CountableObjectAttributes> GetAllCountableObjectAttributes()
        {
            return _context.CountableObjectAttributes.ToList();
        }

        public IEnumerable<CountableObjectAttributes> GetCountableObjectAttributes(List<int> excludedAttributesId)
        {
            return _context.CountableObjectAttributes.Where(item => !excludedAttributesId.Contains(item.Id))
               .ToList();
        }


        public CountableObjectAttributes GetCountableObjectAttribute(int id)
        {
            return _context.CountableObjectAttributes.FirstOrDefault(coa => coa.Id == id);
        }

        public ObjectAttributes GetObjectAttribute(int id)
        {
            return _context.ObjectAttributes.FirstOrDefault(oa => oa.Id == id);
        }


        public async Task AddObject(Objects myobject, string currency)
        {

            myobject = await ExchangeCurrencyAsync(myobject, currency, "BAM");

            _context.Objects.Add(myobject);
            _context.SaveChanges();
        }

        public async Task<Objects> GetObject(int id, string currency)
        {
            var obj =  _context.Objects
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.SpecialOffers)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.ObjectImages)
                .Include(o => o.OccupancyBasedPricing)
                .Include(o => o.StandardPricingModel)
                .FirstOrDefault(o => o.Id == id);
            obj = await ExchangeCurrencyAsync(obj, "BAM", currency);

            return obj;

        }

        public void AddImage(ObjectImages image, int objectid)
        {
            _context.Objects.FirstOrDefault(o => o.Id == objectid).ObjectImages.Add(image);
            _context.SaveChanges();
        }

        public void DeleteImage(int id)
        {
            var img = new ObjectImages() { Id = id};
            _context.ObjectImages.Remove(img);
            _context.SaveChanges();
        }



        private async Task<Objects> ExchangeCurrencyAsync(Objects myobject, string from, string to)
        {
            var exchangerate = await GetExchangeRate(from, to);
            foreach (var item in myobject.SpecialOffers)
            {
                
                item.Price = (float)exchangerate * item.Price;
            }
            if (myobject.OccupancyPricing)
            {
                foreach (var item in myobject.OccupancyBasedPricing.Prices)
                {
                    item.PricePerNight = (float)exchangerate*item.PricePerNight;
                }
            }
            else
            {
                myobject.StandardPricingModel.StandardPricePerNight = (float)exchangerate * myobject.StandardPricingModel.StandardPricePerNight;
            }

            return myobject;
        }


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
