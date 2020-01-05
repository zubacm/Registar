using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Data
{
    public interface IUser
    {
        Task AddLegalPerson(Users user);
        Task AddNaturalPerson(Users user);
        Users GetUser(string id);
        void UpdateUser(Users user);
        void ChangeUsername(string identUserId, string username);
        IEnumerable<Currencies> GetCurrencies();
    }
}
