using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WhatchaWatchin.Startup))]
namespace WhatchaWatchin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
