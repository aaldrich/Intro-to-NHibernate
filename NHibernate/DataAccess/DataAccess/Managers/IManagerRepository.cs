using System.Collections.Generic;
using NHibernateDemo.Entities.Managers;

namespace NHibernateDemo.DataAccess.Managers
{
    public interface IManagerRepository
    {
        Manager get_by_id(int id);
        Manager save(Manager manager);
        void delete(Manager manager);
        IEnumerable<Manager> get_all();
    }
}