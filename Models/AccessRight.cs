namespace Authorization.Models
{
    public class AccessRight
    {

        public AccessRight(string key, string value, Category category, string description)
        {
            Key = key.ToLower();
            DisplayName = value;
            Category = category;
            Description = description;
        }

        public string Description { get; set; }

        public string Key { get; set; }
        public string DisplayName { get; set; }
        public Category Category { get; }
    }
}