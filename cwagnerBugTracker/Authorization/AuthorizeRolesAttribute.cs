using System.Web.Mvc;

namespace cwagnerBugTracker
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = roles.Length > 0 ? string.Join(",", roles) + $",{Domain.Roles.Super}" : string.Empty;
        }
    }
}
