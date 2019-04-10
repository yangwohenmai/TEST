using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTest.Startup))]
namespace WebTest
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
