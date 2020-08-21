using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class InboxViewModel
    {
        public String IdentUserId { get; set; }
        public String Username { get; set; }
        public IEnumerable<InboxConversationsModel> Conversations { get; set; }

    }
}
