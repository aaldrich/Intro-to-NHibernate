using System;
using NHibernate;
using NHibernateDemo.Entities.Employees;
using NHibernateDemo.Entities.Managers;


namespace NHibernateDemo.UnitTests.Setup
{
    public class GlobalDataSetup : IDisposable
    {
        public static ISession Session { get; set; }
       
        public static Employee get_stub_employee_values()
        {
            Employee employee = new Employee
                                    {
                                        FirstName = "Peter",
                                        LastName = "Gibbons"
                                    };
            return employee;
        }

        public static Employee add_stub_employee_to_database()
        {
            Employee employee = get_stub_employee_values();
            employee.Id = -1;

            Session.SaveOrUpdate(employee);
            Session.Flush();

            Session.Clear(); //Do this to ensure our tests don't use the cache

            return employee;
        }

        public static Manager get_stub_manager_values()
        {
            Manager manager = new Manager()
                                  {
                                      FirstName = "Bill",
                                      LastName = "Lumberg"
                                  };
            return manager;

        }

        public static Manager add_stub_manager_to_database()
        {
            Manager manager = get_stub_manager_values();
            manager.Id = -1;

            Session.SaveOrUpdate(manager);
            Session.Flush();

            Session.Clear(); //Do this to ensure our tests don't use the cache

            return manager;
        }

        public static Manager add_stub_manager_with_2_employees_to_database()
        {
            var manager = add_stub_manager_to_database();
            var employee1 = add_stub_employee_to_database();
            var employee2 = add_stub_employee_to_database();
            employee2.FirstName = "Michael";
            employee2.LastName = "Bolton";
            Session.Save(employee2);

            manager.AddEmployee(employee1);
            manager.AddEmployee(employee2);
            Session.SaveOrUpdate(manager);
            Session.Flush();

            Session.Clear(); //Do this to ensure our tests don't use the cache

            return manager;
        }

        public void Dispose()
        {
            Session = null;
        }
    }
}