using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISD.Application.provider.MVC.Startup))]
namespace ISD.Application.provider.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
