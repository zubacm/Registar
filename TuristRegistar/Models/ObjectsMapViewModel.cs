using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class ObjectsMapViewModel
    {
        public List<ObjectItemModel> ObjectsList { get; set; }
        public Search Search { get; set; }

        public Double CenterLat { get; set; }
        public Double CenterLng { get; set; }
    }
}
