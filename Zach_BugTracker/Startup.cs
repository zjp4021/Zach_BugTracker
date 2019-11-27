using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zach_BugTracker.Startup))]
namespace Zach_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
