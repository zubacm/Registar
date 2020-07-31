using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class TypesEditModel
    {
        public IEnumerable<ObjectTypes> AvailableTypes { get; set; }
        public String NewType { get; set; }

        public int EditTypeId { get; set; }
        public String EditTypeName { get; set; }

        public String SubmitButton { get; set; }
    }
}
