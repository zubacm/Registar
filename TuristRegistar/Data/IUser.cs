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
        Users GetUserFromIdentUser(string identUserId);
        void UpdateUser(Users user);
        void ChangeUsername(string identUserId, string username);
        IEnumerable<Currencies> GetCurrencies();
        IEnumerable<Objects> GetUserObjects(String identUserId, int pagenumber, int pagesize);
        IEnumerable<int> GetAllUserBookmarksId(String identUserId);
        IEnumerable<Objects> GetUserBookmarks(String identUserId, int pagenumber, int pagesize);
        int CountUserObjects(String identUserId);
        int CountUserBookmarks(String identUserId);
        void AddBookmark(String identUserId, int objectId);
        void RemoveBookmark(String identUserId, int objectId);

        Task<int> AddInitialConversationAsync(Conversations initialConversation);
        void AddMessageAsync(Messages message);
        Conversations GetConversationBetweenUsers(String IdentUser1Id, String IdentUser2Id);
        IEnumerable<Messages> GetConversationMessages(int conversationId, int pagenumber, int pagesize);
    }
}
