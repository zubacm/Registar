using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class InboxConversationsModel
    {
        public int ConversationId { get; set; }
        public String WithIdentUserId { get; set; }
        public String WithUsername { get; set; }
        public Messages LastMassage { get; set; }
        public bool Unread { get; set; }

        public bool SearchActive { get; set; }
    }
}
