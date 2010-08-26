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
            throw new NotImplementedException();

            return session.Get<Manager>(id);
        }

        public Manager save(Manager manager)
        {
            throw new NotImplementedException();

            session.SaveOrUpdate(manager);
            session.Flush();
            return manager;
        }

        public void delete(Manager manager)
        {
            throw new NotImplementedException();

            session.Delete(manager);
            session.Flush();
        }

        public IEnumerable<Manager> get_all()
        {
            throw new NotImplementedException();

            return session.Linq<Manager>();
        }
    }
}