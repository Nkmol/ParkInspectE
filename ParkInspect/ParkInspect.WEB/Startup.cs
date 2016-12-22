using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ParkInspect.WEB.Startup))]
namespace ParkInspect.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
