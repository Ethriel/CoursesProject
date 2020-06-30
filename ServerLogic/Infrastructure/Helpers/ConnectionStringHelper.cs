namespace Infrastructure.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string Connection
        {
            get => @"Data Source=DELL\SQLEXPRESS;Initial Catalog=CoursesSystemDb;Integrated Security=True;Trusted_Connection=True";
        }
    }
}
