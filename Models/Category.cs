namespace Authorization.Models
{
    public class Category
    {
        public Category(string key, string value)
        {
            Key = key.ToLower();
            DisplayName = value;
        }

        public string Key { get; set; }
        public string DisplayName { get; set; }
    }
}