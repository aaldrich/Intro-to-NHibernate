using System;
using NHibernate;

namespace NHibernateDemo.DataAccess.NHibernate_Setup
{
    public interface INHibernateHelper
    {
        void initialize();
        ISessionFactory SessionFactory { get; set; }
        ISession open_session();
        void create_db();
        void dispose_current_session();
        ISession get_current_session();
    }
}