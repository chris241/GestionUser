using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

[assembly: OwinStartup(typeof(GestionUserBack.App_Start.Startup))]
namespace GestionUserBack.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            UnityContainer unityContainer = new UnityContainer();
           
        }
    }
}