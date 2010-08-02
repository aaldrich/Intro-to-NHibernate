using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernateDemo.DataAccess.NHibernate_Setup;

namespace NHibernateDemo.MVC.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        readonly INHibernateHelper nhibernate_helper;

        public HomeController(INHibernateHelper nhibernate_helper)
        {
            this.nhibernate_helper = nhibernate_helper;
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult CreateDatabase()
        {
            nhibernate_helper.create_db();
            return View("Index");
        }
    }
}