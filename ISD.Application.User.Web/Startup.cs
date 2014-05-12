using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISD.User.Web.Startup))]
namespace ISD.User.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
