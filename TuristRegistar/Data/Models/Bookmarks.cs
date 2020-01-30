using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class Bookmarks
    {
        [Key]
        public int ObjectId { get; set; }
        public Objects Object { get; set; }
        [Key]
        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
