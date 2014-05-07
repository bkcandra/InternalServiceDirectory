using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISD.Administration.Web.Startup))]
namespace ISD.Administration.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
