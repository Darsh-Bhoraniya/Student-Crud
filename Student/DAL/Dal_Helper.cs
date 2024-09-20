namespace Student.DAL
{
    public class Dal_Helper
    {

        public static string ConncetionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("ConnectionString");

    }
}
