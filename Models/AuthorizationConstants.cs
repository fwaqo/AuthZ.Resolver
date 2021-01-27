namespace Authorization.Models
{
    public static class AuthorizationConstants
    {
        public const string ReadAccess = "read";
        public const string WriteAccess = "write";
        public const string ReadAccessDisplayName = "Lese_Berechtigung";
        public const string WriteAccessDisplayName = "Schreib_Berechtigung";
        public const string ReaderPolicyName = "Reader_Policy";
        public const string WriterPolicyName = "Writer_Policy";

        public class PrincipalAuthorizationResponse
        {
            public const string Permissions = "permissions";
            public const string Roles = "roles";

        }

        public class Claims
        {
            public const string Permission = "permission";
            public const string Role = "role";
        }

        public class General
        {
            public const string Authorization = "Authorization";
        }
    }
}