using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Patron.Startup))]
namespace Patron
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
