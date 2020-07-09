using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Sorts
{
    public class Sorting
    {
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public Pagination Pagination { get; set; }
    }
}
