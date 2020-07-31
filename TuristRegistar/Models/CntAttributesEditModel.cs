using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class CntAttributesEditModel
    {
        public IEnumerable<CountableObjectAttributes> AvailableCntAttributes { get; set; }
        public String NewCntAttribute { get; set; }

        public int EditCntAttributeId { get; set; }
        public String EditCntAttributeName { get; set; }

        public String SubmitButton { get; set; }
    }
}
