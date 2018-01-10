using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureADWebApp.Startup))]
namespace AzureADWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
