using System;
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
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> find_by_name(string first_name, string last_name)
        {
            throw new NotImplementedException();
        }

        public void save(Employee employee)
        {
            throw new NotImplementedException();
        }

        public void delete(Employee employee)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> get_all()
        {
            throw new NotImplementedException();
        }
    }
}