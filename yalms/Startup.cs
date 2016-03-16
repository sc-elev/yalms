using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(yalms.Startup))]
namespace yalms
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
