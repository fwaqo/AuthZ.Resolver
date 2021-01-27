namespace Authorization.Options
{
    public class AuthorizationProviderOptions
    {
        public string Address { get; set; }
        public CacheOptions Cache { get; set; } = new CacheOptions();
    }
}