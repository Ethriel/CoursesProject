namespace ServicesAPI.Sorts
{
    public class Pagination
    {
        public string[] Position { get; set; }
        public int Current { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public int GetSkip()
        {
            return (this.Current * this.PageSize) - this.PageSize;
        }
        public int GetTake()
        {
            return this.PageSize == 1 ? this.PageSize : this.Current * this.PageSize;
        }
    }
}
