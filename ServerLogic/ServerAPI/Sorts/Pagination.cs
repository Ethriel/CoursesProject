using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Sorts
{
    public class Pagination
    {
        public string[] Position { get; set; }
        public int Current { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
