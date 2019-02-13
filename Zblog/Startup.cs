using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zblog.Startup))]
namespace Zblog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
