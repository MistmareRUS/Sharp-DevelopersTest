using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServerApp.Startup))]
namespace ServerApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
