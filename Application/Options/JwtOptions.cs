namespace Application.Options
{
    public class JwtOptions
    {
        public static string SectionName => "JwtOptions";

        public  bool ValidateIssuer { get; set; }
        public  string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public string Key { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public int TimeToLive { get; set; }
    }
}
