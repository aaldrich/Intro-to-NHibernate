using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateDemo.Entities.Employees;

namespace NHibernateDemo.DataAccess.NHibernate_Setup
{
    public class NHibernateHelper: INHibernateHelper, IDisposable
    {
        static ISessionFactory session_factory;
        Configuration config;
        ISession current_session;

        public ISessionFactory SessionFactory
        {
            get { return session_factory; }
            set { session_factory = value; }
        }

        public NHibernateHelper()
        {
            initialize();
            initialize_nhibernate_profiler();
        }

        private static void initialize_nhibernate_profiler()
        {
            HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize();
        }

        public void initialize()
        {
            session_factory =
                Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile("nhdemo_sqllite_db.db"))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Employee>().ExportTo(@"Mappings_Export"))
                .ExposeConfiguration(x => config = x)
                .BuildSessionFactory();

            session_factory = config.BuildSessionFactory();
        }

        public ISession open_session()
        {
            current_session = session_factory.OpenSession();
            return current_session;
        }

        public void dispose_current_session()
        {
            current_session.Dispose();
        }

        public ISession get_current_session()
        {
            return current_session;
        }

        public void create_db()
        {
            var connection = session_factory.OpenSession().Connection;

            new SchemaExport(config).Execute(false, true, false, connection, null);
        }

        public void Dispose()
        {
            session_factory.Dispose();
            session_factory = null;
        } 
    }
}