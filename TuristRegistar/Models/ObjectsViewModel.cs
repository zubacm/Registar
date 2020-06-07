using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class ObjectsViewModel
    {
        public List<ObjectItemModel> ObjectsList { get; set; }
        public Pager Pager { get; set; }
        public Search Search { get; set; }

        public int CurrPage { get; set; }

        public String IdentUserId { get; set; }
  
        //[Display(Name = "Tip objekta")]
        //public List<ObjectTypeModel> ObjectTypes { get; set; }
    }
}
