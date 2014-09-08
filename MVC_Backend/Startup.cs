using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Backend.Startup))]
namespace MVC_Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
