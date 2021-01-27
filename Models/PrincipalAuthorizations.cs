using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Authorization.Models
{
    public class PrincipalAuthorizations
    {
        [JsonPropertyName("roles")]
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        [JsonPropertyName("permissions")]
        public IEnumerable<string> Permissions { get; set; } = new List<string>();
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; } = 3600;
    }
}