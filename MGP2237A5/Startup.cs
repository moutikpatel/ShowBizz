using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MGP2237A5.Startup))]

namespace MGP2237A5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
