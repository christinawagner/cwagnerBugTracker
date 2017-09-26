using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cwagnerBugTracker.Startup))]
namespace cwagnerBugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
