namespace ServicesAPI.DataPresentation
{
    public class SearchAndSort
    {
        public string SearchField { get; set; }
        public Sort Sort { get; set; }
        public Pagination Pagination { get; set; }
        public bool IsSearch { get; set; }
    }
}
