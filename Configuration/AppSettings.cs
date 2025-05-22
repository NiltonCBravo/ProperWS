using System.Configuration;
namespace proper_ws.Configuration
{
    public static class AppSettings
    {
        public static string Environment => ConfigurationManager.AppSettings["Environment"];
    }
}
