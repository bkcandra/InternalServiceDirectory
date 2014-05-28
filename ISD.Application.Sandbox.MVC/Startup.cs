using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISD.Application.Sandbox.MVC.Startup))]
namespace ISD.Application.Sandbox.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
