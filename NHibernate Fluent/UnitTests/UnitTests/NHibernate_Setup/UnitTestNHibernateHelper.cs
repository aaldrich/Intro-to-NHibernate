using System;
using System.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateDemo.DataAccess.NHibernate_Setup;
using NHibernateDemo.Entities.Employees;

namespace NHibernateDemo.UnitTests.NHibernate_Setup
{
    public class UnitTestNHibernateHelper : INHibernateHelper 
    {
        static ISessionFactory session_factory;
        static Configuration config;
        IDbConnection connection;
        ISession current_session;

        public UnitTestNHibernateHelper()
        {
            initialize();
        }

        public void initialize()
        {
            session_factory = 
                Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Employee>().ExportTo(@"C:\mappings"))
                .ExposeConfiguration(x=>config = x)
                .BuildSessionFactory();

            connection = session_factory.OpenSession().Connection; //setup an initial connection

            initialize_nhibernate_profiler(); //If you have NHibernate Profiler
        }

        static void create_db(Configuration configuration)
        {
            new SchemaExport(configuration).Create(false,true);
        }

        public ISessionFactory SessionFactory
        {
            get { return session_factory; }
            set { session_factory = value; }
        }

        public ISession open_session()
        {
            current_session = session_factory.OpenSession(connection); //Pass connection for In Memory Database
            return current_session;
        }

        public ISession get_new_session_with_clean_database()
        {
            create_db();
            return open_session();
        }

        public virtual void create_db()
        {
            connection = session_factory.OpenSession().Connection;

            new SchemaExport(config).Execute(false, true, false, connection, null);
        }

        public void dispose_current_session()
        {
            current_session.Dispose();
        }

        public ISession get_current_session()
        {
            return current_session; 
        }

        private static void initialize_nhibernate_profiler()
        {
            HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize();
        }
    }
}