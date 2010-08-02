using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernateDemo.Entities.Managers;

namespace NHibernateDemo.DataAccess.Managers
{
    public class ManagerRepository : IManagerRepository
    {
        readonly ISession session;

        public ManagerRepository(ISession session)
        {
            this.session = session;
        }

        public Manager get_by_id(int id)
        {
            return session.Get<Manager>(id);
        }

        public Manager save(Manager manager)
        {
            using(var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(manager);
                transaction.Commit();
            }

            return manager;
        }

        public void delete(Manager manager)
        {
            using(var transaction = session.BeginTransaction())
            {
                session.Delete(manager);
                transaction.Commit();
            }
        }

        public IEnumerable<Manager> get_all()
        {
            return from m in session.Linq<Manager>() select m;
        }
    }
}