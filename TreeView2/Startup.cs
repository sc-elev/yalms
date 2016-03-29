using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TreeView2.Startup))]
namespace TreeView2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
