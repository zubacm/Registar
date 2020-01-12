using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public void AddObject(Objects myobject)
        {
            _context.Add(myobject);
        }
    }
}
