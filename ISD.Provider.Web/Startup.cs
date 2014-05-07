using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISD.Provider.Web.Startup))]
namespace ISD.Provider.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
