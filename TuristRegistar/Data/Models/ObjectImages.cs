using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class ObjectImages
    {
        public int Id { get; set; }
        public String Path { get; set; }
        public bool IsCover { get; set; }

        public Objects Objects { get; set; }
    }
}
