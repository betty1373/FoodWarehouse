namespace FW.Web.Configurations.Options
{
    public class AuthenticationOptions
    {
        public const string KeyValue = "Authentication";

        public string ApiName { get; set; }
        public string Authority { get; set; }
        public string ClientId { get; set; }
    }
}