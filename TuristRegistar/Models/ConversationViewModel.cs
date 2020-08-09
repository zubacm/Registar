using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class ConversationViewModel
    {

        [Required]
        public String Text { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

        public String SenderId { get; set; }
        public String SenderUsername { get; set; }
        public String ReceiverId { get; set; }
        public String ReceiverUsername { get; set; }

        public int? ConversationId { get; set; }
        public IEnumerable<Messages> Messages { get; set; }
    }
}
