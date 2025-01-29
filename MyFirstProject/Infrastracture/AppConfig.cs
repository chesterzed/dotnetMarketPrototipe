namespace MyFirstProject.Infrastracture
{
    public class AppConfig
    {
        public TineMCE TineMCE { get; set; } = new TineMCE();
        public Company Company { get; set; } = new Company();
        public Database Database { get; set; } = new Database();
    }

    public class Database
    {
        public string? ConnectionString { get; set; }
    }

    public class TineMCE
    {
        public string? APIKey { get; set; }
    }

    public class Company
    {
        public string? CompanyName { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyPhoneShort { get; set; }
        public string? CompanyEmail { get; set; }
    }
}
