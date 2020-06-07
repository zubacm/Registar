using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class UserBookmarksModel
    {
        public String IdentUserId { get; set; }

        public List<ObjectItemModel> Bookmarks { get; set; }


        public Pager Pager { get; set; }
        public int CurrPage { get; set; }

    }
}
