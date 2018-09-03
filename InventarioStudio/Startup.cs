using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InventarioStudio.Startup))]
namespace InventarioStudio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
