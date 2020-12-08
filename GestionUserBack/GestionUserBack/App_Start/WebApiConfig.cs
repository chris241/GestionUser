using GestionUserBack.Business.Repository;
using GestionUserBack.Entity;
using GestionUserBack.Utilty.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;

namespace GestionUserBack
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuration et services API Web
            UnityContainer unityContainer = new UnityContainer();
            SetDependencies(unityContainer);
            // Itinéraires de l'API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
        private static void SetDependencies(UnityContainer container)
        {

            container.RegisterType<EntityRepository<User>, UserRepository>();
        }
    }
}

