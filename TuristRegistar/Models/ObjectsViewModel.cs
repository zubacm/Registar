using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class ObjectsViewModel
    {
        public List<ObjectItemModel> ObjectsList { get; set; }
        public Search Search { get; set; }
        public Pager Pager { get; set; }
        public int CurrPage { get; set; }

    }
}
