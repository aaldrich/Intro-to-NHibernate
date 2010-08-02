using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernateDemo.Entities.Employees;

namespace NHibernateDemo.DataAccess.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        readonly ISession session;

        public EmployeeRepository(ISession session)
        {
            this.session = session;
        }

        public Employee find_by_id(int id)
        {
            return session.Get<Employee>(id);
        }

        public IEnumerable<Employee> find_by_name(string first_name, string last_name)
        {
            return from employee in session.Linq<Employee>()
                   where employee.FirstName == first_name
                         && employee.LastName == last_name
                   select employee;
        }

        public void save(Employee employee)
        {
            using(var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(employee);
                transaction.Commit();
            }
        }

        public void delete(Employee employee)
        {
            using(var transaction = session.BeginTransaction())
            {
                session.Delete(employee);
                transaction.Commit();
            }
        }

        public IEnumerable<Employee> get_all()
        {
            return from employee in session.Linq<Employee>() select employee;
        }
    }
}