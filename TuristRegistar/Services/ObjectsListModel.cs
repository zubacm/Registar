using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Services
{
    public class ObjectsListModel
    {
        public IEnumerable<Objects> Objects { get; set; }
        public int ObjectsCount { get; set; }
    }
}
