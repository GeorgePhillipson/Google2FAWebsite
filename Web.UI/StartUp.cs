using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Web.UI;

[assembly: OwinStartup(typeof(Startup))]

namespace Web.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "TestSite",
                LoginPath = new PathString("/")
            });
        }
    }
}