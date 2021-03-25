namespace ImpactApi.Models
{
    public class ImpactDatabaseSettings : IImpactDatabaseSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }
    }

    public interface IImpactDatabaseSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }
    }
}
