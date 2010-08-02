using System.Collections.Generic;
using NHibernateDemo.Entities.Employees;

namespace NHibernateDemo.DataAccess.Employees
{
    public interface IEmployeeRepository
    {
        Employee find_by_id(int id);
        IEnumerable<Employee> find_by_name(string first_name, string last_name);
        void save(Employee employee);
        void delete(Employee employee);
        IEnumerable<Employee> get_all();
    }
}