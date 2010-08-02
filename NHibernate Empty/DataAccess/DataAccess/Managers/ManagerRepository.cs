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
        }

        public Manager save(Manager manager)
        {
            throw new NotImplementedException();
        }

        public void delete(Manager manager)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Manager> get_all()
        {
            throw new NotImplementedException();
        }
    }
}