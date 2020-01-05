using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class CntObjAttributesCount
    {
        [Key]
        public int CountableObjAttrId { get; set; }
        public CountableObjectAttributes CountableObjAttr { get; set; }
        [Key]
        public int ObjectId { get; set; }
        public Objects Object { get; set; }

        public int Count { get; set; }
    }
}
