using System.Collections.Generic;
using Iesi.Collections.Generic;
using NHibernateDemo.Entities.Employees;

namespace NHibernateDemo.Entities.Managers
{
    public class Manager
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual ISet<Employee> Employees { get; set;}

        public virtual IEnumerable<Employee> GetEmployees()
        {
            if (Employees == null)
            {
                Employees = new HashedSet<Employee>();
            }

            return Employees;
        }

        public virtual IEnumerable<Employee> AddEmployee(Employee employee)
        {
            if (Employees == null)
            {
                Employees = new HashedSet<Employee>();
            }

            Employees.Add(employee);

            return Employees;
        }

        public virtual IEnumerable<Employee> RemoveEmployee(Employee employee)
        {
            if (Employees == null)
            {
                Employees = new HashedSet<Employee>();
            }
            else
            {
                Employees.Remove(employee);
            }
            
            return Employees;
        }

        public Manager()
        {
            this.FirstName = "";
            this.LastName = "";
            this.Id = -1;
        }
    }
}