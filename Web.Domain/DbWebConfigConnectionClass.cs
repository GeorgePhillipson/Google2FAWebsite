using System.Configuration;

namespace Web.Domain
{
    public class DbWebConfigConnectionClass
    {
        public static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
    }
}
