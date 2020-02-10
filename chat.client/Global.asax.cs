using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace chat.client
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup();
            startup.ConfigureServices(services);

            //IServiceProvider serviceProvider = services.BuildServiceProvider();

            var provider = new ServiceProvider(services.BuildServiceProvider());
            HttpRuntime.WebObjectActivator = provider;



            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);           
        }
    }
}