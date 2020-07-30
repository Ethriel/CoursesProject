using System.Collections.Generic;

namespace ServicesAPI.DataPresentation
{
    public class Pagination
    {
        public IEnumerable<string> Position { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public Pagination()
        {
            SetDefaults(5);
        }
        public Pagination(int total)
        {
            SetDefaults(total);
            Total = total;
        }
        public Pagination(IEnumerable<string> position, int page, int pageSize, int total)
        {
            Position = position;
            Page = page;
            PageSize = pageSize;
            Total = total;
        }
        public void SetDefaults(int total, int page = 1, int pageSize = 2)
        {
            Position = new string[] { "none", "bottomCenter" };
            Page = page;
            PageSize = pageSize;
            Total = total;
        }
        public int GetSkip()
        {
            return (this.Page * this.PageSize) - this.PageSize;
        }
        public int GetTake()
        {
            //return this.PageSize == 1 ? this.PageSize : this.Current * this.PageSize;
            return this.PageSize;
        }
    }
}
