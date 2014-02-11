using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SnakeBattleNet.Web.Startup))]

namespace SnakeBattleNet.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
