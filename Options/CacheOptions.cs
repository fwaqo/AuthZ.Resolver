namespace Authorization.Options
{
    public class CacheOptions
    {
        public bool Enabled { get; set; } = true;
        public int ExpiresIn { get; set; } = 1800;
    }
}