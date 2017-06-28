using System;

namespace Web.Security.CustomIdentity
{
    /// <summary>
    /// Create security stamp
    /// </summary>
    public static class SecurityStamp
    {
        public static string EncryptSecurityStamp(string purpose,string id)
        {
            var concentrateString = $"{purpose}{DateTimeOffset.UtcNow}{id}";

            return concentrateString;
        }

        public static string DecryptSecurityStamp()
        {
            //TODO in future blog
            return null;
        }
    }
}
