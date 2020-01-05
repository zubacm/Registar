using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;



namespace TuristRegistar.Services
{
    public class UserService : IUser
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLegalPerson(Users user)
        {
            _context.Userss.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddNaturalPerson(Users user)
        {
            _context.Userss.Add(user);
            await _context.SaveChangesAsync();
        }

        public Users GetUser(string id)
        {
            return _context.Userss
                .Where(u => u.IdentUserId == id)
                .FirstOrDefault();
        }

        public void UpdateUser(Users user)
        {
            var myuser = _context.Userss.FirstOrDefault(u => u.Id == user.Id);
            if (myuser != null)
            {
                _context.Entry(myuser).CurrentValues.SetValues(user);
                _context.SaveChanges();
            }
        }

        public void ChangeUsername(string identUserId, string username)
        {
            _context.Users.FirstOrDefault(u => u.Id == identUserId).UserName = username;
            _context.Userss.FirstOrDefault(u => u.IdentUserId == identUserId).UserName = username;
            _context.SaveChanges();
        }

        public IEnumerable<Currencies> GetCurrencies()
        {
            return _context.Currencies;
        }
    }
}
