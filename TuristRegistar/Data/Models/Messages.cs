using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public Conversations Conversation { get; set; }
        public DateTime DateTime { get; set; }
        public String Message { get; set; }
        public String SendingIdentUserId { get; set; }
        public IdentityUser SendingIdentUser { get; set; }
    }
}
