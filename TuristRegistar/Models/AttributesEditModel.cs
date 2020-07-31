using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class AttributesEditModel
    {
        public IEnumerable<ObjectAttributes> AvailableAttributes { get; set; }
        public String NewAttribute { get; set; }

        public int EditAttributeId { get; set; }
        public String EditAttributeName { get; set; }

        public String SubmitButton { get; set; }
    }
}
