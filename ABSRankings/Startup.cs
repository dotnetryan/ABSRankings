using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ABSRankings.Startup))]
namespace ABSRankings
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
