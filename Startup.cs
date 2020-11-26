using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SRSProgramMVC.Startup))]
namespace SRSProgramMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
