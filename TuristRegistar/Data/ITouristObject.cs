using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Data
{
    public interface ITouristObject
    {
        IEnumerable<Countries> GetCountries();
        IEnumerable<Cities> GetCitiesFromCountry(int countryId);
        IEnumerable<ObjectTypes> GetObjectTypes();
        List<int> ParseStringToIDsList(string text);
        List<KeyValuePair<int, int>> ParseStringToKeyValueList(string text);
        List<KeyValuePair<int, float>> ParseStringToKeyValue(string text);
        List<KeyValuePair<DateTime, DateTime>> ParseDates(string text);
        IEnumerable<ObjectAttributes> GetAllObjectAttributes();
        IEnumerable<ObjectAttributes> GetObjectAttributes(List<int> excludedAttributesId);
        IEnumerable<CountableObjectAttributes> GetAllCountableObjectAttributes();
        IEnumerable<CountableObjectAttributes> GetCountableObjectAttributes(List<int> excludedAttributesId);
        CountableObjectAttributes GetCountableObjectAttribute(int id);
        ObjectAttributes GetObjectAttribute(int id);
        Task AddObject(Objects myobject, string currency);
        Task<Objects> GetObject(int id, string currency);
        void AddImage(ObjectImages image, int objectid);
        void DeleteImage(int id);
    }
}
