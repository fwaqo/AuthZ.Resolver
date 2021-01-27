using System;
using Authorization.Models;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class DefineAccessRightAttribute : Attribute
    {
        public DefineAccessRightAttribute(string categoryKey, string categoryDisplayName, string key, string value, string description = null)
        {
            CategoryKey = string.IsNullOrEmpty(categoryKey)
                ? throw new ArgumentNullException(nameof(categoryKey))
                : categoryKey.Replace(nameof(Controller),  string.Empty);;
            CategoryDisplayName = categoryDisplayName ?? throw new ArgumentNullException(nameof(categoryDisplayName));
            Key = key ?? throw new ArgumentNullException(nameof(key));;
            DisplayName = value ?? throw new ArgumentNullException(nameof(value));
            Description = description;
        }

        public string Description { get; set; }

        public string CategoryKey { get; }
        public string CategoryDisplayName { get; }
        public string Key { get; }
        public string DisplayName { get; }

        public AccessRight GetAccessRight(string key, string value)
        {
            return new AccessRight(key, value, new Category(CategoryKey, CategoryDisplayName), Description);
        }
    }
}