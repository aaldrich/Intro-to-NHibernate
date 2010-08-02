using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateDemo.MVC.Controllers;
using StructureMap;

namespace NHibernateDemo.MVC
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            //Register Dependencies with Structuremap
            ObjectFactory.Initialize(x=>x.AddRegistry<DependencyBootstrapper>());

            //Use StructureMapControllerFactory instead of ASP.Net MVC
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
        }
    }
}