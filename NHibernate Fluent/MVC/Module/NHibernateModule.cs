using System;
using System.Web;
using NHibernate;
using NHibernateDemo.DataAccess.NHibernate_Setup;
using StructureMap;

namespace NHibernateDemo.MVC.Module
{
    public class NHibernateModule : IHttpModule 
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += begin_request;
            context.EndRequest += end_request;
        }

        void end_request(object sender, EventArgs e)
        {
            Dispose(); 
        }

        void begin_request(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<INHibernateHelper>().open_session();
        }

        public void Dispose()
        {
            ObjectFactory.GetInstance<INHibernateHelper>().dispose_current_session();
        }
    }
}