namespace ServicesAPI.DataPresentation
{
    public class Search
    {
        public string SearchCriteria { get; set; }
        public Pagination Pagination { get; set; }
        public Search()
        {

        }
        public Search(string searchCriteria, Pagination pagination)
        {
            SearchCriteria = searchCriteria;
            Pagination = pagination;
        }
    }
}
