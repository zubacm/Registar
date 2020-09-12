using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

        public Users GetUserFromIdentUser(string identUserId)
        {
            return _context.Userss
                .Where(u => u.IdentUserId == identUserId)
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

        public IEnumerable<Objects> GetUserObjects(String identUserId, int pagenumber, int pagesize)
        {
            return _context.Objects
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Where(o => o.IdentUserId == identUserId)
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
        }
        public int CountUserObjects(String identUserId)
        {
            return _context.Objects
                   .Where(o => o.IdentUserId == identUserId).Count();

        }

        public IEnumerable<int> GetAllUserBookmarksId(String identUserId)
        {
            return _context.Bookmark
                .Where(b => b.UserId == identUserId)
                .Select(b => b.ObjectId)
                .ToList();

        }



        public IEnumerable<Objects> GetUserBookmarks(String identUserId, int pagenumber, int pagesize)
        {
            //Check this shit
            var bookmarksObjectsId = _context.Bookmark
                .Where(b => b.UserId == identUserId)
                .Select(b => b.ObjectId)
                .ToList();

            return _context.Objects
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Where(o => bookmarksObjectsId.Contains(o.Id))
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
                
        }

        public int CountUserBookmarks(String identUserId)
        {
            var bookmarksObjectsId = _context.Bookmark
                .Where(b => b.UserId == identUserId)
                .Select(b => b.ObjectId)
                .ToList();

            return _context.Objects
                .Where(o => bookmarksObjectsId.Contains(o.Id))
                .Count();

        }

        public void AddBookmark(String identUserId, int objectId)
        {
            var bookmark = new Bookmark() {
            ObjectId = objectId,
            UserId = identUserId,
            };

            _context.Bookmark.Add(bookmark);
            _context.SaveChanges();
        }

        public void RemoveBookmark(String identUserId, int objectId)
        {
            var bookmark = new Bookmark()
            {
                ObjectId = objectId,
                UserId = identUserId,
            };

            _context.Bookmark.Attach(bookmark);
            _context.Bookmark.Remove(bookmark);
            _context.SaveChanges();
        }


        public async Task<int> AddInitialConversationAsync(Conversations initialConversation)
        {
           await _context.Conversations.AddAsync(initialConversation);            
            _context.SaveChanges();
            return  initialConversation.Id;
        }

        public void AddMessageAsync(Messages message)
        {
            _context.Messages.Add(message);
            _context.SaveChangesAsync();
        }

        public Conversations GetConversationBetweenUsers(String IdentUser1Id, String IdentUser2Id)
        {
            return _context.Conversations
                .FirstOrDefault(c => (c.IdentUser1Id == IdentUser1Id && c.IdentUser2Id == IdentUser2Id) || (c.IdentUser1Id == IdentUser2Id && c.IdentUser2Id == IdentUser1Id));
        }
        public Conversations GetConversationBetweenUsers(int conversaionId)
        {
            return _context.Conversations
                .FirstOrDefault(c => c.Id == conversaionId);
        }

        public IEnumerable<Messages> GetConversationMessages(int conversationId, int pagenumber, int pagesize)
        {
            var query = _context.Messages.Where(m => m.ConversationId == conversationId).OrderByDescending(m => m.Id);
            var total = query.Count();
            if (total == 0)
                return new List<Messages>();
            var skip = total - (pagenumber * pagesize) < 0 && total - (pagenumber * pagesize) > 0 - pagesize ? 0 : total - (pagenumber * pagesize);
           return  _context.Messages.Where(m => m.ConversationId == conversationId)
                                .Skip(skip).Take(pagesize).ToList();
        }

        public void SetUnreadConversation(String receiverIdentUserId, int conversationId, DateTime lastInteraction)
        {
            var conversation = _context.Conversations.FirstOrDefault(c => c.Id == conversationId);
            conversation.Unread = true;
            conversation.UnredIdentUserId = receiverIdentUserId;
            conversation.LastIneractionDateTime = lastInteraction;
            _context.Conversations.Attach(conversation);

            _context.SaveChanges();
        }

        public IEnumerable<Conversations> GetConversations(String identUserId, int pagenumber, int pagesize)
        {
            return _context.Conversations.Where(c => c.IdentUser1Id == identUserId || c.IdentUser2Id == identUserId)
                .OrderByDescending(c => c.LastIneractionDateTime)
                .Include(c => c.IdentUser1)
                .Include(c => c.IdentUser2)
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
        }
        public IEnumerable<Conversations> SearchForConversations(String search, String identUserId, int pagenumber, int pagesize)
        {
            return _context.Conversations.Where(c => c.IdentUser1Id == identUserId || c.IdentUser2Id == identUserId)
                .Where(c => c.IdentUser1Id != identUserId ? c.IdentUser1.UserName.ToLower().Contains(search.ToLower()) : c.IdentUser2.UserName.ToLower().Contains(search.ToLower()) )
               .OrderByDescending(c => c.LastIneractionDateTime)
               .Include(c => c.IdentUser1)
               .Include(c => c.IdentUser2)
               .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
        }


        public Messages GetLastMessage(int conversationid)
        {
            return _context.Messages.Where(m => m.ConversationId == conversationid)
                .OrderByDescending(m => m.DateTime)
                .FirstOrDefault();
        }

        public bool CheckForUnreadMessages(String identUserId)
        {
            return _context.Conversations.Where(c => c.Unread && c.UnredIdentUserId == identUserId).Count() > 0 ?
                 true : false;
        }

        public void SetConversationRead(int conversationId)
        {
            var conversation = _context.Conversations.FirstOrDefault(c => c.Id == conversationId);
            conversation.Unread = false;
            _context.Conversations.Attach(conversation);

            _context.SaveChanges();
        }

    }
}
