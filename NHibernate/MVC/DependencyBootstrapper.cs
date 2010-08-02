using NHibernate;
using NHibernateDemo.DataAccess.Employees;
using NHibernateDemo.DataAccess.Managers;
using NHibernateDemo.DataAccess.NHibernate_Setup;
using StructureMap.Configuration.DSL;

namespace NHibernateDemo.MVC
{
    public class DependencyBootstrapper : Registry
    {
        public DependencyBootstrapper()
        {
            For<INHibernateHelper>().Singleton().Use<NHibernateHelper>();
            For<IEmployeeRepository>().Use<EmployeeRepository>();
            For<IManagerRepository>().Use<ManagerRepository>();
            
            //Tell Structuremap to inject the ISession from the NHibernateHelper.get_current_session() method
            //Hybrid scoped will mean the session will only exist for the length of the http request or thread
            //that is available.
            For<ISession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(x => x.GetInstance<INHibernateHelper>().get_current_session());
        }        
    }
}