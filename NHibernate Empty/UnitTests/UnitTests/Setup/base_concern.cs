using Machine.Specifications;
using NHibernate;
using NHibernateDemo.UnitTests.NHibernate_Setup;

namespace NHibernateDemo.UnitTests.Setup
{
    public abstract class base_concern
    {
        Establish c = () =>
        {
            helper = new UnitTestNHibernateHelper();
            session = helper.get_new_session_with_clean_database();
            GlobalDataSetup.Session = session;
        };

        protected static ISession session;
        protected static UnitTestNHibernateHelper helper;
    }
}