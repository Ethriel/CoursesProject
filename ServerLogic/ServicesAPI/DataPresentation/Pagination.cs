using System.Collections.Generic;

namespace ServicesAPI.DataPresentation
{
    public class Pagination
    {
        public IEnumerable<string> Position { get; set; }
        public int Current { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public Pagination()
        {
            Position = new string[] { "none", "bottomCenter" };
            Current = 1;
            PageSize = 5;
            Total = 5;
        }
        public Pagination(IEnumerable<string> position, int current, int pageSize, int total)
        {
            Position = position;
            Current = current;
            PageSize = pageSize;
            Total = total;
        }
        public int GetSkip()
        {
            return (this.Current * this.PageSize) - this.PageSize;
        }
        public int GetTake()
        {
            //return this.PageSize == 1 ? this.PageSize : this.Current * this.PageSize;
            return this.PageSize;
        }
    }
}
