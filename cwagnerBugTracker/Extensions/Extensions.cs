using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace cwagnerBugTracker.Extensions
{
    public static class Extensions
    {
        public static string GetFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");
            return claim?.Value ?? string.Empty;
        }

        public static string ProfilePicture(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ProfilePic");
            return claim?.Value ?? string.Empty;
        }
    }
}